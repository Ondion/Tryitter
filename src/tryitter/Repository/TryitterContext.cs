using Microsoft.EntityFrameworkCore;
using tryitter.Models;

namespace tryitter.Repository;

public class TryitterContext : DbContext
{

    public DbSet<Post>? Posts { get; set; }
    public DbSet<Student>? Students { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = Environment.GetEnvironmentVariable("DOTNET_CONNECTION_STRING");

            // optionsBuilder.UseSqlServer(@"Server=127.0.0.1;Database=tryitter;User=SA;Password=Password12;");
            //   optionsBuilder.UseSqlServer(@"Server=127.0.0.1;Database=tryitter;User=SA;Password=T@a202101;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>()
        .HasKey(x => x.StudentId);

        modelBuilder.Entity<Post>()
        .HasKey(x => x.PostId);

        modelBuilder.Entity<Post>()
        .HasOne(c => c.Student)
        .WithMany(x => x.Posts)
        .HasForeignKey(d => d.StudentId);
    }
}