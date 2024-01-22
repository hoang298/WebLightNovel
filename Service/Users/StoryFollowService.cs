using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebLightNovel.Models.Entity;
using WebLightNovel.Models.ModelStory;

namespace WebLightNovel.Service.Users
{
    public class StoryFollowService : IStoryFollowService
    {
        private readonly Connect_sql _db;
        public StoryFollowService()
        {
            _db = new Connect_sql();
        }
        public List<StoryFollow> GetAllByUserId(string user_id)
        {
            List<StoryFollow> list_StoryFollow = _db.StoryFollows.Where(h => h.user_id == user_id && h.state == 1).ToList();
            
            return list_StoryFollow;
        }
       public StoryFollow GetByStoryID(string user_id, int story_id)
        {
            return _db.StoryFollows.Where(h => h.user_id == user_id && h.story_id == story_id).FirstOrDefault();
        }
        public void Insert(StoryFollow entity)
        {
            _db.StoryFollows.Add(entity);
            try
            {
                _db.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool UpdateStoryFollow(StoryFollow item)
        {
            if (item != null)
            {
                StoryFollow storyFollow = _db.StoryFollows.Find(item);
                storyFollow.total_unread = 0;
                try
                {
                    _db.SaveChanges();
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        public void Update(StoryFollow entity)
        {

        }
        public void Delete(object id)
        {

        }

        public List<StoryFollow> GetAll()
        {
            return _db.StoryFollows.ToList();
        }
        public void UpdateListStoryFollow(List<StoryFollow> storyFollows)
        {
            foreach (var item in storyFollows)
            {
                item.total_unread += 1;
            }
            _db.SaveChanges();
        }
    }
}