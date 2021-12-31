using Microsoft.EntityFrameworkCore;

namespace VenDevApplication.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
        { }
        public virtual  DbSet<Product> products { get; set; }
    }
}
