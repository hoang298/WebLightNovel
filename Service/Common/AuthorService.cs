using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebLightNovel.Extensions;
using WebLightNovel.Interfaces;
using WebLightNovel.Models.Entity;

namespace WebLightNovel.Service.Common
{
    public class AuthorService : IRepository<Author>
    {
        private readonly Connect_sql _db;
        public AuthorService()
        {
            _db = new Connect_sql();
        }
        public Author GetById(object author_id)
        {
            if ((int)author_id == 0)
                return null;
            return _db.Authors.Find(author_id);
        }
        public List<Author> GetAll()
        {
            return _db.Authors.ToList();
        }
        public void Insert(Author author)
        {
            Guard.NotNull(author, nameof(Author));
            _db.Authors.Add(author);
        }
        public void Update(Author author)
        {
            Guard.NotNull(author, nameof(author));
        }
        public void Delete(object author)
        {
            Guard.NotNull(author, nameof(author));
            Author item = _db.Authors.Find(author);
            if (item != null)
                _db.Authors.Remove(item);
        }
        public List<Author> GetByName(string name)
        {
            return _db.Authors.Where(h => h.name.Contains(name)).ToList();
        }

        public int GetCount()
        {
            return _db.Authors.ToList().Count;
        }
    }

}