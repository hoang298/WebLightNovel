using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLightNovel.Models.Entity;
using WebLightNovel.Models.ModelStory;

namespace WebLightNovel.Service.Users
{
   public interface IStoryHistoryService
    {
        List<StoryHistoryViewModel> GetStoryHistory(string user_id);

        void AddStoryToHistory(string user_id, int story_id, int chapter_id);
        void DeleteStoryHistory(string user_id, int id);
    }
}
