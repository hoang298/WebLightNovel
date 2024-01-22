using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebLightNovel.Models.Detail
{
    public class ChapterViewModel
    {
        public int chapter_id { get; set; }

        public string name { get; set; }

        public int ordinalNumber { get; set; }

        public int volume_id { get; set; }

        public string newtimeUpdate { get; set; }
        public int story_id { get; set; }
        public string link_chapter { get; set; }
    }
}