using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using albumsEntities;
using albumsContext;




namespace Project.Pages
{
 
    public class IndexModel : PageModel
    {
        public string? Heading { get; set; }
        // CLASS AND TABLE NAME
        public List<Album> Albums { get; set; } = new List<Album>();

        public void OnGet()
        {
            // Create DbContext
            using (var db = new chinookDb())
            {
                // fetch albums with their artists 
                Albums = db.Albums
                           .Include(a => a.Artist)
                           .ToList();
            }

        }

    }
}