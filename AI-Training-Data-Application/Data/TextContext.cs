using AI_Training_Data_Application_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace AI_Training_Data_Application_Backend.Data;

public class TextContext(DbContextOptions<TextContext> options) : DbContext(options)
{
    public DbSet<Text> Texts { get; init; }
    public DbSet<Category> Categories { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Text>()
            .HasIndex(t => t.TextString)
            .IsUnique();

        modelBuilder.Entity<Category>()
            .HasIndex(c => c.Name)
            .IsUnique();
    }
}