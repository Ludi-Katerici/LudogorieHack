using System;
using System.Threading.Tasks;

namespace EducateMe.Data.Seeding;

public interface ISeeder
{
    Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider);
}
