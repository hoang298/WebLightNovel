using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
namespace WebLightNovel.Models.Entity
{


    [Table("HistoryStory")]
    public partial class HistoryStory
    {
        [Key]
        public int ordinalNumber { get; set; }

        [Required]
        [StringLength(128)]
        public string user_id { get; set; }

        public int story_id { get; set; }

        public int chapter_id { get; set; }

        public DateTime time { get; set; }

        public virtual Story Story { get; set; }
    }
}
