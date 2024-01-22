using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLightNovel.Models.Entity;

namespace WebLightNovel.Service.Stories
{
    public class VolumeService : IVolumeService
    {
        private readonly Connect_sql _db;
        public VolumeService()
        {
            _db = new Connect_sql();
        }
        public List<Volume> GetAll()
        {
            return _db.Volumes.ToList();
        }
        public Volume GetById(object id)
        {           
            return _db.Volumes.Find((int)id);
        }
        public void Insert(Volume volume)
        {
            _db.Volumes.Add(volume);
            try
            {
                _db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Update(Volume volume)
        {

        }
        public void Delete(object id)
        {

        }
       public List<Volume> GetVolumesByStoryId(int story_id)
        {
            return _db.Volumes.Where(h => h.story_id == story_id).ToList();
        }
        public int GetCount()
        {
            return _db.Volumes.Select(h => h.volume_id).ToList().Count;
        }
    }
}
