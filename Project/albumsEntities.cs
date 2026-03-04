using System;
namespace albumsEntities
{

    public class Album
    {
        public Int32 AlbumId { get; set; }
        public string? Title { get; set; }
        public int ArtistId { get; set; }
        public Artist Artist { get; set; } = null!;
        public List<Track> Tracks { get; set; } = new();

    }
    public class Artist
    {
        public Int32 ArtistId { get; set; }
        public string? Name { get; set; }
        public List<Album> Albums { get; set; } = new();


    }
    public class Track
    {
        public int TrackId { get; set; }
        public string? Name { get; set; }

        public int AlbumId { get; set; }
        public Album Album { get; set; } = null!;

        // Required by DB
        public int MediaTypeId { get; set; }
        public int Milliseconds { get; set; }
        public decimal UnitPrice { get; set; }

        // to avoid foreignkey constrain
        public int? GenreId { get; set; }
        public string? Composer { get; set; }
        public int? Bytes { get; set; }
    }

    public class TrackEditModel
    {
        public int TrackId { get; set; }
        public string Name { get; set; } = string.Empty;
    }


};
