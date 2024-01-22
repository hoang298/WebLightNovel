using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
namespace WebLightNovel.Models.Entity
{

    public partial class ApplicationUserClaim
    {
        [Key]
        public int Id { get; set; }
        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }

        [StringLength(128)]
        public string ApplicationUser_Id { get; set; }
    }
}
