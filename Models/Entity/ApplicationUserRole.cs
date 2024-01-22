using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
namespace WebLightNovel.Models.Entity
{


    public partial class ApplicationUserRole
    {
        [Key]
        [Column(Order = 0)]
        public string UserId { get; set; }

        [Key]
        [Column(Order = 1)]
        public string RoleId { get; set; }

        [StringLength(128)]
        public string IdentityRole_Id { get; set; }

        [StringLength(128)]
        public string ApplicationUser_Id { get; set; }

        public virtual ApplicationRole ApplicationRole { get; set; }
    }
}
