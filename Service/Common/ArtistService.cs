using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebLightNovel.Extensions;
using WebLightNovel.Interfaces;
using WebLightNovel.Models.Entity;

namespace WebLightNovel.Service.Common
{
    public class ArtistService : IRepository<Artist>
    {
        private readonly Connect_sql _db;
        public ArtistService()
        {
            _db = new Connect_sql();
        }
        public Artist GetById(object artist_id)
        {
            if ((int)artist_id == 0)
                return null;
            return _db.Artists.Find(artist_id);
        }
        public List<Artist> GetAll()
        {
            return _db.Artists.ToList();
        }
        public void Insert(Artist artist)
        {
            Guard.NotNull(artist, nameof(artist));
            _db.Artists.Add(artist);
        }
        public void Update(Artist artist)
        {
            Guard.NotNull(artist, nameof(artist));
        }
        public void Delete(object artist)
        {
            Guard.NotNull(artist, nameof(artist));
            Artist item = _db.Artists.Find(artist);
            if (item != null)
                _db.Artists.Remove(item);
        }
        public List<Artist> GetByName(string name)
        {
            return _db.Artists.Where(h => h.name.Contains(name)).ToList();
        }

        public int GetCount()
        {
            return _db.Artists.ToList().Count;
        }
    }
}