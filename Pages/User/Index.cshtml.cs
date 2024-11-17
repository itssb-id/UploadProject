using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace UploadProject.Pages.User
{
    public class IndexModel(Data.ApplicationDbContext context) : PageModel
    {
        private readonly Data.ApplicationDbContext _context = context;

        public IList<Models.User> User { get;set; } = default!;

        public async Task OnGetAsync()
        {
            var role = HttpContext.Session.GetString("Role");
            if(role != "Admin")
            {
                Response.Redirect("/Login");
            }
            User = await _context.Users.OrderBy(x => x.Username).ToListAsync();
        }
    }
}
