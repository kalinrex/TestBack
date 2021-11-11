using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public  class ActivityDbContext : IdentityDbContext<User>
    {
        public ActivityDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Activity>()
          .HasOne<Property>(p => p.Property)
          .WithMany(a => a.Activities)
          .HasForeignKey(pi => pi.property_id);
            builder.Entity<Activity>()
            .HasOne<Survey>(s => s.Survey)
            .WithOne(ac => ac.Activity)
            .HasForeignKey<Survey>(ac => ac.activity_id);
        }

        public DbSet<Property> Property { get; set; }
        public DbSet<Activity> Activity { get; set; }
        public DbSet<Survey> Survey { get; set; }
    }
}
