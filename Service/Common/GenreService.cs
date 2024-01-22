using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebLightNovel.Models.Entity;

namespace WebLightNovel.Service.Common
{
    public class GenreService : IGenreService
    {
        readonly Connect_sql _db;
        public GenreService()
        {
            _db = new Connect_sql();
        }
        public List<Genre> GetAllGenre()
        {
            return _db.Genres.ToList();
        }
        public Genre GetGenreById(int id)
        {
            Genre genre = _db.Genres.Find(id);
            return genre;
        }
        public Genre GetGenreByName(string name)
        {
            return _db.Genres.Where(h => h.name == name).FirstOrDefault();
        }
        public List<Genre> GetGenreByStoryId(int story_id)
        {
            var listGenres = (from item in _db.StoriesGenres
                              join item1 in _db.Genres on item.genres_id equals item1.genres_id
                              where item.story_id == story_id
                              select item1).ToList();
            return listGenres;
        }
    }
}