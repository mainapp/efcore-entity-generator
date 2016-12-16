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
    
    public class User 
    {
        
        public int DepartmentId { get; set; }
        
        public int Age { get; set; }
        [MaxLength(40)]
        public string Name { get; set; }
        
        public int Id { get; set; }
        
        public Department Department { get; set; }
        
    }
    
    public class Blog 
    {
        
        public string Context { get; set; }
        
        public int Id { get; set; }
        
    }
    
    public class Department 
    {
        [MaxLength(40)]
        public string Name { get; set; }
        
        public int Id { get; set; }
        
        public List<User> Users { get; set; }
        
    }
    
}