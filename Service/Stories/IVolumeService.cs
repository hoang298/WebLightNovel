using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLightNovel.Interfaces;
using WebLightNovel.Models.Entity;

namespace WebLightNovel.Service.Stories
{
   public interface IVolumeService : IRepository<Volume>
    {
        List<Volume> GetVolumesByStoryId(int story_id);
    }
}
