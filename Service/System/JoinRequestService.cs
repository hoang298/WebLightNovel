using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebLightNovel.Interfaces;
using WebLightNovel.Models.Entity;

namespace WebLightNovel.Service.System
{
    public class JoinRequestService : IRepository<JoinRequest>
    {
        private readonly Connect_sql _db;
        public JoinRequestService()
        {
            _db = new Connect_sql();
        }
        public void Delete(object id)
        {
            throw new NotImplementedException();
        }

        public List<JoinRequest> GetAll()
        {
            throw new NotImplementedException();
        }

        public JoinRequest GetById(object id)
        {
            throw new NotImplementedException();
        }

        public int GetCount()
        {
            throw new NotImplementedException();
        }

        public void Insert(JoinRequest entity)
        {
            _db.JoinRequests.Add(entity);
            try
            {
                _db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(JoinRequest entity)
        {
            throw new NotImplementedException();
        }
        public List<JoinRequest> GetByUserId(string user_id)
        {
            return _db.JoinRequests.Where(h => h.sender_id == user_id).ToList();
        }
        public List<JoinRequest> GetByTransId(int trans_id)
        {
            return _db.JoinRequests.Where(h => h.trans_id == trans_id).ToList();
        }
    }
}