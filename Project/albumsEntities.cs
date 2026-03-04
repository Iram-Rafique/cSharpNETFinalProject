using System;
namespace albumsEntities
{
    // album
    public class Album
    {
        public Int32 AlbumId { get; set; }
        public string? Title { get; set; }
        public int ArtistId { get; set; }
        // artist associated with this entity
        public Artist Artist { get; set; } = null!;
        // list of tracks related to this entity
        public List<Track> Tracks { get; set; } = new();

    }
    //artist
    public class Artist
    {
        public Int32 ArtistId { get; set; }
        public string? Name { get; set; }
        public List<Album> Albums { get; set; } = new();


    }

    //track 
    public class Track
    {
        public int TrackId { get; set; }
        public string? Name { get; set; }

        public int AlbumId { get; set; }
        public Album Album { get; set; } = null!;

        // required by DB
        public int MediaTypeId { get; set; }
        public int Milliseconds { get; set; }
        public decimal UnitPrice { get; set; }

        // to avoid foreignkey constraint
        public int? GenreId { get; set; }
        public string? Composer { get; set; }
        public int? Bytes { get; set; }
    }
    //this is to update track
    public class TrackEditModel
    {
        public int TrackId { get; set; }
        public string Name { get; set; } = string.Empty;
    }


};
