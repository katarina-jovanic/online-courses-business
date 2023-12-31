﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using IndexAttribute = Microsoft.EntityFrameworkCore.IndexAttribute;
using IdentityUser = Microsoft.AspNetCore.Identity.IdentityUser;

namespace Zavrsni.Models
{
    [Index("Email", Name = "UQ__Users__A9D10534EC4F041F", IsUnique = true)]
    public partial class User 
    {
        [Key]
        public int UserID { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string Surname { get; set; }
        [Required]
        [StringLength(50)]
        public string Email { get; set; }
        [Required]
        [StringLength(50)]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }

        [InverseProperty("AdminNavigation")]
        public virtual Admin Admin { get; set; }
        [InverseProperty("StudentNavigation")]
        public virtual Student Student { get; set; }
        [InverseProperty("TeacherNavigation")]
        public virtual Teacher Teacher { get; set; }
    }
}