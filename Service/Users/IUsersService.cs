using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebLightNovel.Service.Users
{
    public interface IUsersService<T>
    {
        List<T> GetAllByUserId(string user_id);
        //T GetById(object id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(object id);
        List<T> GetAll();
    }
}
