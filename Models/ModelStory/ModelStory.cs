using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebLightNovel.Models.ModelStory
{
    public class ModelChapter
    {
        public int chapter_id;
        public string update_day;
        public string chapter_name;
        public int view;
    }
    public class ModelStory
    {
        public string title { get; set; }
        public string cover_image { get; set; }
        public int story_id { get; set; }
        public int? total_views { get; set; }

        public int? follow_views { get; set; }

        public int? total_word { get; set; }

        public int? newChapter_id { get; set; }

        public DateTime? timeUpdate { get; set; }
        public string chapter_name { get; set; }
        public string link_story { get; set; }
        public string link_chapter { get; set; }
    }
    public class ModelSearchChapter
    {
        public int chapter_id;
        public string chapter_name;
        public string image_name;
        public string matchRate;
        public string link_chapter;
    }

    public class mStoryFollow {
        public int story_id { get; set; }
        public string title { get; set; }
        public string cover_image { get; set; }
        public int type { get; set; }
        public int total_unread { get; set; }
        public int newChapter_id { get; set; }
        public string chapter_name { get; set; }
        public DateTime timeUpdate { get; set; }
        public string link_story { get; set; }
        public string link_chapter { get; set; }
    }

}