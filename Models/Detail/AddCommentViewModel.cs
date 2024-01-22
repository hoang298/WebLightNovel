using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebLightNovel.Models.Detail
{
    public class AddCommentViewModel
    {
        public string content { get; set; }
        public string user_id { get; set; }
        public string receiver_id { get; set; }
        public DateTime time { get; set; }
        public int? story_id { get; set; }
        public string story_name { get; set; }
        public int? id_parent { get; set; }
        public string avatar { get; set; }
        public string user_name { get; set; }
        public string link_post { get; set; }
    }
}