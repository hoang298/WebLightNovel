using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebLightNovel.Models.Entity;
namespace WebLightNovel.Service.Common
{
    public interface IGenreService
    {
         List<Genre> GetAllGenre();
         Genre GetGenreById(int id);
        Genre GetGenreByName(string name);
        List<Genre> GetGenreByStoryId(int story_id);
    }
}