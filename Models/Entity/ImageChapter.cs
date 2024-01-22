using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
namespace WebLightNovel.Models.Entity
{
    [Table("ImageChapter")]
    public partial class ImageChapter
    {
        [Key]
        public int img_id { get; set; }

        [Required]
        [StringLength(255)]
        public string img_name { get; set; }

        public int chapter_id { get; set; }

        public int rowNumber { get; set; }

        [StringLength(128)]
        public string pHashImage { get; set; }

        public virtual Chapter Chapter { get; set; }
    }
}
