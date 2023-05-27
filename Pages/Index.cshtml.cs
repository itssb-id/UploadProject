using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UploadProject.Data;

namespace UploadProject.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private ApplicationDbContext db;

    public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext _db)
    {
        _logger = logger;
        this.db = _db;
    }

    public IActionResult OnGet()
    {
        var userID = Request.Cookies["UserID"];

        if(userID != null && userID != "")
        {
            var user = db.Users.FirstOrDefault(x => x.ID == new Guid(userID));
            if (user != null)
            {
                if(user.IsAdmin)
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
