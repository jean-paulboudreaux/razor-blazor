// Jean-Paul Boudreaux, Andrew Kieu 
// C00416940, C00014562
// CMPS 358 .NET/C# Programming
// project Chinook Razor/Blazor Website Project
using ChinookLibrary.Models;

namespace ChinookLibrary;

public static class DbUtility
{

    // i
    public static List<String> ListGenres()
    {
        using var db = new ChinookContext();
        return db.Genres.OrderBy(genre => genre.Name).Select(genre => genre.Name).ToList();
    }
    
    // ii
    public static List<string> ListArtists()
    {
        using var db = new ChinookContext();
        return db.Artists.OrderBy(artist => artist.Name).Select(artist => artist.Name).ToList();
    }
     
     
    // iii 
    public static List<string> ListTracksByGenre(string genre)
    {
        using var db = new ChinookContext();
        return (from t in db.Tracks
            from al in db.Albums
            from ar in db.Artists
            where genre.Equals(t.Genre.Name) && t.AlbumId == al.AlbumId && al.ArtistId == ar.ArtistId
            select $"{t.Name}, {ar.Name}, {t.UnitPrice}").ToList();
    }
    public static List<string> GetListAllGenres()
    {
        using var db = new ChinookContext();
        var results =
            from genre in db.Genres select genre.Name;
        //select Genre.Name;
        return results.ToList(); 
    }

    
    // iv
    public static List<string> ListTracksByArtist(string artist)
    {
        using var db = new ChinookContext();
        return (from ar in db.Artists
            from al in db.Albums
            from tr in db.Tracks
            where ar.Name == artist && al.ArtistId == ar.ArtistId && tr.AlbumId == al.AlbumId
            select $"{tr.Name}, {tr.UnitPrice}").ToList();
    }

    
    // v
    public static SortedDictionary<string, int> Composers()
    {
        using var db = new ChinookContext();
        SortedDictionary<string, int> composers = new();

        var results =
            from track in db.Tracks
            select track.Composer;
        
        foreach(var composer in results)
            if (composer != null)
                if (composers.ContainsKey(composer))
                    composers[composer] += 1;
                else
                    composers[composer] = 1;

        return composers;
    }
}


