using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UploadProject.Models;

namespace UploadProject.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            HttpContext.Session.Clear();
            //Response.Cookies.Delete("UserID");
            return Redirect("/");
        }
    }
}
