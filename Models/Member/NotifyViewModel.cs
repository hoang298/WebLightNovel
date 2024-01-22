using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebLightNovel.Models.Member
{
    public class NotifyViewModel
    {
        public int notify_id { get; set; }

        public string content { get; set; }

        public string time { get; set; }

        public string url_notify { get; set; }

        public int state { get; set; }
        public string senderAvatar { get; set; }
    }
}