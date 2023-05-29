using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Protocol;
using System.IO.Compression;
using UploadProject.Data;
using UploadProject.Models;
using UploadProject.ViewModels;

namespace UploadProject.Pages
{
    public class CompetitorModel : PageModel
    {
        [BindProperty]
        public string CompetitorID { get; set; } = string.Empty;

        [BindProperty]
        public IFormFile? UploadedFile { get; set; }

        [BindProperty]
        public string Action { get; set; } = string.Empty;

        public User? Competitor;
        public List<CompetitionSession> CompetitionSessions = new List<CompetitionSession>();
        public List<CompetitorUploadedFile> CompetitorUploadedFiles = new List<CompetitorUploadedFile>();
        public List<CompetitorAnswerHistory> CompetitorAnswerHistory = new List<CompetitorAnswerHistory>();

        public readonly ApplicationDbContext db;
        //private readonly IWebHostEnvironment _env;

        public CompetitorModel(ApplicationDbContext _db)
        {
            this.db = _db;
        }

        public void OnGet(Guid competitorID)
        {
            //db.CompetitionSessions.RemoveRange(db.CompetitionSessions.ToList());
            //db.SaveChanges();

            //db.AdminUploadedFiles.RemoveRange(db.AdminUploadedFiles.ToList());
            //db.SaveChanges();

            //db.CompetitorUploadedFiles.RemoveRange(db.CompetitorUploadedFiles.ToList());
            //db.SaveChanges();

            var folderPath = Directory.GetCurrentDirectory() + "/File";

            //string zipPath = Path.Combine(folderPath, "ukk_twitter-main.zip");
            //string zipPath = Path.Combine(folderPath, "BooKu.zip");

            //using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            //{
            //    foreach (ZipArchiveEntry entry in archive.Entries)
            //    {
            //        var isFolder = false;
            //        if (entry.FullName == "")
            //        {

            //        }
            //        strings.Add(entry.FullName);
            //    }
            //}

            var checkUser = db.Users.FirstOrDefault(x => x.ID == competitorID);

            if (checkUser != null)
            {
                this.Competitor = checkUser;
            }

            this.CompetitionSessions = db.CompetitionSessions.OrderBy(x => x.DayNumber).ToList();
            this.CompetitorUploadedFiles = db.CompetitorUploadedFiles.ToList().Where(x => x.UserID == competitorID).ToList();
            this.CompetitorAnswerHistory = this.CompetitorUploadedFiles.Select((x) =>
            {
                var listExtract = new List<string>();
                var pathFile = Path.Combine(folderPath, x.FileName);
                if (System.IO.File.Exists(pathFile))
                {
                    try
                    {
                        using ZipArchive archive = ZipFile.OpenRead(pathFile);
                        listExtract.AddRange(archive.Entries.Select(x => x.FullName));
                    }
                    catch (Exception) { }
                }

                return new CompetitorAnswerHistory()
                {
                    CompetitionSessionID = x.CompetitionSessionID,
                    CreatedAt = x.CreatedAt,
                    UploadedFileZipExtractList = listExtract
                };
            }).OrderByDescending(x => x.CreatedAt).ToList();

            //https://learn.microsoft.com/en-us/dotnet/api/system.io.compression.zipfile.open?view=net-7.0
            // https://learn.microsoft.com/en-us/dotnet/api/system.io.file?view=net-7.0

            //var zipPath = "D:\\cobaZip.zip";

            //if (!extractPath.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
            //extractPath += Path.DirectorySeparatorChar;
            //using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            //{
            //    foreach (ZipArchiveEntry entry in archive.Entries)
            //    {
            //        fileName.Add(entry.FullName);
            //    }
            //}
        }

        public List<string> strings = new List<string>();

        public IActionResult OnPostAsync()
        {
            this.Competitor = db.Users.FirstOrDefault(x => x.ID == new Guid(CompetitorID))!;

            if (this.Action == "DownloadTP")
            {
                var allActiveCompetitionSessions = db.CompetitionSessions.Where(x => DateTime.Now >= x.StartDateTime && DateTime.Now <= x.EndDateTime).ToList();

                if (allActiveCompetitionSessions.Count == 0)
                {
                    TempData["ErrorMessage"] = "There are no active competition sessions";
                    return Redirect("/Competitor/" + this.Competitor.ID);
                }

                using (var memoryStream = new MemoryStream())
                {
                    using (var downloadZip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        foreach (var competitionSession in allActiveCompetitionSessions)
                        {
                            var testProject = db.AdminUploadedFiles.Where(x => x.CompetitionSessionID == competitionSession.ID).ToList();
                            foreach (var item in testProject)
                            {
                                var folderPath = Directory.GetCurrentDirectory() + "/File/TestProjects";
                                var zipPath = Path.Combine(folderPath, item.FileName);

                                var entry = downloadZip.CreateEntry(item.FileName);
                                using (var fileStream = System.IO.File.OpenRead(zipPath))
                                using (var entryStream = entry.Open())
                                {
                                    fileStream.CopyTo(entryStream);
                                }
                            }
                        }
                    }

                    return File(memoryStream.ToArray(), "application/zip", $"Cases-{DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss")}.zip");
                }
            }
            else if (this.Action == "UploadAnswer")
            {
                if (!UploadedFile.FileName.Contains(".zip"))
                    return Page();

                var folderPath = Directory.GetCurrentDirectory() + "/File";
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var checkActiveCompetitionSession = db.CompetitionSessions.Where(x => DateTime.Now >= x.StartDateTime && DateTime.Now <= x.EndDateTime).ToList();
                if (checkActiveCompetitionSession.Count == 0)
                {
                    TempData["ErrorMessage"] = "There are no active competition sessions";
                    return Redirect("/Competitor/" + this.Competitor.ID);
                }

                var activeCompetitionSession = checkActiveCompetitionSession.FirstOrDefault();

                if (activeCompetitionSession == null)
                {
                    TempData["ErrorMessage"] = "There are no active competition sessions";
                    return Redirect("/Competitor/" + this.Competitor.ID);
                }

                var activeCompetitionSessionID = activeCompetitionSession.ID;

                var fileExtension = Path.GetExtension(UploadedFile.FileName);
                var modifiedFileName = $"{activeCompetitionSession.Name}_{Competitor.Username}_{DateTime.Now.ToString("yyyyMMddHHmmss")}{fileExtension}";
                var zipPath = Path.Combine(folderPath, modifiedFileName);

                try
                {
                    using (var stream = System.IO.File.Create(zipPath))
                    {
                        UploadedFile.CopyTo(stream);
                    }
                }
                catch (Exception)
                {
                    TempData["ErrorMessage"] = "Error upload.";
                    return Redirect("/Competitor/" + this.Competitor.ID);
                }

                db.CompetitorUploadedFiles.Add(new CompetitorUploadedFile
                {
                    UserID = Competitor.ID,
                    FileName = modifiedFileName,
                    CompetitionSessionID = activeCompetitionSessionID,
                    CreatedAt = DateTime.Now
                });
                db.SaveChanges();

                //using (ZipArchive archive = ZipFile.OpenRead(zipPath))
                //{
                //    foreach (ZipArchiveEntry entry in archive.Entries)
                //    {
                //        strings.Add(entry.FullName);
                //    }
                //}

                //Console.WriteLine(Competitor.ID.ToString());
            }

            return Redirect("/Competitor/" + this.Competitor.ID);
        }

        //public async Task<IActionResult> OnPostAsync()
        //{
        //    return Redirect("/Admin");
        //}

        //public IActionResult DownloadFile(string fileName)
        //{
        //    var filePath = Path.Combine(_env.ContentRootPath, "Files", fileName);
        //    var fileContentType = "application/zip"; // set the content type to indicate a zip file
        //    var compressedFileName = $"{Path.GetFileNameWithoutExtension(filePath)}.zip";

        //    using (var memoryStream = new MemoryStream())
        //    {
        //        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        //        {
        //            var entry = archive.CreateEntry(Path.GetFileName(filePath));

        //            using (var entryStream = entry.Open())
        //            using (var fileStream = new FileStream(filePath, FileMode.Open))
        //            {
        //                fileStream.CopyTo(entryStream);
        //            }
        //        }

        //        memoryStream.Seek(0, SeekOrigin.Begin);
        //        return File(memoryStream, fileContentType, compressedFileName);
        //    }
        //}
    }
}