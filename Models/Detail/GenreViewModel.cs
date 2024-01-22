using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebLightNovel.Models.Detail
{
    public class GenreViewModel
    {
        public int genres_id { get; set; }

        [Required]
        [StringLength(50)]
        public string name { get; set; }
        public string link_genre { get; set; }
    }
}