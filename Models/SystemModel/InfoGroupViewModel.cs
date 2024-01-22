using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static WebLightNovel.Models.Enum.EnumModel;

namespace WebLightNovel.Models.SystemModel
{
    public class InfoGroupViewModel
    {
        public List<InfoGroupStoryViewModel> ListStories { get; set; }
        public List<MemberTranslation> MemberTranslation { get; set; }
        public List<ReqJoin> ListReqJoin { get; set; }
    }
    public class ReqJoin
    {
        public string user_name { get; set; }
        public DateTime date_req { get; set; }
        public string user_id { get; set; }

    }
    public class InfoGroupStoryViewModel
    {
     
        public int story_id { get; set; }
        public string title { get; set; }
        public statusStory state { get; set; }
        public string image { get; set; }
    }
    public class MemberTranslation
    {
        public string name { get; set; }
        public string user_id { get; set; }
        public string role { get; set; }
        public DateTime dateJoin { get; set; }
    }
}