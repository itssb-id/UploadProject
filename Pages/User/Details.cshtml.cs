using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UploadProject.Data;
using UploadProject.Models;

namespace UploadProject.Pages.User;

public class DetailsModel(ApplicationDbContext context) : PageModel
{
    private readonly ApplicationDbContext _context = context;

    public Models.User User { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        var role = HttpContext.Session.GetString("Role");
        if (role != "Admin")
        {
            Response.Redirect("/Login");
        }

        if (id == null)
        {
            return NotFound();
        }

        var user = await _context.Users.FirstOrDefaultAsync(m => m.ID == id);

        if (user is not null)
        {
            User = user;

            return Page();
        }

        return NotFound();
    }
}
