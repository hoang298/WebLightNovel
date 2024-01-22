using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebLightNovel.Models.Entity;

namespace WebLightNovel.Models.ModelStory
{
    public class StoryIndexViewModel
    {
        public int story_id { get; set; }
        public string title { get; set; }    
        public string cover_image { get; set; }
        public int state { get; set; }
     
        public int type { get; set; }

        public int? total_views { get; set; }

        public int? follow_views { get; set; }

        public int? total_word { get; set; }

        public int? newChapter_id { get; set; }

        public string newtimeUpdate { get; set; }
        public string chapter_name { get; set; }
        public int? day_views { get; set; }

        public int? month_views { get; set; }

        public int? comment_views { get; set; }
        public string link_story { get; set; }
        public string link_chapter { get; set; }
       
    }
}