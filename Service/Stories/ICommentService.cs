using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLightNovel.Interfaces;
using WebLightNovel.Models.Entity;

namespace WebLightNovel.Service.Stories
{
    public interface ICommentService : IRepository<Comment>
    {
        List<Comment> GetCommentByStoryId(int story_id);
        List<Comment> GetCommentChildren(int commentParentId);
    }
}
