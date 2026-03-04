
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using albumsEntities;
using albumsContext;
using System.Collections.Generic;
using System.Linq;

namespace Project.Pages
{
    public class InsertModel : PageModel
    {
        public List<Artist> Artists { get; set; } = new List<Artist>();

        public void OnGet()
        {
            using (var db = new chinookDb())
            {
                Artists = db.Artists.ToList(); // Load all artists for dropdown
            }
        }

        public IActionResult OnPost()
        {
            string albumTitle = Request.Form["tbxAlbumTitle"]!;
            string artistName = Request.Form["tbxArtist"]!;
            var trackNames = Request.Form["tbxTrackNames"]
                                .Where(t => !string.IsNullOrWhiteSpace(t))
                                .Select(t => t!.Trim())
                                .ToArray();
            // var trackNames = Request.Form["tbxTrackNames"].ToArray(); // multiple tracks

         
            bool hasError = false;
            // --- album validation length and empty ---
            if (string.IsNullOrWhiteSpace(albumTitle))
            {
                ModelState.AddModelError("tbxAlbumTitle", "Album title cannot be empty");
                hasError = true;
            }

            if (albumTitle.Length > 100)
            {
                ModelState.AddModelError("tbxAlbumTitle", "Album title is too long (max 100 characters)");
                hasError = true;
            }

            if (trackNames.Any(t => t.Length > 100))
            {
                ModelState.AddModelError("", "One or more track names are too long (max 100 characters)");
                hasError = true;
            }

            // --- Artist validation ---
            if (string.IsNullOrWhiteSpace(artistName))
            {
                ModelState.AddModelError("tbxArtist", "Please select or enter an artist");
                hasError = true;
            }

            using (var db = new chinookDb())
            {
                // check if artist exists
                var artist = db.Artists.FirstOrDefault(a => a.Name!.ToLower() == artistName.ToLower());
                if (artist == null)
                {
                    artist = new Artist { Name = artistName };
                    db.Artists.Add(artist);
                    db.SaveChanges();
                }
                // check for duplicate album for the same artist
                var existingAlbum = db.Albums
                    .FirstOrDefault(a => a.Title!.ToLower() == albumTitle.ToLower() &&
                                         a.Artist!.Name!.ToLower() == artistName.ToLower());
                if (existingAlbum != null)
                {
                    ModelState.AddModelError("tbxAlbumTitle", "This album already exists for the selected artist");
                    hasError = true;
                }
                // stop if errors exist
                if (hasError)
                {
                    Artists = db.Artists.ToList();
                    return Page();
                }
                // insert Album
                var album = new Album
                {
                    Title = albumTitle,
                    ArtistId = artist.ArtistId
                };
                db.Albums.Add(album);
                db.SaveChanges();

                // insert multiple tracks
                foreach (var trackName in trackNames)
                {
                    if (!string.IsNullOrWhiteSpace(trackName))
                    {
                        var track = new Track
                        {
                            Name = trackName,
                            AlbumId = album.AlbumId,
                            MediaTypeId = 1,
                            Milliseconds = 1000,
                            UnitPrice = 0.99m
                        };
                        db.Tracks.Add(track);
                    }
                }
                db.SaveChanges();
            }

            return RedirectToPage("/Index");
        }
    }
}