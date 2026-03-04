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
        // search
        [BindProperty(SupportsGet = true)]
public string SearchTerm { get; set; } = string.Empty;

        // CLASS AND TABLE NAME
        public List<Album> Albums { get; set; } = new List<Album>();

         public void OnGet()
        {
            // Create DbContext
            using (var db = new chinookDb())
            {
                var query = db.Albums
                              .Include(a => a.Artist)
                              .AsQueryable();

                // search filter if SearchTerm is not empty
                if (!string.IsNullOrEmpty(SearchTerm))
                {
                    query = query.Where(a =>
                        EF.Functions.Like(a.Title, $"%{SearchTerm}%") ||
                        EF.Functions.Like(a.Artist.Name, $"%{SearchTerm}%"));
                }

                // Execute query and order results
                Albums = query
                         .OrderBy(a => a.Artist.Name)
                         .ThenBy(a => a.Title)
                         .ToList();
            }
        }

    }
}