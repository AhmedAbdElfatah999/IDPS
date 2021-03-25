using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class IDPSContext : DbContext
    {
        public IDPSContext(DbContextOptions<IDPSContext> options) : base(options)
        {
            
        }

        public DbSet<Disease> Diseases {get; set;}
        public DbSet<Specialization> Specializations{get;set;}
    }
}