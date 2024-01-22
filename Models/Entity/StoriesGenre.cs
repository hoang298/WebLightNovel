using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
namespace WebLightNovel.Models.Entity
{
    public partial class StoriesGenre
    {
        public int story_id { get; set; }

        public int genres_id { get; set; }

        [Key]
        public int ordinalNumber { get; set; }

        public virtual Genre Genre { get; set; }

        public virtual Story Story { get; set; }
    }
}
