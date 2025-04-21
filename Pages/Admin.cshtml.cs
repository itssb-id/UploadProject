using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO.Compression;
using UploadProject.Data;
using UploadProject.Models;
using UploadProject.ViewModels;

namespace UploadProject.Pages
{
    public class AdminModel(ApplicationDbContext db, ILogger<AdminModel> logger) : PageModel
    {
        public List<string> strings = [];
        public readonly ApplicationDbContext _db = db;
        public List<Models.User> Users = [];
        public List<CompetitionSession> CompetitionSessions = [];
        public List<CompetitorUploadedFile> CompetitorUploadedFiles = [];
        public List<CompetitorAnswerHistory> CompetitorAnswerHistory = [];
        private readonly ILogger<AdminModel> _logger = logger;

        [BindProperty]
        public string SelectedID { get; set; } = "";

        public Models.User? Admin;
        public bool VisibleAlert = true;

        [BindProperty]
        public Guid AdminID { get; set; }
        [BindProperty]
        public IFormFile? UploadedFile { get; set; }
        [BindProperty]
        public DateTime StartDate { get; set; } = DateTime.Now.Date;
        [BindProperty]
        public DateTime EndDate { get; set; } = DateTime.Now.Date;
        [BindProperty]
        public string SessionName { get; set; } = "";
        [BindProperty]
        public int DayNumber { get; set; } = 1;

        [BindProperty]
        public Guid ModuleID { get; set; }

        [BindProperty]
        public string ButtonValue { get; set; } = "add";

        public IActionResult OnGet()
        {
            var userID =  HttpContext.Session.GetString("UserID");
            if (string.IsNullOrEmpty(userID))
            {
                return Redirect("/");
            }

            var id = Guid.Parse(userID!);

            var user = _db.Users.SingleOrDefault(x => x.ID == id);

            if (user != null)
            {
                Admin = user;
            }
            //db.CompetitionSessions.RemoveRange(db.CompetitionSessions.ToList());
            //db.SaveChanges();

            //db.AdminUploadedFiles.RemoveRange(db.AdminUploadedFiles.ToList());
            //db.SaveChanges();

            //db.CompetitorUploadedFiles.RemoveRange(db.CompetitorUploadedFiles.ToList());
            //db.SaveChanges();

            //var checkAdmin = _db.Users.SingleOrDefault(x => x.ID == adminID);

            //if (checkAdmin != null)
            //{
            //    Admin = checkAdmin;
            //}


            var sessionNow = _db.CompetitionSessions.FirstOrDefault(x => x.StartDateTime >= DateTime.Now && x.EndDateTime <= DateTime.Now);

            Users = _db.Users.Where(x => !x.IsAdmin).OrderBy(x => x.Username).ToList();
            CompetitionSessions = _db.CompetitionSessions.OrderBy(x => x.StartDateTime).ToList();
            CompetitorUploadedFiles = _db.CompetitorUploadedFiles.ToList();

            var folderPath = Directory.GetCurrentDirectory() + "/File";

            //string zipPath = Path.Combine(folderPath, "ukk_twitter-main.zip");
            //string zipPath = Path.Combine(folderPath, "BooKu.zip");

            //using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            //{
            //    foreach (ZipArchiveEntry entry in archive.Entries)
            //    {
            //        if (entry.FullName != "")
            //        {
            //            strings.Add(entry.FullName);
            //        }
            //    }
            //}

            CompetitionSessions = _db.CompetitionSessions.OrderBy(x => x.DayNumber).ToList();
            CompetitorUploadedFiles = _db.CompetitorUploadedFiles.ToList();
            CompetitorAnswerHistory = CompetitorUploadedFiles.Select((x) =>
            {
                var listExtract = new List<string>();
                var filePath = Path.Combine(folderPath, x.FileName);
                if (System.IO.File.Exists(filePath))
                {
                    try
                    {
                        using ZipArchive archive = ZipFile.OpenRead(filePath);
                        listExtract.AddRange(archive.Entries.Select(x => x.FullName));
                    }
                    catch (Exception) { }
                }

                return new CompetitorAnswerHistory()
                {
                    UserId = x.UserID,
                    CompetitionSessionID = x.CompetitionSessionID,
                    CreatedAt = x.CreatedAt,
                    UploadedFileZipExtractList = listExtract
                };
            }).OrderByDescending(x => x.CreatedAt).ToList();
            return Page();
        }

        public IActionResult OnPostAsync()
        {
            if (StartDate > EndDate)
            {
                TempData["alertMessage"] = "Start date cannot more than end date";
                return Redirect("/Admin");
            }

            string? action = Request.Form["action"];
            switch (action)
            {
                case "delete":
                    var dataDelete = _db.CompetitionSessions.FirstOrDefault(x => x.ID == ModuleID);
                    var dataDeleteAdmin = _db.AdminUploadedFiles.Where(x => x.CompetitionSessionID == dataDelete.ID);
                    _db.AdminUploadedFiles.RemoveRange(dataDeleteAdmin);
                    _db.SaveChanges();
                    _db.CompetitionSessions.Remove(dataDelete);
                    _db.SaveChanges();
                    return Redirect("/Admin");

                case "download":
                    var folderPathA = Directory.GetCurrentDirectory() + "/File/TestProjects/";
                    var a = _db.CompetitionSessions.FirstOrDefault(x => x.ID == ModuleID);
                    var b = _db.AdminUploadedFiles.FirstOrDefault(x => x.CompetitionSessionID == a.ID);
                    var filePath = Path.Combine(folderPathA, b.FileName);
                    var fileContentType = "application/zip"; // set the content type to indicate a zip file
                    var compressedFileName = $"{Path.GetFileNameWithoutExtension(filePath)}";
                    if (System.IO.File.Exists(filePath))
                        return PhysicalFile(filePath, fileContentType, compressedFileName);
                    else return Redirect("/Admin");
                // return PhysicalFile(filePath, fileContentType);
                case "downloadAnswer":
                    return DownloadAnswer(ModuleID);
                case "editDatetime":
                    var dataEdit = _db.CompetitionSessions.FirstOrDefault(x => x.ID == ModuleID);
                    if(dataEdit == null) return Redirect("/Admin");
                    _logger.LogInformation("Time {start} {end}", StartDate, EndDate);
                    dataEdit.StartDateTime = StartDate;
                    dataEdit.EndDateTime = EndDate;
                    //dataEdit.Name = SessionName;
                    //dataEdit.DayNumber = DayNumber;
                    _db.SaveChanges();
                    return Redirect("/Admin");

            }



            if (!UploadedFile.FileName.Contains(".zip"))
                return Redirect("/Admin");

            var folderPath = Directory.GetCurrentDirectory() + "/File/TestProjects";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string zipPath = Path.Combine(folderPath, UploadedFile.FileName);

            using (var stream = System.IO.File.Create(zipPath))
            {
                UploadedFile.CopyTo(stream);
            }

            var competitionSessionID = Guid.NewGuid();
            _db.CompetitionSessions.Add(new CompetitionSession
            {
                ID = competitionSessionID,
                Name = SessionName,
                DayNumber = DayNumber,
                StartDateTime = StartDate,
                EndDateTime = EndDate
            });
            _db.SaveChanges();

            _db.AdminUploadedFiles.Add(new AdminUploadedFile
            {
                FileName = UploadedFile.FileName,
                CreatedAt = DateTime.Now,
                CompetitionSessionID = competitionSessionID,
                UserID = AdminID,
            });
            _db.SaveChanges();


            return Redirect("/Admin");
        }

        private FileContentResult DownloadAnswer(Guid SessionID)
        {
            var pathDir = Path.Combine(Directory.GetCurrentDirectory(), "File");
            _logger.LogInformation("download answer {sessionId}", ModuleID);
            var session = _db.CompetitionSessions.SingleOrDefault(x => x.ID == ModuleID);
            if (session == null) Redirect("/");
            //var pathFile = Path.Combine(pathDir, session!.Name + ".zip"));
            using var memoryStream = new MemoryStream();
            using var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true);

            
            var filenames = _db
                .CompetitorUploadedFiles
                .Where(x => x.CompetitionSessionID == SessionID && x.CreatedAt == _db.CompetitorUploadedFiles.Where(y => y.CompetitionSessionID == SessionID && y.UserID == x.UserID).Max(y => y.CreatedAt))
                .Select(x => x.FileName).ToList();
                
            foreach (var filename in filenames)
            {
                var path = Path.Combine(pathDir, filename);
                if (Path.Exists(path))
                {
                    var file = System.IO.File.OpenRead(path);
                    var zipEntry = archive.CreateEntry(filename, CompressionLevel.Fastest);
                    using var zipStream = zipEntry.Open();
                    file.CopyTo(zipStream);
                }
            }
            var fileContentType = "application/zip";
            return File(memoryStream.ToArray(), fileContentType, session!.Name + ".zip");
        }
    }
}