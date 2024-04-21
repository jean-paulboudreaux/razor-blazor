// Jean-Paul Boudreaux, Andrew Kieu 
// C00416940, C00014562
// CMPS 358 .NET/C# Programming
// project Chinook Razor/Blazor Website Project
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ChinookLibrary.Models;

[Table("playlists")]
public partial class Playlist
{
    [Key]
    public int PlaylistId { get; set; }

    [Column(TypeName = "NVARCHAR(120)")]
    public string? Name { get; set; }

    [ForeignKey("PlaylistId")]
    [InverseProperty("Playlists")]
    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}
