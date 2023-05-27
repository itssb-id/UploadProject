    using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UploadProject.Data;
using UploadProject.Models;

namespace UploadProject.Pages;

//[Authorize]
public class AddUserModel : PageModel
{
    [BindProperty]
    public string Username { get; set; } = string.Empty;

    [BindProperty]
    public string Password { get; set; } = string.Empty;

    private readonly ILogger<User> logger;

    private ApplicationDbContext db;

    public AddUserModel(ILogger<User> _logger, ApplicationDbContext _db)
    {
        this.logger = _logger;
        this.db = _db;
    }

    public void OnGet()
    {
    }

    public async Task<ActionResult> OnPostAsync()
    {
        if (this.Username == "")
        {
        }

        return RedirectToPage("");
    }
}