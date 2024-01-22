using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebLightNovel.Interfaces;
using WebLightNovel.Models.Entity;

namespace WebLightNovel.Service.Stories
{
    public interface IStoryService : IRepository<Story>
    {    
        List<Story> GetStoryByTag(string tag);

        List<Story> GetStoryByType(int type);
        List<Story> GetStoryByName(string name);
        List<Story> GetStoryByTransId(int translation_id);
        List<Story> GetStoryByAuthorId(int author_id);
        List<Story> GetStoryByGenre(int genre_id);
        List<Story> GetStoriesByListStoryId(object arrId);
    }
}