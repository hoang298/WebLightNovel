using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
namespace WebLightNovel.Models.Entity
{

    [Table("Chapter")]
    public partial class Chapter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Chapter()
        {
            ImageChapters = new HashSet<ImageChapter>();
        }

        [Key]
        public int chapter_id { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }

        public string content { get; set; }

        public int ordinalNumber { get; set; }

        public int volume_id { get; set; }

        public DateTime? update_date { get; set; }
        public int story_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ImageChapter> ImageChapters { get; set; }

        public virtual Volume Volume { get; set; }
    }
}
