using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebLightNovel.Models.SystemModel
{
    public class SeriesViewModel
    {
        public int story_id { get; set; }
        public string story_name { get; set; }
        public string user_name { get; set; }
        public string trans_name { get; set; }
    }
}