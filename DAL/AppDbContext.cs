using Indigo.Models;
using Microsoft.EntityFrameworkCore;

namespace Indigo.DAL
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt):base(opt)
        {

        }
        public DbSet<Post> Posts { get; set; }
    }
}
