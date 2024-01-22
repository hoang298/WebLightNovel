using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
namespace WebLightNovel.Models.Entity
{
    [Table("StoryFollow")]
    public partial class StoryFollow
    {
        [Key]
        public int ordinalNumber { get; set; }

        [Required]
        [StringLength(128)]
        public string user_id { get; set; }

        public int story_id { get; set; }

        public int state { get; set; }

        public int total_unread { get; set; }

        public virtual Story Story { get; set; }
    }
}
