using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebLightNovel.Models.Account;
using WebLightNovel.Models.Entity;
using WebLightNovel.Models.Member;

namespace WebLightNovel.Service.System
{
    public class AuthenticationService
    {
        private readonly Connect_sql _db;
        private readonly UserManager<ApplicationUser> userManager;
        public AuthenticationService()
        {
            _db = new Connect_sql();
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_db));
        }
        public List<UserRole> GetRole(string user_id)
        {
            var list_role = from item in _db.TranslationUsers
                            from item1 in _db.Stories
                            where item.user_id == user_id
                            && item.translation_id == item1.translation_id                            
                            select new UserRole
                            {
                                translation_id = item.translation_id,
                                role = item.role,
                                story_id = item1.story_id,
                            };
            List<UserRole> userRoles = list_role.ToList();
            return userRoles;
        }
    }
}