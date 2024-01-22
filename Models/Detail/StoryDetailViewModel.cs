using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebLightNovel.Models.Detail
{
    public class StoryDetailViewModel
    {
        public int story_id { get; set; }

        public string title { get; set; }

        public int author_id { get; set; }
        public string author_name { get; set; }
        public int? artist_id { get; set; }
        public string artist_name { get; set; }
        public string cover_image { get; set; }

        public string synopsis { get; set; }

        public int state { get; set; }
        public string state_name { get; set; }
        public int translation_id { get; set; }

        public int type { get; set; }
        public string type_name { get; set; }
        public int? total_views { get; set; }

        public int? follow_views { get; set; }

        public int? total_word { get; set; }

        public int? newChapter_id { get; set; }

        public DateTime? timeUpdate { get; set; }

        public string chapter_name { get; set; }

        public int? day_views { get; set; }

        public int? month_views { get; set; }

        public int? comment_views { get; set; }
    }
}