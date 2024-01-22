using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLightNovel.Interfaces;
using WebLightNovel.Models.Entity;

namespace WebLightNovel.Service.Users
{
   public interface INotifyService : IUsersService<Notify>
    {
        bool ConfirmReadNotify(List<Notify> listNotify);

    }
}
