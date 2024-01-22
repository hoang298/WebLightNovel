using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebLightNovel.Models.SystemModel
{
    public class StorySystemViewModel
    {
        public string story_name { get; set; }
        public int story_id { get; set; }
        public List<Volume_StorySystem> listVol { get; set; }
        public List<Chapter_StorySystem> listChap { get; set; }
    }
    public class Volume_StorySystem
    {
        public int volume_id { get; set; }
        public string name { get; set; }
        public int ordinalNumber { get; set; }
    }
    public class Chapter_StorySystem
    {
        public int chapter_id { get; set; }
        public int volume_id { get; set; }
        public string name { get; set; }
        public int ordinalNumber { get; set; }

    }
}