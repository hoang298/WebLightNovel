using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
namespace WebLightNovel.Models.Entity
{
    [Table("Notify")]
    public partial class Notify
    {
        [Key]
        public int notify_id { get; set; }

        [Required]
        [StringLength(255)]
        public string content { get; set; }

        [Required]
        [StringLength(128)]
        public string user_id { get; set; }

        public DateTime time { get; set; }

        [Required]
        [StringLength(255)]
        public string url_notify { get; set; }

        public int state { get; set; }

        [Required]
        [StringLength(128)]
        public string sender_id { get; set; }

        public string senderAvatar { get; set; }
    }
}
