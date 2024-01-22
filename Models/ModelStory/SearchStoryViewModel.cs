using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebLightNovel.Models.ModelStory
{
    public class SearchStoryViewModel
    {
        public int story_id { get; set; }
        public string title { get; set; }
        public string cover_image { get; set; }
        public int state { get; set; }     
        public int? newChapter_id { get; set; }
        public string chapter_name { get; set; }
        public string listGenres { get; set; }
        public string link_story { get; set; }
    }
}