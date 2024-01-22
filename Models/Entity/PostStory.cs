using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
namespace WebLightNovel.Models.Entity
{
    public partial class PostStory
    {
        [Key]
        public int ordinalNumber { get; set; }
        [StringLength(128)]
        public string user_id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên truyện")]
        [StringLength(255)]
        public string title { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên tác giả")]
        [StringLength(255)]
        public string author_name { get; set; }

        [StringLength(255)]
        public string artist_name { get; set; }

        [StringLength(255)]
        public string image { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giới thiệu nội dung truyện")]
        public string synopsis { get; set; }

        public int status_story { get; set; }

        [Required]
        [StringLength(255)]
        public string genres { get; set; }

        public DateTime? time { get; set; }

        public int? type_story { get; set; }

        [StringLength(255)]
        public string translationSource { get; set; }

        public int trans_id { get; set; }

        public PostStory()
        {
            time = DateTime.Now;
            image = "default.png";
        }
    }
}
