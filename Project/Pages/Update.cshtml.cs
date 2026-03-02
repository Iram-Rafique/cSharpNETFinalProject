// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.RazorPages;
// using Microsoft.EntityFrameworkCore;
// using albumsEntities;
// using albumsContext;
// using System.Collections.Generic;
// using System.Linq;

// namespace Project.Pages
// {
//     public class UpdateModel : PageModel
//     {
//         [BindProperty(SupportsGet = true)]
//         public int AlbumID { get; set; }

//         [BindProperty]
//         public Album? Album { get; set; }

//         public List<Artist> Artists { get; set; } = new();

//         public void OnGet(int AlbumID)
//         {
//             this.AlbumID = AlbumID;

//             using (var db = new chinookDb())
//             {
//                 Album = db.Albums.FirstOrDefault(a => a.AlbumId == AlbumID);
//                 Artists = db.Artists.ToList();
//             }
//         }

//         public IActionResult OnPost(int AlbumID, string Title, int ArtistId)
//         {
//             using (var db = new chinookDb())
//             {
//                 var album = db.Albums.FirstOrDefault(a => a.AlbumId == AlbumID);
//                 if (album != null)
//                 {
//                     album.Title = Title;
//                     album.ArtistId = ArtistId;

//                     db.SaveChanges();
//                 }
//             }

//             return RedirectToPage("/Index");
//         }
//     }
// }
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using albumsEntities;
using albumsContext;
using System.Collections.Generic;

namespace Project.Pages
{
    public class UpdateModel : PageModel
    {
        chinookDb db = new chinookDb();

        [BindProperty]
        public Album? Album { get; set; }

        [BindProperty]
        public List<TrackEditModel> Tracks { get; set; } = new();

        public List<Artist> Artists { get; set; } = new();

        // Load album and tracks
        public IActionResult OnGet(int id)
        {
            Album = db.Albums.SingleOrDefault(a => a.AlbumId == id);
            if (Album == null)
                return Redirect("~/Index");

            Artists = db.Artists.ToList();

            Tracks = db.Tracks
                        .Where(t => t.AlbumId == id)
                        .Select(t => new TrackEditModel
                        {
                            TrackId = t.TrackId,
                            Name = t.Name
                        })
                        .ToList();

            return Page();
        }

        // Save album and tracks
        public IActionResult OnPost()
        {
            if (Album == null)
                return BadRequest("Album data missing!");

            // Update album
            var albumFromDb = db.Albums.SingleOrDefault(a => a.AlbumId == Album.AlbumId);
            if (albumFromDb == null)
                return NotFound();

            albumFromDb.Title = Album.Title;
            albumFromDb.ArtistId = Album.ArtistId;

            // Update tracks
            foreach (var track in Tracks)
            {
                var dbTrack = db.Tracks.SingleOrDefault(t => t.TrackId == track.TrackId);
                if (dbTrack != null)
                    dbTrack.Name = track.Name;
            }

            db.SaveChanges();
            return Redirect("~/Index");
        }

        // Simple track model for binding
        public class TrackEditModel
        {
            public int TrackId { get; set; }
            public string Name { get; set; } = string.Empty;
        }
    }
}