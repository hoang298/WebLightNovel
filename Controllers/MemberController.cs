using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLightNovel.Extensions;
using WebLightNovel.Models.Account;
using WebLightNovel.Models.Entity;
using WebLightNovel.Models.Member;
using WebLightNovel.Models.ModelStory;
using WebLightNovel.Service.Stories;
using WebLightNovel.Service.Users;

namespace WebLightNovel.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {

        Connect_sql db = new Connect_sql();
        private readonly IStoryService _storyService;
        private readonly IAccountService _accountService;
        private readonly INotifyService _notifyService;
        private readonly ILetterService _letterService;
        private readonly IStoryFollowService _storyFollowService;
       public MemberController(IAccountService accountService,
           INotifyService notifyService,
           IStoryService storyService)
        {
            _storyService = storyService;
            _accountService = accountService;
            _notifyService = notifyService;
            _letterService = new LetterService();
            _storyFollowService = new StoryFollowService();
            
        }
        // GET: Member

        public ActionResult Follow()
        {
            string user_id = User.Identity.GetUserId();
            var list_StoryFollow = _storyFollowService.GetAllByUserId(user_id);
            List<int> listId = list_StoryFollow.Select(g => g.story_id).ToList();
            List<Story> stories = _storyService.GetAll().Where(h => listId.Contains(h.story_id)).ToList();
            List<mStoryFollow> list_story = Mapper.Map<List<mStoryFollow>>(stories);
            foreach (var item in list_story)
            {
                item.total_unread = list_StoryFollow.Where(h => h.story_id == item.story_id).Select(h => h.total_unread).FirstOrDefault();
            }
            return View(list_story);
        }
        public ActionResult Notify()
        {
            string user_id = User.Identity.GetUserId();
            List<Notify> listNotify = _notifyService.GetAllByUserId(user_id);
            return View(listNotify);

        }
        public ActionResult Messenger()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Messenger(string type)
        {
            List<ModelMessenger> list_letter = new List<ModelMessenger>();
            string user_id = User.Identity.GetUserId();
            var listLetter = _letterService.GetAllByUserId(user_id);
            if (type == "send")
            {
                List<Letter> list_sender = listLetter.Where(h => h.sender_id == user_id).ToList();
                var list = from item in list_sender
                           select new ModelMessenger
                           {
                               sender_id = item.sender_id,
                               receiver_id = item.receiver_id,
                               title = item.title,
                               content = item.content,
                               time = DataConverter.ConvertTimeLetter(item.time),
                               state = item.state,
                               sender_name = User.Identity.GetUserName(),
                               avatar = User.Identity.GetUserAvatar(),
                               letter_id = item.letter_id
                           };
                list_letter = list.ToList();
            }
            else
            {
                List<Letter> list_rec = listLetter.Where(h => h.receiver_id == user_id).ToList();
                List<string> list_sender_id = list_rec.Select(h => h.sender_id).ToList();
                List<ApplicationUser> list_user_senderDb = _accountService.GetListUserById(list_sender_id);
                List<UserModel> list_user_sender = list_user_senderDb.Select(h => new UserModel { user_id = h.Id, name = h.name, avatar = h.avatar }).ToList();
                var list = from item in list_rec
                           from item1 in list_user_sender
                           where item.sender_id == item1.user_id
                           select new ModelMessenger
                           {
                               sender_id = item.sender_id,
                               receiver_id = item.receiver_id,
                               title = item.title,
                               content = item.content,
                               time = DataConverter.ConvertTimeLetter(item.time),
                               state = item.state,
                               sender_name = item1.name,
                               avatar = item1.avatar,
                               letter_id = item.letter_id
                           };
                list_letter = list.ToList();
            }
            return View(list_letter);
        }

        public ActionResult ConfirmRead(int story_id)
        {

            string user_id = User.Identity.GetUserId();
            StoryFollow item = _storyFollowService.GetByStoryID(user_id, story_id);
            if (_storyFollowService.UpdateStoryFollow(item))
                return Content("true", "text/html");
            return Content("Đã có lỗi xảy ra vui lòng thử lại!", "text/html");
        }
        public ActionResult ConfirmNotify()
        {
            string user_id = User.Identity.GetUserId();
            List<Notify> listNotify = _notifyService.GetAllByUserId(user_id);          
            if (_notifyService.ConfirmReadNotify(listNotify))
                return Content("true", "text/html");
            return Content("false", "text/html");
        }
        [HttpPost]
        public ActionResult SendMessenger(Letter sp, string userNameTo)
        {

            string user_id = User.Identity.GetUserId();
            var userRec = _accountService.GetUserByName(userNameTo);
            if (userRec == null)
            {
                TempData["ErrorNotFoundSendMessenger"] = true;
                return Redirect(TempData["urlMess"].ToString());
            }
            sp.sender_id = user_id;
            sp.receiver_id = userRec.Id;
            sp.content = sp.content.Replace("#LineBreak", "@LineBreak").Trim();
            string[] content = sp.content.Split(new string[] { "@LineBreak" }, StringSplitOptions.None);
            sp.time = DateTime.Now;
            sp.state = 0;
            if (ModelState.IsValid)
            {
                try
                {
                    _letterService.Insert(sp);
                    TempData["SendMessenger"] = true;
                    return Redirect(TempData["urlMess"].ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }             
            }
            TempData["ErrorSendMessenger"] = true;
            return Redirect(TempData["urlMess"].ToString());
        }
        public void DeleteLetter(string jsonArrIdLetter)
        {
            string user_id = User.Identity.GetUserId();
            int[] arrIdLetter = JsonConvert.DeserializeObject<int[]>(jsonArrIdLetter);
            try
            {
                _letterService.Delete(arrIdLetter, user_id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }
        [HttpPost]
        public ActionResult ChangeAvatar(HttpPostedFileBase file)
        {
            string user_id = User.Identity.GetUserId();
            if (file != null && file.ContentLength > 0)
            {
                ApplicationUser item = _accountService.GetUserById(user_id);
                string _FileName = Path.GetFileName(file.FileName);
                string _path = Path.Combine(Server.MapPath("~/avatar"), _FileName);
                file.SaveAs(_path);
               if(_accountService.UpdateAvatar(user_id, _FileName))
                {
                    TempData["ChangeSucces"] = true;
                    Session["user"] = item;
                    return RedirectToAction("Infor", "Account", new { id = user_id });
                }             
            }
            TempData["ErrorChangeSucces"] = true;
            return RedirectToAction("Infor", "Account", new { id = user_id });
        }


    }
}