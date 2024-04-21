// Jean-Paul Boudreaux, Andrew Kieu 
// C00416940, C00014562
// CMPS 358 .NET/C# Programming
// project Chinook Razor/Blazor Website Project
using System;
using System.Collections.Generic;
using ChinookLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace ChinookLibrary.Models;

public partial class ChinookContext : DbContext
{
    private string path;
    
    public ChinookContext()
    {
        SetPath();
    }

    public ChinookContext(DbContextOptions<ChinookContext> options)
        : base(options)
    {
        SetPath();
    }

    private void SetPath()
    {
        path = "../../../../../ChinookLibrary/ChinookLibrary/chinook.db";
        if (!File.Exists(path))
            path = "../../ChinookLibrary/ChinookLibrary/chinook.db";
    }

    public virtual DbSet<Album> Albums { get; set; }

    public virtual DbSet<Artist> Artists { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<InvoiceItem> InvoiceItems { get; set; }

    public virtual DbSet<MediaType> MediaTypes { get; set; }

    public virtual DbSet<Playlist> Playlists { get; set; }

    public virtual DbSet<Track> Tracks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
    {
        if (!optionsBuilder.IsConfigured)
        {
            if (!File.Exists((path)))
                throw new FileNotFoundException($"{path} not found");
    
            optionsBuilder.UseSqlite($"Data Source={path}");

            optionsBuilder.LogTo(
                ChinookContextLogger.WriteToLog, new[]{Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting});
        }
    }
    
/*    Prescribed but not working solution
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
    {
        if (!optionsBuilder.IsConfigured)
        {
            string database = "chinook.db";
            string directory = Environment.CurrentDirectory;
            string path = database;

            if (directory.EndsWith("net8.0"))
            {
                path = Path.Combine(@"../../../", database);
            }

            path = Path.GetFullPath(path);
            ChinookContextLogger.WriteToLog(
                $"Database Path: {path}");

            if (!File.Exists((path)))
                throw new FileNotFoundException($"{path} not found");
        
            optionsBuilder.UseSqlite($"Data Source={path}");

            optionsBuilder.LogTo(
                ChinookContextLogger.WriteToLog, new[]{Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting});
        }
    }
*/    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasOne(d => d.Artist).WithMany(p => p.Albums).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasOne(d => d.Customer).WithMany(p => p.Invoices).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<InvoiceItem>(entity =>
        {
            entity.HasOne(d => d.Invoice).WithMany(p => p.InvoiceItems).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Track).WithMany(p => p.InvoiceItems).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.HasMany(d => d.Tracks).WithMany(p => p.Playlists)
                .UsingEntity<Dictionary<string, object>>(
                    "PlaylistTrack",
                    r => r.HasOne<Track>().WithMany()
                        .HasForeignKey("TrackId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    l => l.HasOne<Playlist>().WithMany()
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    j =>
                    {
                        j.HasKey("PlaylistId", "TrackId");
                        j.ToTable("playlist_track");
                        j.HasIndex(new[] { "TrackId" }, "IFK_PlaylistTrackTrackId");
                    });
        });

        modelBuilder.Entity<Track>(entity =>
        {
            entity.HasOne(d => d.MediaType).WithMany(p => p.Tracks).OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
