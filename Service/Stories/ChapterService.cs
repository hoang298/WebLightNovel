using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebLightNovel.Models.Entity;

namespace WebLightNovel.Service.Stories
{
    public class ChapterService : IChapterService
    {
        private readonly Connect_sql _db;
        public ChapterService()
        {
            _db = new Connect_sql();
        }
        public List<Chapter> GetAll()
        {
            return _db.Chapters.ToList();
        }
        public Chapter GetById(object id)
        {
            return _db.Chapters.Find((int)id);
        }
        public void Insert(Chapter chapter)
        {
            _db.Chapters.Add(chapter);
            try
            {
                _db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Update(Chapter chapter)
        {

        }
        public void Delete(object id)
        {

        }
       public List<Chapter> GetChaptersByStoryId(int story_id)
        {
            return _db.Chapters.Where(h => h.story_id == story_id).ToList();
        }
        public List<Chapter> GetChaptersByListChapterId(List<int> arrId)
        {
            IEnumerable<int> listId = arrId as IEnumerable<int>;
            return _db.Chapters.Where(h => listId.Contains(h.chapter_id)).ToList();
        }
        public List<ImageChapter> GetImageByChapterId(int chapter_id)
        {
           return _db.ImageChapters.Where(h => h.chapter_id == chapter_id).ToList();
        }
        public int GetCount()
        {
            return _db.Chapters.Select(h => h.chapter_id).ToList().Count;
        }
    }
}