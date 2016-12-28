using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.DotNet.ProjectModel.Resolution;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace YourNamespace
{
    public class YourDbContext
    {
        public YourDbContext(DbContextOptions options)
                    : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<Department>()
                .HasIndex(index => new { index.Name  });
            
            builder.Entity<IndicatorsRelatedUserGroup>()
                .HasIndex(index => new { index.Name  });
            

            
            builder.Entity<ApplicationUser>()
                .HasOne(p => p.Department)
                .WithMany(b => b.Users)
                .HasForeignKey(p => p.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<IndicatorsGroup>()
                .HasOne(p => p.MainUsers)
                .WithMany()
                .HasForeignKey(p => p.MainUserGroupId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<IndicatorsGroup>()
                .HasOne(p => p.RelatedUsers)
                .WithMany()
                .HasForeignKey(p => p.RelatedUserGroupId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<GroupUser>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<GroupUser>()
                .HasOne(p => p.Group)
                .WithMany(b => b.GroupUsers)
                .HasForeignKey(p => p.GroupId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<TargetGroup>()
                .HasOne(p => p.Indicator)
                .WithMany()
                .HasForeignKey(p => p.IndicatorId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<TargetGroup>()
                .HasOne(p => p.Department)
                .WithMany(b => b.TargetGroups)
                .HasForeignKey(p => p.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);
            
        }


        
        public DbSet<Department> Departments { get; set; }
        
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        
        public DbSet<Indicator> Indicators { get; set; }
        
        public DbSet<IndicatorsGroup> IndicatorsGroups { get; set; }
        
        public DbSet<IndicatorsRelatedUserGroup> IndicatorsRelatedUserGroups { get; set; }
        
        public DbSet<GroupUser> GroupUsers { get; set; }
        
        public DbSet<TargetGroup> TargetGroups { get; set; }
        

    }
}