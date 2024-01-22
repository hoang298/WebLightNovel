using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLightNovel.Models.Entity;

namespace WebLightNovel.Service.Users
{
    public interface IStoryFollowService : IUsersService<StoryFollow>
    {
        bool UpdateStoryFollow(StoryFollow item);
        StoryFollow GetByStoryID(string user_id, int story_id);
        void UpdateListStoryFollow(List<StoryFollow> storyFollows);
    }
}
