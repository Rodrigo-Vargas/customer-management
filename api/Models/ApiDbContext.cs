using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Models
{
   public class ApiDbContext : IdentityDbContext<User>
   {
      public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

      public DbSet<User> ApplicationUsers { get; set; }
      public DbSet<Gender> Genders { get; set; }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         base.OnModelCreating(modelBuilder);

         foreach (var entity in modelBuilder.Model.GetEntityTypes())
         {
            // Remove 'AspNet' prefix AspNetRoleClaims -> RoleClaims
            entity.SetTableName(entity.GetTableName().Replace("AspNet", ""));
         }
      }
   }
}