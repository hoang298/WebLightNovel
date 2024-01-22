using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static WebLightNovel.Models.Enum.EnumModel;
namespace WebLightNovel.Models.SystemModel
{
    public class SearchGroupViewModel
    {
        public int translation_id { get; set; }
        public string name { get; set; }
        public statusReq state { get; set; }
        public string user_name { get; set; }
        public string date_create { get; set; }
        public string state_req { get; set; }
    }  
}