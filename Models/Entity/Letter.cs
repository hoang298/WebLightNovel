using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
namespace WebLightNovel.Models.Entity
{
    [Table("Letter")]
    public partial class Letter
    {
        [Key]
        public int letter_id { get; set; }

        [StringLength(500)]
        public string title { get; set; }

        [Required]
        public string content { get; set; }

       // [Required]
        [StringLength(128)]
        public string sender_id { get; set; }

       //[Required]
        [StringLength(128)]
        public string receiver_id { get; set; }

        public DateTime time { get; set; }

        public int state { get; set; }

        public bool isDeleteSender { get; set; }
        public bool isDeleteReceiver { get; set; }
    }
}
