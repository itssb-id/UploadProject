using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;
using UploadProject.Data;
using UploadProject.Models;
using UploadProject.ViewModels;

namespace UploadProject.Pages;

public class CompetitorModel(ApplicationDbContext db) : PageModel
{
    [BindProperty]
    public string CompetitorID { get; set; } = string.Empty;

    [BindProperty]
    public IFormFile? UploadedFile { get; set; }

    [BindProperty]
    public string Action { get; set; } = string.Empty;

    public Models.User? Competitor;
    public List<CompetitionSession> CompetitionSessions = [];
    public List<CompetitorUploadedFile> CompetitorUploadedFiles = [];
    public List<CompetitorAnswerHistory> CompetitorAnswerHistory = [];

    public readonly ApplicationDbContext _db = db;

    public IActionResult OnGet()
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

        var userID = HttpContext.Session.GetString("UserID");
        if (string.IsNullOrEmpty(userID))
        {
            return Redirect("/");
        }

        var id = Guid.Parse(userID!);

        var checkUser = _db.Users.FirstOrDefault(x => x.ID == id);

        if (checkUser != null)
        {
            Competitor = checkUser;
        }

        CompetitionSessions = _db.CompetitionSessions.OrderBy(x => x.DayNumber).ToList();
        CompetitorUploadedFiles = _db.CompetitorUploadedFiles.ToList().Where(x => x.UserID == id).ToList();
        CompetitorAnswerHistory = CompetitorUploadedFiles.Select((x) =>
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
        return Page();
    }

    public List<string> strings = [];

    public async Task<IActionResult> OnPostAsync()
    {
        Competitor = await _db.Users.FirstOrDefaultAsync(x => x.ID == new Guid(CompetitorID))!;

        if (Action == "DownloadTP")
        {
            var allActiveCompetitionSessions = _db.CompetitionSessions.Where(x => DateTime.Now >= x.StartDateTime && DateTime.Now <= x.EndDateTime).ToList();

            if (allActiveCompetitionSessions.Count == 0)
            {
                TempData["ErrorMessage"] = "There are no active competition sessions";
                return RedirectToPage();
            }

            using(var memoryStream = new MemoryStream())
            {
                using (var downloadZip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var competitionSession in allActiveCompetitionSessions)
                    {
                        var testProject = _db.AdminUploadedFiles.Where(x => x.CompetitionSessionID == competitionSession.ID).ToList();
                        foreach (var item in testProject)
                        {
                            var zipPath = Path.Combine(Directory.GetCurrentDirectory(), "File", "TestProjects", item.FileName);

                            var entry = downloadZip.CreateEntry(item.FileName);
                            using var fileStream = System.IO.File.OpenRead(zipPath);
                            using var entryStream = entry.Open();
                            fileStream.CopyTo(entryStream);
                        }
                    }
                }

                return File(memoryStream.ToArray(), "application/zip", $"TP-{DateTime.Now:yyyy-MM-dd_HH_mm_ss}.zip");
            }
        }
        else if (Action == "UploadAnswer")
        {
            if (!UploadedFile.FileName.Contains(".zip"))
                return Page();

            var folderPath = Directory.GetCurrentDirectory() + "/File";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var checkActiveCompetitionSession = _db.CompetitionSessions.Where(x => DateTime.Now >= x.StartDateTime && DateTime.Now <= x.EndDateTime).ToList();
            if (checkActiveCompetitionSession.Count == 0)
            {
                TempData["ErrorMessage"] = "There are no active competition sessions";
                return RedirectToPage();
            }

            var activeCompetitionSession = checkActiveCompetitionSession.FirstOrDefault();

            if (activeCompetitionSession == null)
            {
                TempData["ErrorMessage"] = "There are no active competition sessions";
                return RedirectToPage();
            }

            var activeCompetitionSessionID = activeCompetitionSession.ID;

            var fileExtension = Path.GetExtension(UploadedFile.FileName);
            var modifiedFileName = $"{activeCompetitionSession.Name}_{Competitor.Username}_{DateTime.Now:yyyyMMddHHmmss}{fileExtension}";
            var zipPath = Path.Combine(folderPath, modifiedFileName);

            try
            {
                using var stream = System.IO.File.Create(zipPath);
                UploadedFile.CopyTo(stream);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Error upload.";
                return RedirectToPage();
            }

            _db.CompetitorUploadedFiles.Add(new CompetitorUploadedFile
            {
                UserID = Competitor.ID,
                FileName = modifiedFileName,
                CompetitionSessionID = activeCompetitionSessionID,
                CreatedAt = DateTime.Now
            });
            _db.SaveChanges();

            //using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            //{
            //    foreach (ZipArchiveEntry entry in archive.Entries)
            //    {
            //        strings.Add(entry.FullName);
            //    }
            //}

            //Console.WriteLine(Competitor.ID.ToString());
        }

        return RedirectToPage();
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