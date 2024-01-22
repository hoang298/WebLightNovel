using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLightNovel.Interfaces;
using WebLightNovel.Models.Detail;
using WebLightNovel.Models.Entity;

namespace WebLightNovel.Service.Common
{
    public interface ITranslationTeamService : IRepository<TranslationTeam>
    {
         List<TranslationTeam> GetByName(string name);
        List<TranslationTeam> GetByListTransId(object arrId);
        List<TransViewModel> GetAllInfo();
        List<TranslationUser> GetMemberByTransId(object translationTeam_id);
        void InsertMember(TranslationUser user);
        void DeleteMember(TranslationUser user);
        List<TranslationUser> GetAllTransUser();
    }
}
