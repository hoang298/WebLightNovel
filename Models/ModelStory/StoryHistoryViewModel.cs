using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebLightNovel.Models.ModelStory
{
    public class StoryHistoryViewModel
    {
        public string chapterName { get; set; }
        public string image { get; set; }
        public int story_id { get; set; }
        public int chapter_id { get; set; }
        public string story_name { get; set; }
    }
}