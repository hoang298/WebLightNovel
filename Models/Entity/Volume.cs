using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Web.Mvc;

namespace WebLightNovel.Models.Entity
{
    [Table("Volume")]
    public partial class Volume
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Volume()
        {
            Chapters = new HashSet<Chapter>();
        }

        [Key]
        public int volume_id { get; set; }
        public int ordinalNumber { get; set; }

        [StringLength(255)]
        public string name { get; set; }

        public int story_id { get; set; }

        [StringLength(255)]
        public string volumeImg { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Chapter> Chapters { get; set; }

        public virtual Story Story { get; set; }
    }
}
