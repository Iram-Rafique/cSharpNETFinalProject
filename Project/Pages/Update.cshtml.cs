
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
        [BindProperty]
        public string ArtistName { get; set; } = string.Empty;
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
                            Name = t.Name!
                        })
                        .ToList();

            return Page();
        }

        // Save album and tracks
        public IActionResult OnPost()
        {
            if (Album == null)
                return BadRequest("Album data missing!");

            var albumFromDb = db.Albums
                                .SingleOrDefault(a => a.AlbumId == Album.AlbumId);

            if (albumFromDb == null)
                return NotFound();

            // ===== FIXED ARTIST LOGIC =====

            if (!string.IsNullOrWhiteSpace(ArtistName))
            {
                // Check if artist already exists
                var existingArtist = db.Artists
                                       .SingleOrDefault(a => a.Name == ArtistName);

                if (existingArtist == null)
                {
                    // Create new artist
                    existingArtist = new Artist
                    {
                        Name = ArtistName
                    };

                    db.Artists.Add(existingArtist);
                    db.SaveChanges(); // Save to generate ArtistId
                }

                albumFromDb.ArtistId = existingArtist.ArtistId;
            }

            // ===== UPDATE ALBUM TITLE =====
            albumFromDb.Title = Album.Title;

            // ===== UPDATE TRACKS =====
            foreach (var track in Tracks)
            {
                var dbTrack = db.Tracks
                .SingleOrDefault(t => t.TrackId == track.TrackId);

                if (dbTrack != null)
                    dbTrack.Name = track.Name;
            }

            db.SaveChanges();
            return Redirect("~/Index");
        }
    }
}