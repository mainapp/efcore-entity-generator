using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace YourNamespace
{
    
    public class Department 
    {
        
        public int Id { get; set; }
        [MaxLength(40)]
        public string Name { get; set; }
        
        public List<User> Users { get; set; }
        
        public List<Blog> Blogs { get; set; }
        
    }
    
    public class User 
    {
        
        public int Id { get; set; }
        [MaxLength(40)]
        public string Name { get; set; }
        
        public int Age { get; set; }
        
        public int DepartmentId { get; set; }
        
        public Department Department { get; set; }
        
        public List<Blog> Blogs { get; set; }
        
    }
    
    public class Blog 
    {
        
        public int Id { get; set; }
        
        public string Context { get; set; }
        
        public string SenderId { get; set; }
        
        public User Sender { get; set; }
        
        public int DepartmentId { get; set; }
        
        public Department Department { get; set; }
        
    }
    
}