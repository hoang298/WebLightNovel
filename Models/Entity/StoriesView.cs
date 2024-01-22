using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
namespace WebLightNovel.Models.Entity
{
    [Table("StoriesView")]
    public partial class StoriesView
    {
        public int story_id { get; set; }

        public int day_views { get; set; }

        public int month_views { get; set; }

        public int total_views { get; set; }

        public int comment_views { get; set; }

        public int follow_views { get; set; }

        public int total_word { get; set; }

        [Key]
        public int ordinalNumber { get; set; }

        public virtual Story Story { get; set; }
    }
}
