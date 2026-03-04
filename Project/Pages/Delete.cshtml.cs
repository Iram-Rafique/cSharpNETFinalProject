using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using albumsEntities;
using albumsContext;
using System.Collections.Generic;
using System.Linq;

namespace Project.Pages
{
    public class DeleteModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int AlbumID { get; set; }

        public Album? Album { get; set; }
        public List<Track> Tracks { get; set; } = new();
        //first show all details on the page 
        public void OnGet(int AlbumID)
        {
            this.AlbumID = AlbumID;

            using (var db = new chinookDb())
            {
                Album = db.Albums
                          .Include(a => a.Artist)
                          .FirstOrDefault(a => a.AlbumId == AlbumID);

                Tracks = db.Tracks
                           .Where(t => t.AlbumId == AlbumID)
                           .ToList();
            }
        }
        //then delete
        public IActionResult OnPost(int AlbumID, string confirm)
        {
            if (confirm != "yes")
            {
                return RedirectToPage("/Index");
            }

            using (var db = new chinookDb())
            {
                var album = db.Albums
                              .Include(a => a.Tracks)
                              .FirstOrDefault(a => a.AlbumId == AlbumID);

                if (album != null)
                {
                    // delete all tracks first (because of foreign key)
                    var tracks = db.Tracks.Where(t => t.AlbumId == AlbumID).ToList();
                    db.Tracks.RemoveRange(tracks);

                    // Delete the album
                    db.Albums.Remove(album);

                    db.SaveChanges();
                }
            }

            return RedirectToPage("/Index");
        }
    }
}