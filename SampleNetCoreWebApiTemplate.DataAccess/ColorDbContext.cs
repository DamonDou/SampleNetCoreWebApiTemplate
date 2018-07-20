using Microsoft.EntityFrameworkCore;
using SampleNetCoreWebApiTemplate.Model.ViewModel;

namespace SampleNetCoreWebApiTemplate.DataAccess
{
    public class ColorDbContext:DbContext
    {

        public ColorDbContext(DbContextOptions<ColorDbContext> options):base(options)
        {

        }

        public DbSet<UserViewModel> UserViewModels{get;set;}
    }
}