using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
    public class AccountService : IAccountService
    {
        UserManager<ApplicationUser> userManager;
        private readonly Connect_sql _db;
        public AccountService()
        {
            _db = new Connect_sql();
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_db));
        }
        public List<ApplicationUser> GetListUserById(List<string> listId)
        {
            return userManager.Users.Where(h => listId.Contains(h.Id)).ToList();
        }
        public ApplicationUser GetUserById(string user_id)
        {
            return userManager.Users.Where(h => h.Id == user_id).FirstOrDefault();
        }
        public ApplicationUser GetUserByName(string name)
        {
           return userManager.Users.Where(h => h.name == name.Trim()).FirstOrDefault();
        }
        public bool UpdateAvatar(string user_id, string avatar)
        {
            ApplicationUser user = userManager.FindById(user_id);
            user.avatar = avatar;
            var result = userManager.Update(user);
            if (result.Succeeded)
                return true;
            return false;
        }
        public List<SeriesViewModel> GetAllTransByUserId(string user_id,int role,int theLoai)
        {
            var listTransUser = (from item in _db.TranslationTeams
                                 from item1 in _db.TranslationUsers
                                 from item2 in _db.Stories
                                 where item1.translation_id == item2.translation_id
                                 && item.translation_id == item1.translation_id
                                 && item1.user_id == user_id
                                 && item2.type == theLoai
                                 && item1.role == role
                                 select new SeriesViewModel
                                 {
                                     story_name = item2.title,
                                     trans_name = item.name,
                                     story_id = item2.story_id
                                 }).ToList();
            return listTransUser;
        }
    }
}