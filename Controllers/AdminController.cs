using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebLightNovel.Models.Account;
using WebLightNovel.Models.Entity;
namespace WebLightNovel.Controllers
{
    public class AdminController : Controller
    {
        Connect_sql db = new Connect_sql();
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["admin"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("LoginAdmin","Admin");
            }       
        }

        public ActionResult LoginAdmin()
        {
            return View();
        }
      /*  public ActionResult CheckLoginUser(string user, string password)
        {
            UserModel item = db.UserAccounts.Where(h => h.user_name == user && h.user_password == password).FirstOrDefault();
            if (item != null && item.role == 1)
            {
                string user_id = item.user_id.ToString();                         
                //Xác thực người dùng đã đăng nhập
                FormsAuthentication.SetAuthCookie(user, false);
                //Phân quyền cho người dùng
                Session["admin"] = item;
                return Redirect("Index");
            }
            return View("LoginAdmin");
        }*/

        public ActionResult ListStory()
        {
            List<Story> l = db.Stories.Take(10).ToList();
            ViewBag.soTrang = db.Stories.ToList().Count / 10 + 1;
            return View(l);
        }
        public ActionResult ListPostStory()
        {
            List<PostStory> postStories = db.PostStories.Where(h => h.status_story == 0).ToList();
            return View(postStories);
        }
        public ActionResult DetailPostStory(int id)
        {
            PostStory postStory = db.PostStories.Where(h => h.ordinalNumber == id).FirstOrDefault();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var user = userManager.FindById(postStory.user_id);
            ViewBag.userName = user;
            return View();
        }
        public ActionResult ConfirmPost(int post_id, int state)
        {
            PostStory postStory = db.PostStories.Where(h => h.ordinalNumber == post_id).FirstOrDefault();
            if(state == 1 && postStory != null)
            {
                Story story = new Story();
                Author author = db.Authors.Where(h => h.name == postStory.author_name).FirstOrDefault();
                if(postStory.artist_name != "")
                {
                    Artist artist = db.Artists.Where(h => h.name == postStory.artist_name).FirstOrDefault();
                    if (artist != null)
                        story.artist_id = artist.artist_id;
                    else
                    {
                        Artist artist1 = new Artist();
                        artist1.name = postStory.artist_name;
                        db.Artists.Add(artist1);
                        if(db.SaveChanges() > 0)
                            story.artist_id = artist1.artist_id;
                    }
                }
                if (author == null)
                {
                    Author author1 = new Author();
                    author1.name = postStory.author_name;
                    db.Authors.Add(author1);
                    if(db.SaveChanges() > 0)
                        story.author_id = author1.author_id;
                }
                else
                    story.author_id = author.author_id;
                story.title = postStory.title;
                story.cover_image = postStory.image;
                story.state = 1;
                story.synopsis = postStory.synopsis;
                story.translation_id = postStory.trans_id;
                story.type = (int)postStory.type_story;
                story.total_views = 0;
                story.follow_views = 0;
                story.total_word = 0;
                db.Stories.Add(story);
                if (db.SaveChanges() > 0)
                {
                    UserModel us = Session["admin"] as UserModel;
                    Notify notify = new Notify();
                    notify.sender_id = us.user_id;
                    notify.user_id = postStory.user_id;
                    notify.time = DateTime.Now;
                    notify.content = us.name + " đã duyệt truyện " + postStory.title + " mà bạn đã đăng";
                    notify.url_notify = "/System";
                    notify.state = 0;
                    db.Notifies.Add(notify);
                    db.SaveChanges();
                    return Content("true", "text/html");
                }
            }
            else if(state == 0 && postStory != null)
            {
                postStory.status_story = -1;
                db.SaveChanges();
                return Content("true", "text/html");
            }
            return Content("false", "text/html");
        }
        public ActionResult PhanTrang(int page)
        {
            int soluong = 10;
            List<Story> l = db.Stories.OrderBy(h => h.story_id).Skip(soluong * (page - 1)).Take(10).ToList();
            try
            {
                string content = "";
                int i = 1;
                foreach (var item in l)
                {
                    content += "<tr>" +
                                    "<th>" + i + "</th>" +
                                    "<td>" + item.story_id + "</td>" +
                                    "<td>" + item.title + "</td>" +
                                    "<td><img src = \"/image/" + item.cover_image + "\"/>" + "</td>" +
                                    "<td>" +
                                        "<a href=\"/Admin/EditStory?id=" + item.story_id + "\"class=\"btn btn-primary btn-icon\"><i class=\"fa fa-edit\"></i></a>" +
                                        "<a href=\"/Admin/Delete?id=" + item.story_id + "\"class=\"btn btn-danger btn-icon\"><i class=\"fa fa-trash\"></i></a>" +
                                    "</td>" +
                            "</tr>";
                    i++;
                }
                content += "";
                return Content(content, "text/html");
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Admin");
            }
        }

        public ActionResult EditStory(int id)
        {
            Story item = db.Stories.Where(h => h.story_id == id).FirstOrDefault();
            ViewBag.authorName = db.Authors.Where(h => h.author_id == item.author_id).Select(h => h.name).FirstOrDefault();
            ViewBag.artistName = db.Artists.Where(h => h.artist_id == item.artist_id).Select(h => h.name).FirstOrDefault();
            ViewBag.transName = db.TranslationTeams.Where(h => h.translation_id == item.translation_id).Select(h => h.name).FirstOrDefault();
            return View(item);
        }
        [HttpPost]
        public bool UpdateStory(int story_id, string title, string author, string artist, string synopsis, int state, string trans)
        {
            Story item = db.Stories.Where(h => h.story_id == story_id).FirstOrDefault();
            if (item == null)
                return false;
            else
            {
                item.title = title;
                item.author_id = db.Authors.Where(h => h.name == author).Select(h => h.author_id).FirstOrDefault();
                item.artist_id = db.Artists.Where(h => h.name == artist).Select(h => h.artist_id).FirstOrDefault();
                item.synopsis = synopsis;
                item.state = state;
                item.translation_id = db.TranslationTeams.Where(h => h.name == trans).Select(h => h.translation_id).FirstOrDefault();
                db.SaveChanges();
            }
            return true;
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Story sp, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0 && ModelState.IsValid)
            {
                string _FileName = Path.GetFileName(file.FileName);
                string _path = Path.Combine(Server.MapPath("~/image"), _FileName);
                file.SaveAs(_path);
                sp.cover_image = _FileName;
                db.Stories.Add(sp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Create");
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            Story sanpham = db.Stories.Where(h => h.story_id == id).FirstOrDefault();
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sanpham);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult XacNhanXoa(int id)
        {
            Story sanpham = db.Stories.Where(h => h.story_id == id).FirstOrDefault();

            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.Stories.Remove(sanpham);
            db.SaveChanges();
            return Redirect("/Admin/ListStory");
        }
        [HttpGet]
        public ActionResult InsertVolume(int id)
        {
            TempData["story_id"] = id;
            return View();
        }
        [HttpPost]
        public ActionResult InsertVolume(Volume sp, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0 && ModelState.IsValid)
            {
                string _FileName = Path.GetFileName(file.FileName);
                string _path = Path.Combine(Server.MapPath("~/image_vol"), _FileName);
                file.SaveAs(_path);
                sp.volumeImg = _FileName;
                db.Volumes.Add(sp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("InsertVolume");
        }
        public ActionResult EditVolume(int id)
        {
            return View(db.Volumes.Where(h => h.volume_id == id).FirstOrDefault());
        }
        [HttpGet]
        public ActionResult InsertChapter(int id, int story_id)
        {
            TempData["volume_id"] = id;
            TempData["story_id"] = story_id;
            return View();
        }
        [HttpPost]
        public ActionResult InsertChapter(Chapter sp)
        {
            if (ModelState.IsValid)
            {
                db.Chapters.Add(sp);
                db.SaveChanges();
                return RedirectToAction("ListStory");
            }
            return RedirectToAction("Index");
        }

        public ActionResult ListVolume(int id)
        {
            return View(db.Volumes.Where(h => h.story_id == id).ToList());
        }
        [HttpGet]
        public ActionResult DeleteVolume(int id)
        {
            Volume volume = db.Volumes.Where(h => h.volume_id == id).FirstOrDefault();
            if (volume == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(volume);
        }
        [HttpPost, ActionName("DeleteVolume")]
        public ActionResult XacNhanXoaVolume(int id)
        {
            Volume volume = db.Volumes.Where(h => h.volume_id == id).FirstOrDefault();
            if (volume == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            int story_id = volume.story_id;
            db.Volumes.Remove(volume);
            db.SaveChanges();
            return Redirect("/Admin/ListVolume?id=" + story_id);
        }
        public ActionResult ListChapter(int id)
        {
            return View(db.Chapters.Where(h => h.volume_id == id).ToList());
        }
        [HttpGet]
        public ActionResult DeleteChapter(int id)
        {
            Chapter chapter = db.Chapters.Where(h => h.chapter_id == id).FirstOrDefault();
            if (chapter == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(chapter);
        }
        [HttpPost, ActionName("DeleteChapter")]
        public ActionResult XacNhanXoaChapter(int id)
        {
            Chapter chapter = db.Chapters.Where(h => h.chapter_id == id).FirstOrDefault();
            if (chapter == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            int? volume_id = chapter.volume_id;
            int? story_id = chapter.story_id;
            db.Chapters.Remove(chapter);
            db.SaveChanges();
            return Redirect("/Admin/ListChapter?id=" + volume_id + "&story_id" + story_id);
        }
    }
}