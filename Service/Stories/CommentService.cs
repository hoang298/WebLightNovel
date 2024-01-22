using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLightNovel.Extensions;
using WebLightNovel.Models.Entity;

namespace WebLightNovel.Service.Stories
{
    public class CommentService : ICommentService
    {
        private readonly Connect_sql _db;
        public CommentService()
        {
            _db = new Connect_sql();
        }
        public void Insert(Comment comment)
        {
            Guard.NotNull(comment, nameof(comment));
            _db.Comments.Add(comment);
            try
            {
                _db.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void Update(Comment comment)
        {
            Guard.NotNull(comment, nameof(comment));

        }
        public void Delete(object comment_id)
        {
            Comment comment = _db.Comments.Find(comment_id);
            if (comment != null)
            {
                if (comment.id_parent != 0)
                {
                    _db.Comments.Remove(comment);
                    _db.SaveChanges();
                }
                else
                {
                    List<Comment> list = _db.Comments.Where(h => h.id_parent == (int)comment_id).ToList();
                    if (list.Count > 0)
                    {
                        foreach (var item in list)
                        {
                            _db.Comments.Remove(item);
                        }

                    }
                    _db.Comments.Remove(comment);
                    _db.SaveChanges();
                }
            }
        }
        public Comment GetById(object comment_id)
        {
            if ((int)comment_id == 0)
                return null;
            return _db.Comments.Find(comment_id);
        }
        public List<Comment> GetAll()
        {
            return _db.Comments.ToList();
        }
        public List<Comment> GetCommentByStoryId(int story_id)
        {
            return _db.Comments.Where(h => h.story_id == story_id).ToList();
        }
       public List<Comment> GetCommentChildren(int commentParentId)
        {
            return _db.Comments.Where(h => h.id_parent == commentParentId).ToList();
        }

        public int GetCount()
        {
            return _db.Comments.ToList().Count;
        }
    }
}
