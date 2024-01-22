using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebLightNovel.Models.Entity;

namespace WebLightNovel.Models.SystemModel
{
    public class CreateStoryViewModel
    {
        public List<Genre> listGenres { get; set; }
        public List<CreateStoryTransViewModel> listTrans { get; set; }
    }
    public class CreateStoryTransViewModel
    {
        public int translation_id { get; set; }
        public string name { get; set; }
    }
}