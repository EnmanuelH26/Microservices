using Fruit.Services.AuthAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fruit.Services.AuthAPI.Data
{
    public class AppDbContext : IdentityDbContext<AplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext>options) : base (options)
        {
            
        }
        public DbSet<AplicationUser> AplicationUser { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }

    }
}
