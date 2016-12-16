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
                .HasIndex(index => new {  index.Name, index.Id  });
            

            
            builder.Entity<User>()
                .HasOne(p => p.Department)
                .WithMany()
                .HasForeignKey(p => p.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);
            
        }


        
        public DbSet<User> Users { get; set; }
        
        public DbSet<Blog> Blogs { get; set; }
        
        public DbSet<Department> Departments { get; set; }
        

    }
}