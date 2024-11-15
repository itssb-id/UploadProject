using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UploadProject.Data;

namespace UploadProject.Pages
{
    public class LoginModel(ApplicationDbContext db, /*SignInManager<IdentityUser> SignInManager,*/ ILogger<LoginModel> logger) : PageModel
    {
        public bool VisibleAlert = false;

        [Required]
        [BindProperty]
        public string Username { get; set; } = string.Empty;

        [Required]
        [BindProperty]
        public string Password { get; set; } = string.Empty;

        private readonly ApplicationDbContext _db = db;
        //private readonly SignInManager<IdentityUser> _signInManager = SignInManager;
        private readonly ILogger<LoginModel> _logger = logger;

        //
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (Username == "" || Password == "")
            {
                VisibleAlert = true;
                ModelState.AddModelError("Error", "Please insert Username & Password!");
                return Page();
            }

            var user = await _db.Users.SingleOrDefaultAsync(x => x.Username == Username && x.Password == Password);

            if (user == null)
            {
                VisibleAlert = true;
                ModelState.AddModelError("Error", "User not found!");
                return Page();
            }

            // Response.Cookies.Append("UserID", user.ID.ToString());
            _logger.LogInformation("UserID: {userId}", user.ID);
            HttpContext.Session.SetString("UserID", user.ID.ToString());
            
            //await _signInManager.SignInAsync(new IdentityUser(user.Username), true);


            if (user.IsAdmin)
            {
                return Redirect($"/Admin");
            }
            else
            {
                return Redirect($"/Competitor/{user.ID}");
            }
        }
    }
}