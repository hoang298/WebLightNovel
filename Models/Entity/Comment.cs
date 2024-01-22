using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
namespace WebLightNovel.Models.Entity
{
    [Table("Comment")]
    public partial class Comment
    {
        [Key]
        public int comment_id { get; set; }

        [StringLength(4000)]
        public string content { get; set; }

        [Required]
        [StringLength(128)]
        public string user_id { get; set; }

        public DateTime time { get; set; }

        public int? story_id { get; set; }

        public int? chapter_id { get; set; }

        public int? id_parent { get; set; }

        [Required]
        [StringLength(50)]
        public string user_name { get; set; }

        public virtual Story Story { get; set; }
    }
}
