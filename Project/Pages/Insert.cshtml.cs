
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
  string albumTitle = Request.Form["tbxAlbumTitle"]! ;
string artistName = Request.Form["tbxArtist"]! ;
    var trackNames = Request.Form["tbxTrackNames"].ToArray(); // multiple tracks

    if (string.IsNullOrWhiteSpace(albumTitle))
    {
        ModelState.AddModelError("", "Album title cannot be empty");
        OnGet();
        return Page();
    }

    if (string.IsNullOrWhiteSpace(artistName))
    {
        ModelState.AddModelError("", "Please select or enter an artist");
        OnGet();
        return Page();
    }

    using (var db = new chinookDb())
    {
        // Check if artist exists
        var artist = db.Artists.FirstOrDefault(a => a.Name!.ToLower() == artistName.ToLower());
        if (artist == null)
        {
            artist = new Artist { Name = artistName };
            db.Artists.Add(artist);
            db.SaveChanges();
        }

        // Insert Album
        var album = new Album
        {
            Title = albumTitle,
            ArtistId = artist.ArtistId
        };
        db.Albums.Add(album);
        db.SaveChanges();

        // Insert multiple tracks
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
}}}