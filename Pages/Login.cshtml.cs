using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UploadProject.Data;

namespace UploadProject.Pages
{
    public class LoginModel : PageModel
    {
        public bool VisibleAlert = false;

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        private ApplicationDbContext db;

        public LoginModel(ApplicationDbContext _db)
        {
            this.db = _db;
        }

        //
        public IActionResult OnGet()
        {
            var userID = Request.Cookies["UserID"];

            if (userID != null && userID != "")
            {
                var user = db.Users.FirstOrDefault(x => x.ID == new Guid(userID));
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (this.Username == "" || this.Password == "")
            {
                VisibleAlert = true;
                ModelState.AddModelError("Error", "Please insert Username & Password!");
                return Page();
            }

            var user = await db.Users.FirstOrDefaultAsync(x => x.Username == this.Username && x.Password == this.Password);

            if (user == null)
            {
                VisibleAlert = true;
                ModelState.AddModelError("Error", "User not found!");
                return Page();
            }

            Response.Cookies.Append("UserID", user.ID.ToString());

            if (user.IsAdmin)
            {
                return Redirect($"/Admin/{user.ID}");
            }
            else
            {
                return Redirect($"Competitor/{user.ID}");
            }
        }
    }
}