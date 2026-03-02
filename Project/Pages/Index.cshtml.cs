using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using albumsEntities;
using albumsContext;



// this must be the folder name which is our web name
namespace Project.Pages
{
    // HERE WE NEED crud operation name 
    public class IndexModel: PageModel
    {
        public string? Heading { get; set; }
        //CHAGE CLASS AND TABLE NAME
        public List<Album> Albums { get; set; } = new List<Album>();

        public void OnGet()
        {
            // Create and dispose DbContext safely
    using (var db = new chinookDb())
    {
        // Fetch albums with their artists and store in the public property
        Albums = db.Albums
                   .Include(a => a.Artist)
                   .ToList();
    }

        }

    }
}