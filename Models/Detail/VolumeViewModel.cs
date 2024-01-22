using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebLightNovel.Models.Detail
{
    public class VolumeViewModel
    {
        public string name;
        public string volumeImg;
        public int volume_id;
        public List<ChapterViewModel> list_chap = new List<ChapterViewModel>();
    }
}