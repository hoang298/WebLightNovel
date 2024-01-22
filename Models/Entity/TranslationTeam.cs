using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
namespace WebLightNovel.Models.Entity
{
    [Table("TranslationTeam")]
    public partial class TranslationTeam
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TranslationTeam()
        {
            JoinRequests = new HashSet<JoinRequest>();
            Stories = new HashSet<Story>();
            TranslationUsers = new HashSet<TranslationUser>();
            state = 1;
            avatar = "default.png";
            dateCreated = DateTime.Now;
        }

        [Key]
        public int translation_id { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }

        public int state { get; set; }

        [StringLength(255)]
        public string avatar { get; set; }

        [Required]
        [StringLength(128)]
        public string user_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime dateCreated { get; set; }

        [Required]
        [StringLength(4000)]
        public string description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JoinRequest> JoinRequests { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Story> Stories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TranslationUser> TranslationUsers { get; set; }
    }
}
