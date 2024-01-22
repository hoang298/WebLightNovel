using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebLightNovel.Extensions;
using WebLightNovel.Interfaces;
using WebLightNovel.Models.Entity;

namespace WebLightNovel.Service.Stories
{
    public class ImageService : IRepository<ImageChapter>
    {
        private readonly Connect_sql _db;
        public ImageService()
        {
            _db = new Connect_sql();
        }
        public ImageChapter GetById(object imageId)
        {
            if ((int)imageId == 0)
                return null;
            return _db.ImageChapters.Find(imageId);
        }
        public List<ImageChapter> GetAll()
        {
            return _db.ImageChapters.ToList();
        }
        public void Insert(ImageChapter imageId)
        {
            Guard.NotNull(imageId, nameof(imageId));
            _db.ImageChapters.Add(imageId);
            _db.SaveChanges();
        }
        public void Update(ImageChapter imageId)
        {
            Guard.NotNull(imageId, nameof(imageId));
        }
        public void Delete(object imageId)
        {
            Guard.NotNull(imageId, nameof(imageId));
            ImageChapter item = _db.ImageChapters.Find(imageId);
            if (item != null)
                _db.ImageChapters.Remove(item);
        }
        public List<ImageChapter> GetByChapterId(int chapter_id)
        {
            return _db.ImageChapters.Where(h => h.chapter_id == chapter_id).ToList();
        }
        public int GetCount()
        {
            throw new NotImplementedException();
        }
    }
}