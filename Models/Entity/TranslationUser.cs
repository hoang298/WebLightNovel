using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
namespace WebLightNovel.Models.Entity
{
    [Table("TranslationUser")]
    public partial class TranslationUser
    {
        [Key]
        public int ordinalNumber { get; set; }

        public int translation_id { get; set; }

        public string user_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime dateJoin { get; set; }

        public int role { get; set; }

        public virtual TranslationTeam TranslationTeam { get; set; }
    }
}
