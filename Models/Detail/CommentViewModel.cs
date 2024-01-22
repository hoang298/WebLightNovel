using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebLightNovel.Models.Detail
{
    public class CommentViewModel
    {
        public int comment_id { get; set; }
        public string content { get; set; }
        public string user_id { get; set; }
        public string time { get; set; }
        public int? story_id { get; set; }
        public int? id_parent { get; set; }
        public string avatar { get; set; }
        public string user_name { get; set; }
        //public List<CommentViewModel> listCommentChild;
        public int countChil { get; set; }
    }
}