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

        // Pagination
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public int TotalPages { get; set; }

        public int PageSize { get; set; } = 10; // albums per page

        // CLASS AND TABLE NAME
        public List<Album> Albums { get; set; } = new List<Album>();
        public void OnGet()
{
    using (var db = new chinookDb())
    {
        var query = db.Albums
                      .Include(a => a.Artist)
                      .AsQueryable();

        // Search filter
        if (!string.IsNullOrEmpty(SearchTerm))
        {
            query = query.Where(a =>
                EF.Functions.Like(a.Title, $"%{SearchTerm}%") ||
                EF.Functions.Like(a.Artist.Name, $"%{SearchTerm}%"));
        }

        // get total records count
        int totalRecords = query.Count();

        // Calculate total pages
        TotalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);

        // apply ordering + pagination
        Albums = query
                 .OrderBy(a => a.Artist.Name)
                 .ThenBy(a => a.Title)
                 .Skip((PageNumber - 1) * PageSize)
                 .Take(PageSize)
                 .ToList();
    }
}

    }
}