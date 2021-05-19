using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContext : IdentityDbContext<Person>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {
        }
     public DbSet<Admin> Admins {get;set;}
     public DbSet<Doctor> Doctors {get;set;}
     public DbSet<Patient> Patients {get;set;}
     public DbSet<Specialization> Specializations {get;set;}

        public DbSet<Value> Values { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Message> Messages { get; set; }
    // public DbSet<IdentityRole> dentityRoles {get;set;}
        protected override void OnModelCreating(ModelBuilder builder) 
        {
            base.OnModelCreating(builder);

            builder.Entity<Message>()
                .HasOne(u => u.Doctor)
                .WithMany(m => m.MyMessage)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(u => u.Patient)
                .WithMany(m => m.MyMessage)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}