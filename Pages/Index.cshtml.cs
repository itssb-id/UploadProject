using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UploadProject.Data;

namespace UploadProject.Pages;

public class IndexModel(ILogger<IndexModel> logger, ApplicationDbContext db) : PageModel
{
    private readonly ILogger<IndexModel> _logger = logger;
    private readonly ApplicationDbContext _db = db;

    public IActionResult OnGet()
    {
        var userID = Request.Cookies["UserID"];

        if (!string.IsNullOrEmpty(userID))
        {
            var user = _db.Users.SingleOrDefault(x => x.ID == Guid.Parse(userID));
            if (user != null)
            {
                if (user.IsAdmin)
                {
                    return Redirect($"/Admin/{user.ID}");
                }
                else
                {
                    return Redirect($"/Competitor/{user.ID}");
                }
            }
        }

        return Page();
    }
}
