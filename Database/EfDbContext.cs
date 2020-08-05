using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;

using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using test_mvc_app.Models;

namespace test_mvc_app.Database
{
    public class EfDbContext
    :IdentityDbContext<ApplicationUser>
    {
      public EfDbContext(DbContextOptions<EfDbContext> options)
      :base(options){

      }

      DbSet<Employee> Employee {get;set;}

      protected override void OnModelCreating(ModelBuilder modelBuilder){
        base.OnModelCreating(modelBuilder);

      }
       
    }
}