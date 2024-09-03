using Mango.Services.CouponAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext>options) : base (options)
        {
            
        }
        //se crea un dbset para la entidad coupon, un dbset es una coleccion de objetos de la entidad coupon
        public DbSet<Coupon> Coupon { get; set; }
        //se sobreescribe el metodo OnModelCreating para agregar data a la tabla coupon
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //incluir data con entity
            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                CouponId = 1,
                CouponCode = "10OFF",
                DiscountAmount = 10,
                MinAmount = 20
            });


            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                CouponId = 2,
                CouponCode = "11OFF",
                DiscountAmount = 20,
                MinAmount = 40
            });
        }

    }
}
