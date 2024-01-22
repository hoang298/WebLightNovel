using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebLightNovel.Models.Member
{
    public class ModelMessenger
    {
        public string sender_id { get; set; }
        public string receiver_id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string time { get; set; }
        public int state { get; set; }
        public string sender_name { get; set; }
        public string avatar { get; set; }
        public int letter_id { get; set; }
    }
}