
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
        public List<string> NewTrackNames { get; set; } = new(); // TO ADD A NEW TRACK

        [BindProperty]
        public Album? Album { get; set; }

        [BindProperty]
        public List<TrackEditModel> Tracks { get; set; } = new();
        [BindProperty]
        public string ArtistName { get; set; } = string.Empty;
        public List<Artist> Artists { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            Album = db.Albums.SingleOrDefault(a => a.AlbumId == id);
            if (Album == null)
                return Redirect("~/Index");

            Artists = db.Artists.ToList();

            // show Tracks for editing
            Tracks = db.Tracks
                        .Where(t => t.AlbumId == id)
                        .Select(t => new TrackEditModel
                        {
                            TrackId = t.TrackId,
                            Name = t.Name!
                        })
                        .ToList();

            // show ArtistName 
            if (Album.ArtistId != 0)
            {
                var artist = db.Artists.SingleOrDefault(a => a.ArtistId == Album.ArtistId);
                if (artist != null)
                    ArtistName = artist.Name!;
            }

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

            //artist logic

            if (!string.IsNullOrWhiteSpace(ArtistName))
            {
                // Check if artist already exists
                var existingArtist = db.Artists
                .SingleOrDefault(a => a.Name == ArtistName);

                if (existingArtist == null)
                {
                    // create new artist
                    existingArtist = new Artist
                    {
                        Name = ArtistName
                    };

                    db.Artists.Add(existingArtist);
                    db.SaveChanges(); // Save to generate artistId
                }

                albumFromDb.ArtistId = existingArtist.ArtistId;
            }

            //    updatw title
            albumFromDb.Title = Album.Title;

            // update track
            foreach (var track in Tracks)
            {
                var dbTrack = db.Tracks
                .SingleOrDefault(t => t.TrackId == track.TrackId);

                if (dbTrack != null)
                    dbTrack.Name = track.Name;
            }




            //    save album first
            db.SaveChanges(); // ensures Album has valid ArtistId in DB

            //  ADD NEW TRACKS 
            if (NewTrackNames != null && NewTrackNames.Any())
            {
                foreach (var trackName in NewTrackNames)
                {
                    if (!string.IsNullOrWhiteSpace(trackName))
                    {
                        db.Tracks.Add(new Track
                        {
                            Name = trackName,
                            AlbumId = albumFromDb.AlbumId,
                            MediaTypeId = 1,      // default values
                            Milliseconds = 1000,  // default values
                            UnitPrice = 0.99m// default values
                        });
                    }
                }
            }

            // AVE NEW TRACKS
            db.SaveChanges();
            return Redirect("~/Index");
        }
    }
}