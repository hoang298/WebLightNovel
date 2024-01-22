using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebLightNovel.Extensions;
using WebLightNovel.Interfaces;
using WebLightNovel.Models.Entity;

namespace WebLightNovel.Service.Stories
{
    public class StoryService : IStoryService
    {
        private readonly Connect_sql _db;
        public StoryService()
        {
            _db = new Connect_sql();
        }
        public void Insert(Story story)
        {
            Guard.NotNull(story, nameof(story));
            _db.Stories.Add(story);
        }
       public void Update(Story story)
        {
            Guard.NotNull(story, nameof(story));
           
        }
       public void Delete(object story)
        {
            Guard.NotNull(story, nameof(story));
            Story item = _db.Stories.Find(story);
            if(item != null)
                _db.Stories.Remove(item);
        }
       public Story GetById(object story_id)
        {
            if ((int)story_id == 0)
                return null;
            return _db.Stories.Find(story_id);
        }
        public List<Story> GetAll()
        {
            return _db.Stories.ToList();
        }
        public List<Story> GetStoryByTag(string tag)
        {
            if(tag == "topView")
                return _db.Stories.OrderByDescending(h => h.total_views).Take(10).ToList();
            if(tag == "lastUpdate")
                return _db.Stories.OrderByDescending(h => h.timeUpdate).Take(31).ToList();
            if(tag == "topViewDay")
                return _db.Stories.OrderByDescending(h => h.day_views).Take(5).ToList();
            return null;
        }
        public List<Story> GetStoryByType(int type)
        {
            return _db.Stories.Where(h => h.type == type).ToList();
        }
        public List<Story> GetStoryByName(string name)
        {
           return _db.Stories.Where(h => h.title.Contains(name)).Take(20).ToList();
        }
        public List<Story> GetStoryByTransId(int translation_id)
        {
            return _db.Stories.Where(h => h.translation_id == translation_id).ToList();
        }
        public List<Story> GetStoryByAuthorId(int author_id)
        {
            return _db.Stories.Where(h => h.author_id == author_id).ToList();
        }
        public List<Story> GetStoryByGenre(int genre_id)
        {
            var listStory = from item in _db.Stories
                            join item1 in _db.StoriesGenres
                            on item.story_id equals item1.story_id
                            where item1.genres_id == genre_id
                            orderby item.timeUpdate descending
                            select item;
            return listStory.ToList();
        }
        public List<Story> GetStoriesByListStoryId(object arrId)
        {
            IEnumerable<int> listId = arrId as IEnumerable<int>;
            return _db.Stories.Where(h => listId.Contains(h.story_id)).ToList();
        }
        public int GetCount()
        {
            return _db.Stories.Select(h => h.story_id).ToList().Count; ;
        }
    }
}