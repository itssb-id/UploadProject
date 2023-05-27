using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO.Compression;
using UploadProject.Data;
using UploadProject.Models;
using UploadProject.ViewModels;

namespace UploadProject.Pages
{
    public class AdminModel : PageModel
    {
        public List<string> strings = new List<string>();
        private ApplicationDbContext db;
        public List<User> Users = new();
        public List<CompetitionSession> CompetitionSessions = new List<CompetitionSession>();
        public List<CompetitorUploadedFile> CompetitorUploadedFiles = new List<CompetitorUploadedFile>();
        public List<CompetitorAnswerHistory> CompetitorAnswerHistory = new List<CompetitorAnswerHistory>();

        [BindProperty]
        public string SelectedID { get; set; } = "";

        public User Admin;
        public bool VisibleAlert = true;

        [BindProperty]
        public Guid AdminID { get; set; }
        [BindProperty]
        public IFormFile UploadedFile { get; set; }
        [BindProperty]
        public DateTime StartDate { get; set; } = DateTime.Now.Date;
        [BindProperty]
        public DateTime EndDate { get; set; } = DateTime.Now.Date;
        [BindProperty]
        public String SessionName { get; set; }
        [BindProperty]
        public int DayNumber { get; set; } = 1;

        [BindProperty]
        public Guid ModuleID { get; set; }

        [BindProperty]
        public string ButtonValue { get; set; } = "add";

        public AdminModel(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void OnGet(Guid adminID)
        {
            //db.CompetitionSessions.RemoveRange(db.CompetitionSessions.ToList());
            //db.SaveChanges();

            //db.AdminUploadedFiles.RemoveRange(db.AdminUploadedFiles.ToList());
            //db.SaveChanges();

            //db.CompetitorUploadedFiles.RemoveRange(db.CompetitorUploadedFiles.ToList());
            //db.SaveChanges();

            var checkAdmin = db.Users.FirstOrDefault(x => x.ID == adminID);

            if (checkAdmin != null)
            {
                this.Admin = checkAdmin;
            }


            var sessionNow = db.CompetitionSessions.FirstOrDefault(x => x.StartDateTime >= DateTime.Now && x.EndDateTime <= DateTime.Now);

            Users = db.Users.Where(x => !x.IsAdmin).OrderBy(x => x.Username).ToList();
            CompetitionSessions = db.CompetitionSessions.OrderBy(x => x.StartDateTime).ToList();
            CompetitorUploadedFiles = db.CompetitorUploadedFiles.ToList();

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

            this.CompetitionSessions = db.CompetitionSessions.OrderBy(x => x.DayNumber).ToList();
            this.CompetitorUploadedFiles = db.CompetitorUploadedFiles.ToList();
            this.CompetitorAnswerHistory = this.CompetitorUploadedFiles.Select((x) =>
            {
                var listExtract = new List<string>();

                using (ZipArchive archive = ZipFile.OpenRead(Path.Combine(folderPath, x.FileName)))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        listExtract.Add(entry.FullName);
                    }
                }

                return new CompetitorAnswerHistory()
                {
                    UserId = x.UserID,
                    CompetitionSessionID = x.CompetitionSessionID,
                    CreatedAt = x.CreatedAt,
                    UploadedFileZipExtractList = listExtract
                };
            }).OrderByDescending(x => x.CreatedAt).ToList();
        }

        public IActionResult OnPostAsync()
        {
            if (StartDate > EndDate)
            {
                TempData["alertMessage"] = "Start date cannot more than end date";
                return Redirect("/Admin/" + this.AdminID);
            }

            string action = Request.Form["action"];
            switch (action)
            {
                case "delete":
                    var dataDelete = db.CompetitionSessions.FirstOrDefault(x => x.ID == ModuleID);
                    var dataDeleteAdmin = db.AdminUploadedFiles.Where(x => x.CompetitionSessionID == dataDelete.ID);
                    db.AdminUploadedFiles.RemoveRange(dataDeleteAdmin);
                    db.SaveChanges();
                    db.CompetitionSessions.Remove(dataDelete);
                    db.SaveChanges();
                    return Redirect("/Admin/" + this.AdminID);

                case "download":
                    var folderPathA = Directory.GetCurrentDirectory() + "/File/TestProjects/";
                    var a = db.CompetitionSessions.FirstOrDefault(x => x.ID == ModuleID);
                    var b = db.AdminUploadedFiles.FirstOrDefault(x => x.CompetitionSessionID == a.ID);
                    var filePath = Path.Combine(folderPathA, b.FileName);
                    var fileContentType = "application/zip"; // set the content type to indicate a zip file
                    var compressedFileName = $"{Path.GetFileNameWithoutExtension(filePath)}";
                    return PhysicalFile(filePath, fileContentType);
                
            }



            if (!UploadedFile.FileName.Contains(".zip"))
                return Redirect("/Admin/" + this.AdminID);

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
            db.CompetitionSessions.Add(new CompetitionSession
            {
                ID = competitionSessionID,
                Name = this.SessionName,
                DayNumber = this.DayNumber,
                StartDateTime = this.StartDate,
                EndDateTime = this.EndDate
            });
            db.SaveChanges();

            db.AdminUploadedFiles.Add(new AdminUploadedFile
            {
                FileName = UploadedFile.FileName,
                CreatedAt = DateTime.Now,
                CompetitionSessionID = competitionSessionID,
                UserID = AdminID,
            });
            db.SaveChanges();


            return Redirect("/Admin/" + this.AdminID);
        }
    }
}