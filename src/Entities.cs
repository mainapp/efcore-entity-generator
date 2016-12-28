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
        
        public int SortOrder { get; set; }
        
        public List<User> Users { get; set; }
        
        public List<TargetGroup> TargetGroups { get; set; }
        
    }
    
    public class ApplicationUser : IdentityUser 
    {
        [MaxLength(40)]
        public string RealName { get; set; }
        
        public bool IsRoot { get; set; }
        
        public bool IsManager { get; set; }
        
        public int DepartmentId { get; set; }
        
        public Department Department { get; set; }
        
    }
    
    public class Indicator 
    {
        
        public int Id { get; set; }
        [MaxLength(40)]
        public string Name { get; set; }
        
        public bool IsManualInput { get; set; }
        [MaxLength(80)]
        public string ExternalCode { get; set; }
        [MaxLength(20)]
        public string ExternalSystemName { get; set; }
        
    }
    
    public class IndicatorsGroup 
    {
        
        public int Id { get; set; }
        [MaxLength(40)]
        public string Name { get; set; }
        
        public int SortOrder { get; set; }
        
        public datetime BeginDate { get; set; }
        
        public datetime EndDate { get; set; }
        
        public List<TargetGroup> Indicators { get; set; }
        
        public int MainUserGroupId { get; set; }
        
        public IndicatorsRelatedUserGroup MainUsers { get; set; }
        
        public int RelatedUserGroupId { get; set; }
        
        public IndicatorsRelatedUserGroup RelatedUsers { get; set; }
        
    }
    
    public class IndicatorsRelatedUserGroup 
    {
        
        public int Id { get; set; }
        [MaxLength(20)]
        public string Name { get; set; }
        
        public List<GroupUser> GroupUsers { get; set; }
        
    }
    
    public class GroupUser 
    {
        
        public int Id { get; set; }
        
        public int SortOrder { get; set; }
        
        public string UserId { get; set; }
        
        public ApplicationUser User { get; set; }
        
        public int GroupId { get; set; }
        
        public IndicatorsRelatedUserGroup Group { get; set; }
        
    }
    
    public class TargetGroup 
    {
        
        public int Id { get; set; }
        
        public int SortOrder { get; set; }
        [MaxLength(40)]
        public string TargetValue { get; set; }
        [MaxLength(40)]
        public string CurrentValue { get; set; }
        [MaxLength(40)]
        public string AlertValue { get; set; }
        
        public bool AlertEnabled { get; set; }
        
        public datetime BeginDate { get; set; }
        
        public datetime EndDate { get; set; }
        
        public int IndicatorId { get; set; }
        
        public Indicator Indicator { get; set; }
        
        public int DepartmentId { get; set; }
        
        public Department Department { get; set; }
        
    }
    
}