using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLightNovel.Interfaces;
using WebLightNovel.Models.Entity;

namespace WebLightNovel.Service.Stories
{
    public interface IChapterService : IRepository<Chapter>
    {
        List<Chapter> GetChaptersByStoryId(int story_id);
        List<ImageChapter> GetImageByChapterId(int chapter_id);
        List<Chapter> GetChaptersByListChapterId(List<int> arrId);
    }
}
