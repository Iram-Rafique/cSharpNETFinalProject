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
    Albums = db.Albums
               .Include(a => a.Artist)
               .OrderBy(a => a.Artist.Name)   //  sort by artist
               .ThenBy(a => a.Title)          // then sort albums inside each artist
               .ToList();
}

        }

    }
}