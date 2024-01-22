using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLightNovel.Models.Entity;
using WebLightNovel.Models.ModelStory;
using WebLightNovel.Models.Account;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net;
using System.Diagnostics;
using AutoMapper;
using WebLightNovel.Models.Detail;
using WebLightNovel.Models.DetailChapter;
using Newtonsoft.Json;
using WebLightNovel.Extensions;
using WebLightNovel.Service.Common;
using WebLightNovel.Service.Users;
using WebLightNovel.Service.Stories;

namespace WebLightNovel.Controllers
{
    [HandleError]
    public class StoryController : Controller
    {
        // GET: Story
        static int limitStory = 32;
        static List<AuthorArtistViewModel> _listAuthor;
        static List<AuthorArtistViewModel> _listArtist;
        static List<TransViewModel> _listTrans;
        private readonly IGenreService _genreService;
        private readonly IStoryFollowService _storyFollowService;
        private readonly IStoryService _storyService;
        private readonly IVolumeService _volumeService;
        private readonly IChapterService _chapterService;
        private readonly ICommentService _commentService;
        private readonly IAccountService _accountService;
        private readonly INotifyService _notifyService;
        private readonly ITranslationTeamService _translationTeamService;
        private readonly ArtistService _artistService = new ArtistService();
        private readonly AuthorService _authorService = new AuthorService();
        public StoryController(
            IGenreService genreService,
            IStoryService storyService,
            IAccountService accountService,
            INotifyService notifyService
            )
        {
            _genreService = genreService;
            _storyService = storyService;
            _notifyService = notifyService;
            _accountService = accountService;
            _volumeService = new VolumeService();
            _chapterService = new ChapterService();
            _storyFollowService = new StoryFollowService();
            _commentService = new CommentService();
            _translationTeamService = new TranslationTeamService();
            _listAuthor = CacheManager.Instance.GetCache()["cachedDataAuthor"] as List<AuthorArtistViewModel>;
            _listArtist = CacheManager.Instance.GetCache()["cachedDataArtist"] as List<AuthorArtistViewModel>;
            _listTrans = CacheManager.Instance.GetCache()["cachedDataTrans"] as List<TransViewModel>;
            if (_listAuthor == null && _listArtist == null)
            {
                _listAuthor = _authorService.GetAll().Select(h => new AuthorArtistViewModel { id = h.author_id, name = h.name }).ToList();
                _listArtist = _artistService.GetAll().Select(h => new AuthorArtistViewModel { id = h.artist_id, name = h.name }).ToList();
                _listTrans = _translationTeamService.GetAllInfo();
                CacheManager.Instance.SetCache("cachedDataAuthor", _listAuthor, DateTimeOffset.Now.AddDays(1));
                CacheManager.Instance.SetCache("cachedDataArtist", _listArtist, DateTimeOffset.Now.AddDays(1));
                CacheManager.Instance.SetCache("cachedDataTrans", _listTrans, DateTimeOffset.Now.AddHours(1));
            }
        }       

        public TransViewModel GetInfoTrans(int trans_id)
        {
            TransViewModel trans = _listTrans.Where(h => h.trans_id == trans_id).FirstOrDefault();
            return trans;
        }
        public int CheckStatusFollow(int story_id)
        {
            if (User.Identity.IsAuthenticated)
            {
                string user_id = User.Identity.GetUserId();
                StoryFollow stateFollow = _storyFollowService.GetByStoryID(user_id,story_id);
                if (stateFollow != null)
                {
                    if (stateFollow.state == 1)
                        return 1;
                    else
                        return 0;
                }
            }
            return 0;
        }
        public int CreateComment(AddCommentViewModel commentObj)
        {
            try
            {
                Comment comment = Mapper.Map<Comment>(commentObj);
                comment.time = DateTime.Now;
                _commentService.Insert(comment);
                 return comment.comment_id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 0;
            }
        }
        public void DeleteComment(int id_cmt)
        {
            _commentService.Delete(id_cmt);
            //Response.Write("<script>alert('Data inserted fail')</script>");
        }
        public JsonResult MoreCommentReply(int comment_id)
        {
            List<Comment> listCommentChil = _commentService.GetCommentChildren(comment_id);
            List<CommentViewModel> listCommentChilDTO = Mapper.Map<List<CommentViewModel>>(listCommentChil);
            foreach (var item in listCommentChilDTO)
            {
                item.avatar = _accountService.GetUserById(item.user_id).avatar;
            }
            return Json(listCommentChilDTO, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDataVolume(int story_id)
        {
            Story story = _storyService.GetById(story_id);
            if (story == null)
                return Json("", JsonRequestBehavior.AllowGet);
            List<Volume> listVolume = _volumeService.GetVolumesByStoryId(story_id).OrderBy(h => h.ordinalNumber).ToList();
            List<Chapter> listChapter = _chapterService.GetChaptersByStoryId(story_id).OrderBy(h => h.ordinalNumber).ToList();
            var listChapterDTO = Mapper.Map<List<ChapterViewModel>>(listChapter);
            foreach (var item in listChapterDTO)
            {
                item.link_chapter = DataConverter.ConvertLinkChapter(story_id, story.title, item.chapter_id,item.name);
            }
            List<VolumeViewModel> list_vol = Mapper.Map<List<VolumeViewModel>>(listVolume);
            foreach (var item in list_vol)
            {
                item.list_chap = listChapterDTO.Where(h => h.volume_id == item.volume_id).ToList();
            }
            return Json(list_vol, JsonRequestBehavior.AllowGet);
        }
      
        public ActionResult Detail(int? id, int? comment_id)
        {        
            Story story = _storyService.GetById(id);
            if (story == null)            
                return new HttpStatusCodeResult(404);
            int story_id = story.story_id;
            var storyDTO = Mapper.Map<StoryDetailViewModel>(story);
            story.artist_id = story.artist_id != null ? story.artist_id : 0;
            AuthorArtistViewModel author = _listAuthor.Where(h => h.id == story.author_id).FirstOrDefault();
            AuthorArtistViewModel artist = _listArtist.Where(h => h.id == story.artist_id).FirstOrDefault();
            storyDTO.author_name = author != null ? author.name : "Chưa cập nhật";
            storyDTO.artist_name = artist != null ? artist.name : "Chưa cập nhật";
            ViewBag.story = storyDTO;
            TempData["story"] = story;
            List<Volume> listVolume = _volumeService.GetVolumesByStoryId(story_id).OrderBy(h => h.ordinalNumber).ToList();
            List<Chapter> listChapter = _chapterService.GetChaptersByStoryId(story_id).OrderBy(h => h.ordinalNumber).ToList();
            var listChapterDTO = Mapper.Map<List<ChapterViewModel>>(listChapter);
            foreach (var item in listChapterDTO)
            {
                item.link_chapter = DataConverter.ConvertLinkChapter(story.story_id, story.title, item.chapter_id, item.name);
            }
            List<VolumeViewModel> list_vol = Mapper.Map<List<VolumeViewModel>>(listVolume);
            foreach (var item in list_vol)
            {
                item.list_chap = listChapterDTO.Where(h => h.volume_id == item.volume_id).ToList();
            }
            var listGenre = _genreService.GetGenreByStoryId(story_id);
            var listGenreDTO = Mapper.Map<List<GenreViewModel>>(listGenre);
            ViewBag.ListGenres = listGenreDTO;
            ViewBag.ModelTrans = GetInfoTrans(story.translation_id);
            ViewBag.stateFollow = CheckStatusFollow(story_id);
            var listComent = _commentService.GetCommentByStoryId(story_id);
            List<CommentViewModel> listCommentDTO = Mapper.Map<List<CommentViewModel>>(listComent);
            List<CommentViewModel> listCommentParent = listCommentDTO.Where(h => h.id_parent == 0).ToList();
            foreach (var item in listCommentParent)
            {
                item.avatar = _accountService.GetUserById(item.user_id).avatar;
                item.countChil = listCommentDTO.Where(h => h.id_parent == item.comment_id).ToList().Count;
            }
            if(comment_id != 0)
                ViewBag.CommentNoitfy = listCommentDTO.Where(h => h.comment_id == comment_id).FirstOrDefault();
            ViewBag.listParent = listCommentParent;
            return View(list_vol);
        }
        [ChildActionOnly]   
        public ActionResult ListStoryOfAuthorTrans()
        {
            Story story = TempData["story"] as Story;
            int author_id = _listAuthor.Where(h => h.id == story.author_id).Select(h => h.id).FirstOrDefault();
            List<Story> listStoryofTrans = _storyService.GetStoryByTransId(story.translation_id).Where(h => h.story_id != story.story_id).Take(5).ToList();
            ViewBag.listStoryofTrans = Mapper.Map<List<StoryIndexViewModel>>(listStoryofTrans);
            List<Story> listStoryofAuthor = _storyService.GetStoryByAuthorId(author_id).Where(h => h.story_id != story.story_id).Take(5).ToList();
            ViewBag.listStoryofAuthor = Mapper.Map<List<StoryIndexViewModel>>(listStoryofAuthor);
            return View();
        }
        public void AddNotifyCmt(ApplicationUser user, AddCommentViewModel commentObj, int comment_id)
        {
            Notify notify = new Notify();
            notify.user_id = commentObj.receiver_id;
            notify.sender_id = user.Id;
            notify.senderAvatar = user.avatar;
            notify.state = 0;
            notify.time = DateTime.Now;
            notify.content = user.name + " đã nhắc đến bạn trong một bình luận ở truyện " + commentObj.story_name;
            notify.url_notify = commentObj.link_post + "?comment_id=" + comment_id;
            _notifyService.Insert(notify);
        }
        
        public ActionResult DetailChapter(int? id_story, int? id)
        {
            if (id == null)
                return RedirectToAction("Error404", "Error");
            if (id_story == null)
                return RedirectToAction("Error404", "Error");
            Story story  = _storyService.GetById(id_story);
            ViewBag.story = story;
            List<Chapter> chapters = _chapterService.GetChaptersByStoryId((int)id_story);
            Chapter chapter = chapters.Where(h => h.chapter_id == id).FirstOrDefault();
            if (chapter == null)
                return RedirectToAction("Error404", "Error");
            int stt = chapter.ordinalNumber;
            Chapter chapter_next = chapters.Where(h => h.ordinalNumber == stt + 1).FirstOrDefault();
            Chapter chapter_prev = chapters.Where(h => h.ordinalNumber == stt - 1).FirstOrDefault();
            if (chapter_next != null)
                ViewBag.chapter_next = DataConverter.ConvertLinkChapter(story.story_id, story.title, chapter_next.chapter_id, chapter_next.name);
           if(chapter_prev != null)
                ViewBag.chapter_prev = DataConverter.ConvertLinkChapter(story.story_id, story.title, chapter_prev.chapter_id, chapter_prev.name);
            var chapterDTO = Mapper.Map<ChapterDetailViewModel>(chapter);
            ViewBag.chapter = chapterDTO;
            List<ImageChapter> imageChapters = _chapterService.GetImageByChapterId((int)id);
            var imageChaptersDTO = Mapper.Map<List<ImageViewModel>>(imageChapters);
            return View(imageChaptersDTO);
        }
        public ActionResult Search_Ajax(string key)
        {
            string content = "";
            List<Story> listStory = _storyService.GetStoryByName(key);
            var listStoryDTO = Mapper.Map<List<SearchStoryViewModel>>(listStory);
            foreach (var item in listStoryDTO)
            {
                if (item.newChapter_id == null)
                {
                    item.chapter_name = "Chưa có chapter nào";
                }
                var listGenres = _genreService.GetGenreByStoryId(item.story_id);
                foreach (var item1 in listGenres)
                {
                    item.listGenres += item1.name + ", ";
                }
                content += " <li class=\"suggestsearch__item \">" +
                               "<a class=\"suggestsearch__content\" href=\"/truyen/" + item.link_story + "\">" +
                                    "<img src = \"/Content/imageIllustration/" + item.cover_image + "\">" +
                                    "<h3>" + item.title + "</h3>" +
                                    "<h4>" +
                                        "<i>" + item.chapter_name + "</i>" +
                                        "<i>" + item.listGenres + "</i>" +
                                    "</h4>" +
                                "</a>" +
                            "</li>";
            }
            return Content(content, "text/html");
        }

        public ActionResult FollowStory(int story_id)
        {
            string content = "";
            if (!User.Identity.IsAuthenticated)
                return Content("Bạn phải đăng nhập để theo dõi truyện", "text/html");
            else
            {
                string user_id = User.Identity.GetUserId();
                StoryFollow sf = _storyFollowService.GetByStoryID(user_id, story_id);
                if (sf == null)
                {
                    StoryFollow item = new StoryFollow();
                    item.story_id = story_id;
                    item.user_id = user_id;
                    item.total_unread = 0;
                    item.state = 1;
                    try
                    {
                        _storyFollowService.Insert(item);
                        return Content("Bạn đã theo dõi truyện này", "text/html");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        return Content("Đã có lỗi xảy ra", "text/html");
                    }
                }
                else
                {
                    if (sf.state == 1)
                    {
                        sf.state = 0;
                        content = "Bạn đã bỏ theo dõi truyện này";                      
                    }
                    else
                    {
                        sf.state = 1;
                        content = "Bạn đã theo dõi truyện này";                      
                    }
                    try
                    {
                        _storyFollowService.Update(sf);
                        return Content(content, "text/html");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        return Content("Đã có lỗi xảy ra", "text/html");
                    }
                }
            }
        }

        public ActionResult Category(string category_name, string sort = "update", int? page = 1)
        {
            category_name = category_name.Replace("-", " ");
            Genre genre = _genreService.GetGenreByName(category_name);
            ViewBag.Genres = genre;
            List<Story> listStory =_storyService.GetStoryByGenre(genre.genres_id);
            ViewBag.countPageCat = listStory.Count / limitStory + 1;
            int pageNumber = (page ?? 1);
            ViewBag.pageCat = pageNumber;
            if (sort == "update")
            {
                listStory = listStory.OrderByDescending(h => h.timeUpdate).Skip(limitStory * (pageNumber - 1)).Take(limitStory).ToList();
                ViewData["sortCat"] = "Mới cập nhật";
            }
            else if (sort == "topFollow")
            {
                listStory = listStory.OrderByDescending(h => h.follow_views).Skip(limitStory * (pageNumber - 1)).Take(limitStory).ToList();
                ViewData["sortCat"] = "Theo dõi";
            }
            else if (sort == "topView")
            {
                listStory = listStory.OrderByDescending(h => h.total_views).Skip(limitStory * (pageNumber - 1)).Take(limitStory).ToList();
                ViewData["sortCat"] = "Top toàn thời gian";
            }
            else if (sort == "topTotalWord")
            {
                listStory = listStory.OrderByDescending(h => h.total_word).Skip(limitStory * (pageNumber - 1)).Take(limitStory).ToList();
                ViewData["sortCat"] = "Số từ";
            }
            var listStoryDTO = Mapper.Map<List<StoryIndexViewModel>>(listStory);
            return View(listStoryDTO);
        }
        public ActionResult ListStory(string sort, int? page)
        {
            int countStory = _storyService.GetAll().Count;
            ViewBag.countPage = countStory / limitStory + 1;
            int pageNumber = (page ?? 1);
            ViewBag.page = pageNumber;
            List<Story> listStory = new List<Story>();
            if (sort == "update")
            {
                listStory = _storyService.GetAll().OrderByDescending(h => h.timeUpdate).Skip(limitStory * (pageNumber - 1)).Take(limitStory).ToList();
                ViewData["sort"] = "Mới cập nhật";
            }
            else if (sort == "topFollow")
            {
                listStory = _storyService.GetAll().OrderByDescending(h => h.follow_views).Skip(limitStory * (pageNumber - 1)).Take(limitStory).ToList();
                ViewData["sort"] = "Theo dõi";
            }
            else if (sort == "topView")
            {
                listStory = _storyService.GetAll().OrderByDescending(h => h.total_views).Skip(limitStory * (pageNumber - 1)).Take(limitStory).ToList();
                ViewData["sort"] = "Top toàn thời gian";
            }
            else if (sort == "topTotalWord")
            {
                listStory = _storyService.GetAll().OrderByDescending(h => h.total_word).Skip(limitStory * (pageNumber - 1)).Take(limitStory).ToList();
                ViewData["sort"] = "Số từ";
            }
            var listStoryDTO = Mapper.Map<List<StoryIndexViewModel>>(listStory);
            return View(listStoryDTO);
        }
        [ChildActionOnly]
        [OutputCache(Duration = 60 * 60 * 24 * 7)]
        public ActionResult ListGenres()
        {
            return PartialView("ListGenres", _genreService.GetAllGenre());
        }
        public ActionResult ListManga(string sort, int? page)
        {
            int countStory = _storyService.GetAll().Count;
            int limitStory = 16;
            ViewBag.countPage = countStory / limitStory + 1;
            int pageNumber = (page ?? 1);
            ViewBag.page = pageNumber;
            List<Story> listStory = new List<Story>();
            if (sort == "update")
            {
                listStory = _storyService.GetAll().OrderByDescending(h => h.timeUpdate).Skip(limitStory * (pageNumber - 1)).Take(limitStory).ToList();
                ViewData["sort"] = "Mới cập nhật";
            }
            else if (sort == "topFollow")
            {
                listStory = _storyService.GetAll().OrderByDescending(h => h.follow_views).Skip(limitStory * (pageNumber - 1)).Take(limitStory).ToList();
                ViewData["sort"] = "Theo dõi";
            }
            else if (sort == "topView")
            {
                listStory = _storyService.GetAll().OrderByDescending(h => h.total_views).Skip(limitStory * (pageNumber - 1)).Take(limitStory).ToList();
                ViewData["sort"] = "Top toàn thời gian";
            }
            else if (sort == "topTotalWord")
            {
                listStory = _storyService.GetAll().OrderByDescending(h => h.total_word).Skip(limitStory * (pageNumber - 1)).Take(limitStory).ToList();
                ViewData["sort"] = "Số từ";
            }
            return View(listStory);
        }
       
        public ActionResult AddComment(string jsonComment)
        {
            AddCommentViewModel commentObj = JsonConvert.DeserializeObject<AddCommentViewModel>(jsonComment);
            int comment_id = CreateComment(commentObj);
            commentObj.link_post = "/truyen/"+ commentObj.story_id + "/" + commentObj.story_name.Trim().Replace(" ", "-").ToLower();
            if (commentObj.id_parent == 0 && comment_id != 0)
            {
                string _content = "<div data-id-parent=\"" + comment_id + "\" id = \"" + comment_id + "\" class=\"ln-comment-group\">" +
                                        "<div class= \"ln-comment-item\" >" +
                                            "<div class=\"ln-comment-user-avt\">" +
                                            "<img src = " + "\"/avatar/" + commentObj.avatar + "\"" + "alt=\"Avatar\" />" +
                                            "</div>" +
                                            "<div class=\"ln-comment-info\">" +
                                                "<div class=\"ln-comment-wrapper\">" +
                                                    "<div class=\"ln-comment-user-name\">" +
                                                    "<a>" + commentObj.user_name + "</a>" +
                                                    "</div>" +
                                                    "<div contenteditable=\"false\" class=\"ln-comment-text\">" + commentObj.content +
                                                    "</div>" +
                                                    "<div class=\"visible-toolkit\">" +
                                                        "<span class=\"ln-comment-time\"><a style = \"color:#9191c5\">1 phút</a>" +
                                                        "</span>" +
                                                        "<a data-user_id=\"" + commentObj.user_id + "\" name=\"" + commentObj.user_name + "\"class=\"ln-comment-text-reply\">Trả lời</a>" +
                                                     "</div>" +
                                            "</div>" +
                                            "<div class=\"ln-comment-menu\">" +
                                                    "<div class=\"ln-comment-icon\">" +
                                                        "<i class=\"fas fa-angle-down\"></i>" +
                                                    "</div>" +
                                                    "<div data-id-comment= \"" + comment_id + "\" data-user_id=\"" + commentObj.user_id + "\" class=\"ln-comment-edit\">" +
                                                        "<span class=\"ln-comment-edit-span\">" +
                                                            "<i class=\"fas fa-edit\"> </i>" +
                                                            "Chỉnh sửa" +
                                                        "</span>" +
                                                        "<span class=\"ln-comment-delete\">" +
                                                            "<i class=\"fas fa-times\"> </i>" +
                                                            "Xóa" +
                                                        "</span>" +
                                                    "</div>" +
                                            "</div>" +
                                         "</div>" +
                                     "</div>";
                return Content(_content, "text/html");
            }
            else if(commentObj.id_parent != 0 && comment_id != 0)
            {
                //string user_name = commentObj.content.Split(':')[0].Replace("@", "").Trim();
                ApplicationUser user = _accountService.GetUserById(commentObj.user_id);
                if (user != null && commentObj.user_id != commentObj.receiver_id)
                    AddNotifyCmt(user, commentObj, comment_id);
                string _content = "<div data-id-parent=\"" + commentObj.id_parent + "\" id=\"" + comment_id + "\" class=\"ln-comment-reply\">" +
                                        "<div class= \"ln-comment-item\" >" +
                                            "<div class=\"ln-comment-user-avt\">" +
                                            "<img src = \"/avatar/" + commentObj.avatar + "\"" + "alt=\"Avatar\" />" +
                                            "</div>" +
                                            "<div class=\"ln-comment-info\">" +
                                                "<div class=\"ln-comment-wrapper\">" +
                                                    "<div class=\"ln-comment-user-name\">" +
                                                    "<a>" + commentObj.user_name + "</a>" +
                                                    "</div>" +
                                                    "<div class=\"ln-comment-text\">" +
                                                        commentObj.content +
                                                    "</div>" +
                                                "</div>" +
                                                "<div class=\"visible-toolkit\">" +
                                                    "<span class=\"ln-comment-time\">" +
                                                        "<a style = \"color:#9191c5\">1 phút</a>" +
                                                    "</span>" +
                                                    "<a data-user_id=\""+ commentObj.user_id+"\" name =\"" + commentObj.user_name + "\" class=\"ln-comment-text-reply\">Trả lời</a>" +
                                                "</div>" +
                                                 "<div class=\"ln-comment-menu\">" +
                                                    "<div class=\"ln-comment-icon\">" +
                                                        "<i class=\"fas fa-angle-down\"></i>" +
                                                    "</div>" +
                                                    "<div data-id-comment=\"" + comment_id + "\"  data-user_id = \"" + commentObj.user_id + "\" class=\"ln-comment-edit\">" +
                                                        "<span class=\"ln-comment-edit-span\">" +
                                                            "<i class=\"fas fa-edit\"> </i>" +
                                                            "Chỉnh sửa" +
                                                        "</span>" +
                                                        "<span class=\"ln-comment-delete\">" +
                                                            "<i class=\"fas fa-times\"> </i>" +
                                                            "Xóa" +
                                                        "</span>" +
                                                    "</div>" +
                                                "</div>" +
                                           "</div >" +
                                         "</div >" +
                                     "</div >";
                return Content(_content, "text/html");
            }      
         return Content("Đã có lỗi xảy ra", "text/html");
    }

       
}
}