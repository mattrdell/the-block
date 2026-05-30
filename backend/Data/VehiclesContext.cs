using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TheBlock.Api.Models;

namespace TheBlock.Api.Data;

public class VehiclesContext(DbContextOptions<VehiclesContext> options) : DbContext(options)
{
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var listComparer = new ValueComparer<List<string>>(
            (left, right) => ListsEqual(left, right),
            value => ListHash(value),
            value => Snapshot(value));

        modelBuilder.Entity<Vehicle>()
            .Property(v => v.DamageNotes)
            .HasConversion(
                notes => string.Join("||", notes),
                notes => string.IsNullOrWhiteSpace(notes)
                    ? new List<string>()
                    : notes.Split("||", StringSplitOptions.RemoveEmptyEntries).ToList())
            .Metadata.SetValueComparer(listComparer);

        modelBuilder.Entity<Vehicle>()
            .Property(v => v.Images)
            .HasConversion(
                images => string.Join("||", images),
                images => string.IsNullOrWhiteSpace(images)
                    ? new List<string>()
                    : images.Split("||", StringSplitOptions.RemoveEmptyEntries).ToList())
            .Metadata.SetValueComparer(listComparer);
    }

    private static bool ListsEqual(List<string>? left, List<string>? right)
    {
        return (left ?? new List<string>()).SequenceEqual(right ?? new List<string>());
    }

    private static int ListHash(List<string>? value)
    {
        if (value is null)
        {
            return 0;
        }

        var hash = new HashCode();
        foreach (var item in value)
        {
            hash.Add(item);
        }

        return hash.ToHashCode();
    }

    private static List<string> Snapshot(List<string>? value)
    {
        return value?.ToList() ?? new List<string>();
    }
}
