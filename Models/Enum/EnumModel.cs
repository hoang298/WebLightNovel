using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebLightNovel.Models.Enum
{
    public class EnumModel
    {
        public enum typeStory
        {
            composed = 0,
            translated = 1,
            convert = 2
        }
        public enum statusStory
        {
            stop = 0,
            active = 1,
            done = 2
        }
        public enum roleTrans
        {
            captain = 1,
            member = 0
        }
        public enum statusReq
        {
            not,
            waitting,
            succes
        }
    }
}