using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.IO.Compression;
using UploadProject.Data;
using UploadProject.Models;
using UploadProject.ViewModels;

namespace UploadProject.Pages;

public class AdminModel(ApplicationDbContext db, ILogger<AdminModel> logger) : PageModel
{
    public List<string> strings = [];
    public readonly ApplicationDbContext _db = db;
    public List<Models.User> Users = [];
    public List<CompetitionSession> CompetitionSessions = [];
    public List<CompetitorUploadedFile> CompetitorUploadedFiles = [];
    public List<CompetitorAnswerHistory> CompetitorAnswerHistory = [];
    private readonly ILogger<AdminModel> _logger = logger;

    public Models.User? Admin;
    public bool VisibleAlert = true;

    [BindProperty]
    public UploadTestProjectViewModel UploadModel { get; set; } = new();

    [BindProperty]
    public TimeRange TimeRangeModel { get; set; } = new();


    public IActionResult OnGet()
    {
        var userID = HttpContext.Session.GetString("UserID");
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

    public IActionResult OnPostDownloadTestProjectAsync(Guid id)
    {
        var folderPathA = Path.Combine(Directory.GetCurrentDirectory(), "File", "TestProjects");
        var competitionSession = _db.CompetitionSessions.FirstOrDefault(x => x.ID == id);
        if (competitionSession == null) 
            return RedirectToPage();
        var testProject = _db.AdminUploadedFiles.FirstOrDefault(x => x.CompetitionSessionID == competitionSession.ID);
        if (testProject == null) 
            return RedirectToPage();
        var filePath = Path.Combine(folderPathA, testProject.FileName);
        var fileContentType = "application/zip"; // set the content type to indicate a zip file
        var compressedFileName = $"{Path.GetFileNameWithoutExtension(filePath)}_{DateTime.Now.ToFileTime()}{Path.GetExtension(filePath)}";
        _logger.LogInformation("download test project {filePath} {compressedFileName}", filePath, compressedFileName);
        if (!System.IO.File.Exists(filePath)) 
            return RedirectToPage();
        return PhysicalFile(filePath, fileContentType, compressedFileName);
    }

    public async Task<IActionResult> OnPostEditDateTimeAsync(Guid id)
    {
        if(TimeRangeModel == null) return RedirectToPage();
        if (TimeRangeModel.Start >= TimeRangeModel.End)
        {
            TempData["alertMessage"] = "Start date cannot more than end date";
            return RedirectToPage();
        }
        var competitionSession = await _db.CompetitionSessions.FirstOrDefaultAsync(x => x.ID == id);
        if (competitionSession == null)
        {
            return RedirectToPage();
        }
        _logger.LogInformation("Time {start} {end}", TimeRangeModel.Start, TimeRangeModel.End);
        competitionSession.StartDateTime = TimeRangeModel.Start;
        competitionSession.EndDateTime = TimeRangeModel.End;
        await _db.SaveChangesAsync();
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDownloadAllAnswerAsync(Guid id)
    {
        var pathDir = Path.Combine(Directory.GetCurrentDirectory(), "File");
        _logger.LogInformation("download answer {sessionId}", id);
        var CompetitionSession = await _db.CompetitionSessions.SingleOrDefaultAsync(x => x.ID == id);
        if (CompetitionSession == null)
        {
            return RedirectToPage();
        }
        //var pathFile = Path.Combine(pathDir, session!.Name + ".zip"));
        using (var memoryStream = new MemoryStream())
        {
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                var filenames = await _db
                .CompetitorUploadedFiles
                .Where(x => x.CompetitionSessionID == id && x.CreatedAt == _db.CompetitorUploadedFiles.Where(y => y.CompetitionSessionID == id && y.UserID == x.UserID).Max(y => y.CreatedAt))
                .Select(x => x.FileName)
                .ToListAsync();

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
            }

            var fileContentType = "application/zip";
            return File(memoryStream.ToArray(), fileContentType, $"{CompetitionSession.Name}_{DateTime.Now.ToFileTime()}.zip");
        }
    }

    public async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        var competitionSession = await _db.CompetitionSessions.FirstOrDefaultAsync(x => x.ID == id);
        if (competitionSession == null)
        {
            return RedirectToPage();
        }
        var dataDeleteAdmin = _db.AdminUploadedFiles.Where(x => x.CompetitionSessionID == competitionSession.ID);
        _db.AdminUploadedFiles.RemoveRange(dataDeleteAdmin);
        _db.CompetitionSessions.Remove(competitionSession);
        await _db.SaveChangesAsync();
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var userID = HttpContext.Session.GetString("UserID");
        if (string.IsNullOrEmpty(userID))
        {
            return Redirect("/");
        }

        var id = Guid.Parse(userID!);

        var user = await _db.Users.SingleOrDefaultAsync(x => x.ID == id);
        if (user == null || !user.IsAdmin) return RedirectToPage("./");
        if (UploadModel.StartDate >= UploadModel.EndDate)
        {
            ModelState.TryAddModelError("StartDate", "Start date cannot more than end date");
        }
        if (!ModelState.IsValid)
        {
            _logger.LogInformation("{err}", ModelState.Keys.Select(x => $"{x} - {ModelState[x]}").ToArray());
            return Page();
        }
        if (UploadModel == null) return RedirectToPage();
        if (UploadModel.StartDate >= UploadModel.EndDate)
        {
            TempData["alertMessage"] = "Start date cannot more than end date";
            _logger.LogInformation("Start date cannot more than end date");
            return RedirectToPage();
        }

        if (UploadModel.UploadedFile == null || !UploadModel.UploadedFile.FileName.Contains(".zip"))
            return RedirectToPage();

        var folderPath = Directory.GetCurrentDirectory() + "/File/TestProjects";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string zipPath = Path.Combine(folderPath, UploadModel.UploadedFile.FileName);

        using var stream = System.IO.File.Create(zipPath);
        UploadModel.UploadedFile.CopyTo(stream);

        var competitionSession = new CompetitionSession
        {
            Name = UploadModel.SessionName,
            DayNumber = UploadModel.DayNumber,
            StartDateTime = UploadModel.StartDate,
            EndDateTime = UploadModel.EndDate
        };
        await _db.CompetitionSessions.AddAsync(competitionSession);

        await _db.AdminUploadedFiles.AddAsync(new AdminUploadedFile
        {
            FileName = UploadModel.UploadedFile.FileName,
            CreatedAt = DateTime.Now,
            CompetitionSessionID = competitionSession.ID,
            UserID = user.ID,
        });
        await _db.SaveChangesAsync();


        return RedirectToPage();
    }
}

public class UploadTestProjectViewModel
{
    [Required]
    public string SessionName { get; set; } = "";
    [Required]
    public int DayNumber { get; set; } = 1;
    public DateTime StartDate { get; set; } = DateTime.Now.Date;
    public DateTime EndDate { get; set; } = DateTime.Now.Date;
    [Required]
    public IFormFile? UploadedFile { get; set; }
}

public class TimeRange
{
    public DateTime Start { get; set; } =  DateTime.Now.Date;
    public DateTime End { get; set; } = DateTime.Now.Date;
}