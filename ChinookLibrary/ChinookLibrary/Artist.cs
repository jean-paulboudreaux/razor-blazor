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

[Table("artists")]
public partial class Artist
{
    [Key]
    public int ArtistId { get; set; }

    [Column(TypeName = "NVARCHAR(120)")]
    public string? Name { get; set; }

    [InverseProperty("Artist")]
    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();
}
