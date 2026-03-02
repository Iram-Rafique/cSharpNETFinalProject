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
    public class ViewAlbumDetailsModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int AlbumId { get; set; }

        public Album? Album { get; set; }
        public List<Track> Tracks { get; set; } = new();

        public void OnGet(int AlbumId)
        {
            this.AlbumId = AlbumId;

            using (var db = new chinookDb())
            {
                Album = db.Albums
                          .Include(a => a.Artist)
                          .FirstOrDefault(a => a.AlbumId == AlbumId);

                Tracks = db.Tracks
                           .Where(t => t.AlbumId == AlbumId)
                           .ToList();
            }
        }
    }
}


// namespace Project.Pages
// {
//     public class ViewAlbumDetailsModel : PageModel
//     {
//         [BindProperty(SupportsGet = true)]
//         public int AlbumID { get; set; }
//         public String? Title { get; set; }


//         public void OnGet(int AlbumID)
//         {
//             this.AlbumID = AlbumID; // stores route parameter
            
//         }
//     }
// }