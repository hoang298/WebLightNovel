using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebLightNovel.Models.Entity;
using WebLightNovel.Models.ModelStory;

namespace WebLightNovel.Service.Users
{
    public class StoryHistoryService : IStoryHistoryService
    {
        private readonly Connect_sql _db;
        public StoryHistoryService()
        {
            _db = new Connect_sql();
        }
        public List<StoryHistoryViewModel> GetStoryHistory(string user_id)
        {
            List<HistoryStory> listStoryHistory = _db.HistoryStories
                .Where(h => h.user_id == user_id).ToList()
                .OrderByDescending(h => h.time).ToList();
            List<int> listStoryId = listStoryHistory.Select(h => h.story_id).ToList();
            List<int> listChapterId = listStoryHistory.Select(h => h.chapter_id).ToList();
            List<Story> listStory = _db.Stories.Where(h => listStoryId.Contains(h.story_id)).ToList();
            List<Chapter> listChapter = _db.Chapters.Where(h => listChapterId.Contains(h.chapter_id)).ToList();
            var query = (from item in listStoryHistory
                         from item1 in listStory
                         from item2 in listChapter
                         where item.story_id == item1.story_id
                         && item.chapter_id == item2.chapter_id
                         select new StoryHistoryViewModel
                         {
                             chapterName = item2.name,
                             image = item1.cover_image,
                             story_id = item.story_id,
                             chapter_id = item.chapter_id,
                             story_name = item1.title
                         });
            return query.ToList();
        }

        public void AddStoryToHistory(string user_id, int story_id, int chapter_id)
        {
            HistoryStory item = new HistoryStory();
            List<HistoryStory> listHistory = _db.HistoryStories.Where(h => h.user_id == user_id).ToList();
            if (listHistory.Where(h => h.story_id == story_id).FirstOrDefault() == null)
            {
                try
                {
                    item.story_id = story_id;
                    item.chapter_id = chapter_id;
                    item.user_id = user_id;
                    item.time = DateTime.Now;
                    _db.HistoryStories.Add(item);
                    _db.SaveChanges();
                }
                catch (Exception)
                {

                    throw;
                }
            }
            else
            {
                HistoryStory item1 = listHistory.Where(h => h.story_id == story_id).FirstOrDefault();
                item1.chapter_id = chapter_id;
                item1.time = DateTime.Now;
                _db.SaveChanges();
            }
        }
        public void DeleteStoryHistory(string user_id, int id)
        {
            HistoryStory history = _db.HistoryStories.Where(h => h.user_id == user_id && h.story_id == id).FirstOrDefault();
            if (history != null)
            {
                _db.HistoryStories.Remove(history);
                _db.SaveChanges();
            }
        }
    }
}