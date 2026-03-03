// using System;
// using System.Collections.Generic;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.RazorPages;
// using albumsEntities;
// using albumsContext;

// namespace Project.Pages
// {
//     // File name must match: Insert
//     public class Insert : PageModel
//     {
//         public string? Heading { get; set; }

//         public List<Album> Albums { get; set; } = new List<Album>();

//         public void OnGet()
//         {
//             // No code needed for GET
//         }

//         public IActionResult OnPost()
//         {
//             // Create new Album object using form data
//             Album Insert = new Album()
//             {
//                 Title = Request.Form["tbxAlbumTitle"]
//             };

//             // Insert into database
//             chinookDb db = new chinookDb(); // adjust if needed for constructor
//             db.Albums.Add(Insert);
//             db.SaveChanges();

//             return RedirectToPage("Index"); // go back to index page after insert
//         }
//     }
// }
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.RazorPages;
// using albumsEntities;
// using System.Linq;
// using albumsContext;

// namespace Project.Pages
// {
//     public class Insert : PageModel
//     {
//         public void OnGet()
//         {
//             // nothing needed for GET
//         }

//         public IActionResult OnPost()
//         {
//             string albumTitle = Request.Form["tbxAlbumTitle"];

//             if (string.IsNullOrWhiteSpace(albumTitle))
//             {
//                 ModelState.AddModelError("", "Album title cannot be empty");
//                 return Page();
//             }

//             using (var db = new chinookDb())
//             {
//                 // Ensure an artist exists
//                 Artist artist = db.Artists.FirstOrDefault();
//                 if (artist == null)
//                 {
//                     artist = new Artist { Name = "Unknown Artist" };
//                     db.Artists.Add(artist);
//                     db.SaveChanges(); // save artist first
//                 }

//                 // Insert album with valid ArtistId
//                 Album album = new Album
//                 {
//                     Title = albumTitle,
//                     ArtistId = artist.ArtistId
//                 };

//                 db.Albums.Add(album);
//                 db.SaveChanges();
//             }

//             return RedirectToPage("/Index"); // go back to Index after insert
//         }
//     }
// }
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.RazorPages;
// using albumsEntities;
// using System.Linq;
// using Microsoft.EntityFrameworkCore;
// using System.Collections.Generic;
// using albumsContext;

// namespace Project.Pages
// {
//     public class Insert : PageModel
//     {
//         public List<Artist> Artists { get; set; } = new List<Artist>();

//         public void OnGet()
//         {
           
//             using (var db = new chinookDb())
//             {
//                 Artists = db.Artists.ToList(); // Load all artists for dropdown
//             }
//         }

//         public IActionResult OnPost()
//         {
//             string albumTitle = Request.Form["tbxAlbumTitle"];
//             string selectedArtistId = Request.Form["ArtistId"];
//             string newArtistName = Request.Form["tbxNewArtist"];

//             if (string.IsNullOrWhiteSpace(albumTitle))
//             {
//                 ModelState.AddModelError("", "Album title cannot be empty");
//                 OnGet(); // reload artists for dropdown
//                 return Page();
//             }

           

//             using (var db = new chinookDb())
//             {
//                 int artistId;

//                 if (!string.IsNullOrWhiteSpace(newArtistName))
//                 {
//                     // Create new artist
//                     var artist = new Artist { Name = newArtistName };
//                     db.Artists.Add(artist);
//                     db.SaveChanges();
//                     artistId = artist.ArtistId;
//                 }
//                 else if (!string.IsNullOrWhiteSpace(selectedArtistId))
//                 {
//                     artistId = int.Parse(selectedArtistId);
//                 }
//                 else
//                 {
//                     ModelState.AddModelError("", "Please select or enter an artist");
//                     OnGet(); // reload artists for dropdown
//                     return Page();
//                 }

//                 // Insert album
//                 var album = new Album
//                 {
//                     Title = albumTitle,
//                     ArtistId = artistId
//                 };

//                 db.Albums.Add(album);
//                 db.SaveChanges();
//             }

//             return RedirectToPage("/Index");
//         }
//     }
// }
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
  string albumTitle = Request.Form["tbxAlbumTitle"] ;
string artistName = Request.Form["tbxArtist"] ;
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
        var artist = db.Artists.FirstOrDefault(a => a.Name.ToLower() == artistName.ToLower());
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