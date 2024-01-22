using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebLightNovel.Extensions;
using WebLightNovel.Models.Account;
using WebLightNovel.Models.Detail;
using WebLightNovel.Models.Entity;

namespace WebLightNovel.Service.Common
{
    public class TranslationTeamService : ITranslationTeamService
    {
        private readonly Connect_sql _db;
        private readonly UserManager<ApplicationUser> userManager;
        public TranslationTeamService()
        {
            _db = new Connect_sql();
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_db));
        }
        public TranslationTeam GetById(object translationTeam_id)
        {
            if ((int)translationTeam_id == 0)
                return null;
            return _db.TranslationTeams.Find(translationTeam_id);
        }
        public List<TranslationTeam> GetAll()
        {
            return _db.TranslationTeams.ToList();
        }
        public void Insert(TranslationTeam translationTeam)
        {
            Guard.NotNull(translationTeam, nameof(translationTeam));
            _db.TranslationTeams.Add(translationTeam);
        }
        public void InsertMember(TranslationUser user)
        {
            _db.TranslationUsers.Add(user);
            try
            {
                _db.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void Update(TranslationTeam translationTeam)
        {
            Guard.NotNull(translationTeam, nameof(TranslationTeam));
        }
        public void Delete(object translationTeam)
        {
            Guard.NotNull(translationTeam, nameof(translationTeam));
            TranslationTeam item = _db.TranslationTeams.Find(translationTeam);
            if (item != null)
                _db.TranslationTeams.Remove(item);
        }
        public List<TranslationTeam> GetByName(string name)
        {
            return _db.TranslationTeams.Where(h => h.name.Contains(name)).ToList();
        }
        public List<TransViewModel> GetAllInfo()
        {
            return (from item in _db.TranslationTeams
                    join item1 in userManager.Users on item.user_id equals item1.Id
                    select new TransViewModel
                    {
                        avatar = item1.avatar,
                        user_id = item1.Id,
                        trans_name = item.name,
                        user_name = item1.name,
                        trans_id = item.translation_id
                    }).ToList();
        }
        public List<TranslationTeam> GetByListTransId(object arrId)
        {
            IEnumerable<int> listId = arrId as IEnumerable<int>;
            return  _db.TranslationTeams
                .Where(h => listId.Contains(h.translation_id)).ToList();
        }

        public int GetCount()
        {
            return _db.TranslationTeams.ToList().Count;
        }

        public List<TranslationUser> GetMemberByTransId(object translationTeam_id)
        {
            return _db.TranslationUsers.Where(h => h.translation_id == (int)translationTeam_id).ToList();
        }    
        public void DeleteMember(TranslationUser user)
        {
            _db.TranslationUsers.Remove(user);
            try
            {
                _db.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<TranslationUser> GetAllTransUser()
        {
            return _db.TranslationUsers.ToList();
        }
    }
}