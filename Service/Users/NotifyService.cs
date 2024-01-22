using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebLightNovel.Interfaces;
using WebLightNovel.Models.Entity;

namespace WebLightNovel.Service.Users
{
    public class NotifyService : INotifyService
    {
        private readonly Connect_sql _db;
        public NotifyService()
        {
            _db = new Connect_sql();
        }
       public List<Notify> GetAllByUserId(string user_id)
        {
            return _db.Notifies.Where(h => h.user_id == user_id).OrderByDescending(h => h.time).ToList();
        }  
        public bool ConfirmReadNotify(List<Notify> listNotify)
        {
            foreach (var item in listNotify)
            {
                item.state = 1;
            }
            if (_db.SaveChanges() > 0)
                return true;
            return false;
        }
        public void Insert(Notify entity)
        {
            _db.Notifies.Add(entity);
            try
            {
                _db.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void Update(Notify entity)
        {

        }
        public void Delete(object notify_id)
        {

        }

        public List<Notify> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}