using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebLightNovel.Extensions;
using WebLightNovel.Models.Entity;

namespace WebLightNovel.Service.Users
{
    public class LetterService : ILetterService
    {
        private readonly Connect_sql _db;
        public LetterService()
        {
            _db = new Connect_sql();
        }
        public List<Letter> GetAllByUserId(string user_id)
        {
            return _db.Letters
                .Where(h => h.sender_id == user_id || h.receiver_id == user_id && h.isDeleteSender == false)
                .OrderByDescending(h => h.time).ToList();
        }
        public List<Letter> GetLettersByUser(string user_id, string type)
        {
            if (type == "send")
                return _db.Letters.Where(h => h.sender_id == user_id && h.isDeleteSender == false).OrderByDescending(h => h.time).ToList();
            return _db.Letters.Where(h => h.receiver_id == user_id && h.isDeleteReceiver == false).OrderByDescending(h => h.time).ToList();
        }
        public void Insert(Letter entity)
        {
            Guard.NotNull(entity, nameof(entity));
            _db.Letters.Add(entity);
            try
            {
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi từ InsertLetter ", ex);
            }
        }
        public void Update(Letter entity)
        {

        }
        public void Delete(int[] arrLetterId, string user_id)
        {
            foreach (var item in arrLetterId)
            {
                Letter letter = _db.Letters.Find(item);
                if (letter == null)
                    continue;
                if (user_id == letter.receiver_id)
                    letter.isDeleteReceiver = true;
                else
                    letter.isDeleteSender = true;
                try
                {
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi từ DeleteLetter ", ex);
                }
            }
        }
        public void Delete(object id)
        {

        }
        public List<Letter> GetAll()
        {
            return _db.Letters.ToList();
        }
    }
}