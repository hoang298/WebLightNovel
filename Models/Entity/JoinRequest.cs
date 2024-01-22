using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
namespace WebLightNovel.Models.Entity
{
    [Table("JoinRequest")]
    public partial class JoinRequest
    {
        [Key]
        public int req_id { get; set; }

        [Required]
        [StringLength(128)]
        public string sender_id { get; set; }

        public DateTime time_req { get; set; }

        public int state { get; set; }

        public int trans_id { get; set; }

        public virtual TranslationTeam TranslationTeam { get; set; }
    }
}
