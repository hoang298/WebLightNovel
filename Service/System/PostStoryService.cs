using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebLightNovel.Interfaces;
using WebLightNovel.Models.Entity;

namespace WebLightNovel.Service.System
{
    public class PostStoryService : IRepository<PostStory>
    {
        private readonly Connect_sql _db;
        public PostStoryService()
        {
            _db = new Connect_sql();
        }
        public void Delete(object id)
        {
           PostStory postStory = _db.PostStories.Find(id);
            if(postStory != null)
            {
                _db.PostStories.Remove(postStory);
                try
                {
                    _db.SaveChanges();
                }
                catch (Exception)
                {
                }
            }
        }

        public List<PostStory> GetAll()
        {
           return _db.PostStories.ToList();
        }

        public PostStory GetById(object id)
        {
            return _db.PostStories.Find(id);
        }

        public int GetCount()
        {
            return _db.PostStories.ToList().Count;
        }

        public void Insert(PostStory entity)
        {
            _db.PostStories.Add(entity);
            try
            {
                _db.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Update(PostStory entity)
        {
            throw new NotImplementedException();
        }
    }
}