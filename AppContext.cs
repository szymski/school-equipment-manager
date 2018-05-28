using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolEquipmentManager.Models;

namespace SchoolEquipmentManager
{
    public class AppContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<ItemTemplate> ItemTemplates { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Message> Messages { get; set; }

        public AppContext(DbContextOptions<AppContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Item>().HasOne(i => i.Location);
            modelBuilder.Entity<Item>().HasOne(i => i.Template);
            modelBuilder.Entity<BorrowEvent>().HasOne(e => e.Teacher);
            modelBuilder.Entity<Item>().HasMany(i => i.Events).WithOne(e => e.Item).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Teacher>().HasMany(t => t.Messages).WithOne(m => m.Recipent)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

