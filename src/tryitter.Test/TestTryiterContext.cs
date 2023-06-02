using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using tryitter.Models;
using tryitter.Repository;

namespace tryitter.Test;

public class TestTryitterContext<TEntryPoint> : WebApplicationFactory<Program> where TEntryPoint : Program
{

  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    builder.ConfigureServices(services =>
    {
      var descriptor = services.SingleOrDefault(
              d => d.ServiceType ==
                  typeof(DbContextOptions<TryitterContext>));
      if (descriptor != null)
        services.Remove(descriptor);
      services.AddDbContext<TryitterContext>(options =>
          {
            options.UseInMemoryDatabase("db");
          });
      var sp = services.BuildServiceProvider();
      using (var scope = sp.CreateScope())
      using (var appContext = scope.ServiceProvider.GetRequiredService<TryitterContext>())
      {
        try
        {
          appContext.Database.EnsureDeleted();

          appContext.Database.EnsureCreated();
          appContext.Students.AddRange(
            new Student { Name = "Ana", Email = "ana@gmail.com", Password = "xft@ff", Status = "Focada" },
            new Student { Name = "Paulo", Email = "paulo@gmail.com", Password = "xft@ff", Status = "Focada" },
            new Student { Name = "Tom", Email = "tom@gmail.com", Password = "xft@ff", Status = "Focada" },
            new Student { Name = "Joao", Email = "joao@gmail.com", Password = "xft@ff", Status = "Focada" }
            );
          appContext.SaveChanges();
        }
        catch (Exception e)
        {
          throw e;
        }
      }
    });
  }
}