using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLightNovel.Models.Entity;

namespace WebLightNovel.Service.Users
{
    public interface ILetterService : IUsersService<Letter>
    {
        void Delete(int[] id, string user_id);
    }
}
