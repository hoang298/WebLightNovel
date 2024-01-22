using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebLightNovel.Models.Account;
using WebLightNovel.Models.Entity;
using WebLightNovel.Models.ModelStory;
using WebLightNovel.Models.SystemModel;

namespace WebLightNovel.Service.Users
{
    public interface IAccountService
    {
        List<ApplicationUser> GetListUserById(List<string> listId);
        ApplicationUser GetUserById(string user_id);
        ApplicationUser GetUserByName(string name);
        bool UpdateAvatar(string user_id,string avatar);
        List<SeriesViewModel> GetAllTransByUserId(string user_id, int role, int theLoai);
    }
}