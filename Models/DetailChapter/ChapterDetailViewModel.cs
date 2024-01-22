using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebLightNovel.Models.DetailChapter
{
    public class ChapterDetailViewModel
    {
        public int chapter_id { get; set; }
        public string name { get; set; }
        public string content { get; set; }
        public int story_id { get; set; }

    }
}