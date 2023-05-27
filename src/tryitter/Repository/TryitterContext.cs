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

            //alterar
            // optionsBuilder.UseSqlServer(@"Server=127.0.0.1;Database=master;User=SA;Password=Password12!;");
        }
    }
}