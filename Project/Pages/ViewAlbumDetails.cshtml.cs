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
        // binds AlbumId from the route (URL param)
        [BindProperty(SupportsGet = true)]
        public int AlbumId { get; set; }
        //  holds the selected album 
        public Album? Album { get; set; }
        public List<Track> Tracks { get; set; } = new();

        public void OnGet(int AlbumId)
        {
            this.AlbumId = AlbumId;

            using (var db = new chinookDb())
            {
                Album = db.Albums
                .Include(a => a.Artist)
                //   returns the album if found, otherwise returns null.
                .FirstOrDefault(a => a.AlbumId == AlbumId);

                Tracks = db.Tracks
                .Where(t => t.AlbumId == AlbumId)
                .ToList();
            }
        }
    }
}

