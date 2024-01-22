using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;
using WebLightNovel.Extensions;
using WebLightNovel.Models.Account;
using WebLightNovel.Models.Entity;
using WebLightNovel.Models.Member;
using WebLightNovel.Models.SystemModel;
using WebLightNovel.Service.Common;
using WebLightNovel.Service.Stories;
using WebLightNovel.Service.System;
using WebLightNovel.Service.Users;
using static WebLightNovel.Models.Enum.EnumModel;

namespace WebLightNovel.Controllers
{
    [Authorize]
    public class SystemController : Controller
    {
        // GET: System
        private readonly AuthenticationService _authenticationService;
        private readonly IStoryService _storyService;
        private readonly IStoryFollowService _storyFollowService;
        private readonly IVolumeService _volumeService;
        private readonly IChapterService _chapterService;
        private readonly IGenreService _genreService;
        private readonly ITranslationTeamService _translationTeamService;
        private readonly PostStoryService _postStoryService;
        private readonly JoinRequestService _requestService;
        private readonly IAccountService _accountService;
        private readonly INotifyService _notifyService;
        private readonly ImageService _imageService;
        List<UserRole> listRole = new List<UserRole>();
        public SystemController(
            IGenreService genreService,
            IAccountService accountService,
            IStoryService storyService,
            INotifyService notifyService
            )
        {
            _storyService = storyService;
            _genreService = genreService;
            _accountService = accountService;
            _notifyService = notifyService;
            _authenticationService = new AuthenticationService();
            _volumeService = new VolumeService();
            _chapterService = new ChapterService();
            _translationTeamService = new TranslationTeamService();
            _postStoryService = new PostStoryService();
            _requestService = new JoinRequestService();
            _imageService = new ImageService();
            _storyFollowService = new StoryFollowService();
        }

        public int checkRole(int story_id)
        {
            UserRole userrole = listRole.Where(h => h.story_id == story_id).FirstOrDefault();
            if (userrole != null)
                return userrole.role;
            return -1;
        }
        public int checkRoleTrans(int trans_id)
        {
            UserRole userRole = listRole.Where(h => h.translation_id == trans_id).FirstOrDefault();
            if (userRole != null)
                return userRole.role;
            return -1;
        }

        public ActionResult Index()
        {
            listRole = _authenticationService.GetRole(User.Identity.GetUserId());
            List<int> cachedDataStory = CacheManager.Instance.GetCache()["cachedDataStory"] as List<int>;
            if (cachedDataStory == null)
            {
                cachedDataStory = new List<int>();
                int totalStory = _storyService.GetCount();
                int totalVolume = _volumeService.GetCount();
                int totalChapter = _chapterService.GetCount();
                cachedDataStory.Add(totalStory);
                cachedDataStory.Add(totalVolume);
                cachedDataStory.Add(totalChapter);
                CacheManager.Instance.SetCache("cachedDataStory", cachedDataStory, DateTimeOffset.Now.AddDays(1));
            }
            ViewBag.totalStory = cachedDataStory[0];
            ViewBag.totalVolume = cachedDataStory[1];
            ViewBag.totalChapter = cachedDataStory[2];
            return View();
        }



        public bool IsImageFile(HttpPostedFileBase file)
        {
            // Kiểm tra định dạng tệp
            string fileExtension = Path.GetExtension(file.FileName);
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" }; // Các định dạng ảnh cho phép
            bool isImage = allowedExtensions.Contains(fileExtension, StringComparer.OrdinalIgnoreCase);

            // Kiểm tra kiểu MIME của tệp
            string[] allowedMimeTypes = { "image/jpeg", "image/png", "image/gif" }; // Các kiểu MIME cho phép
            bool isValidMimeType = allowedMimeTypes.Contains(file.ContentType);
            if (isImage && isValidMimeType)
            {
                var memoryStream = new MemoryStream();
                file.InputStream.CopyTo(memoryStream);
                byte[] bytes = memoryStream.ToArray();
                byte[] png = new byte[] { 137, 80, 78, 71 };
                byte[] jpeg = new byte[] { 255, 216, 255, 224 };
                byte[] jpg = new byte[] { 255, 216, 255, 225 };
                byte[] gif = new byte[] { 71, 73, 70, 56 };
                byte[] tiff = new byte[] { 73, 73, 42 };
                byte[] bmp = new byte[] { 66, 77 };
                switch (fileExtension)
                {
                    case ".png":
                        return png.SequenceEqual(bytes.Take(png.Length).ToArray());
                    case ".jpg":
                        return jpg.SequenceEqual(bytes.Take(jpg.Length).ToArray());
                    case ".jpeg":
                        return jpeg.SequenceEqual(bytes.Take(jpeg.Length).ToArray());
                    case ".gif":
                        return gif.SequenceEqual(bytes.Take(gif.Length).ToArray());
                    case ".tiff":
                        return tiff.SequenceEqual(bytes.Take(tiff.Length).ToArray());
                    case ".bmp":
                        return bmp.SequenceEqual(bytes.Take(bmp.Length).ToArray());
                    default:
                        break;
                }
            }
            return false;
        }
        public ActionResult Group()
        {
            string user_id = User.Identity.GetUserId();
            List<UserRole> userRoles = _authenticationService.GetRole(user_id);
            List<int> listTrans_id = userRoles.Select(p => p.translation_id).Distinct().ToList();
            List<TranslationTeam> listTrans = _translationTeamService.GetByListTransId(listTrans_id);
            List<int> listTrans_idOfme = userRoles.Where(h => h.role == 1).Select(h => h.translation_id).ToList();
            List<int> listTrans_idJoin = userRoles.Where(h => h.role == 0).Select(h => h.translation_id).ToList();
            List<GroupViewModel> listTransOfMe = Mapper.Map<List<GroupViewModel>>(listTrans.Where(h => listTrans_idOfme.Contains(h.translation_id)).ToList());
            List<GroupViewModel> listTransJoin = Mapper.Map<List<GroupViewModel>>(listTrans.Where(h => listTrans_idJoin.Contains(h.translation_id)).ToList());
            ViewBag.trans_Ofme = listTransOfMe;
            ViewBag.trans_Join = listTransJoin;
            return View();
        }
        public ActionResult CreateStory()
        {
            string user_id = User.Identity.GetUserId();
            List<UserRole> userRoles = _authenticationService.GetRole(user_id);
            List<int> trans_ids = userRoles.Select(g => g.translation_id).Distinct().ToList();
            if (trans_ids.Count == 0)
            {
                ViewBag.ErrorNotTrans = true;
                return View();
            }
            List<CreateStoryTransViewModel> listTrans = _translationTeamService.GetByListTransId(trans_ids)
            .Select(h => new CreateStoryTransViewModel { translation_id = h.translation_id, name = h.name }).ToList();
            if (listTrans.Count > 0)
                ViewBag.listTrans = listTrans;
            CreateStoryViewModel createStoryViewModel = new CreateStoryViewModel();
            createStoryViewModel.listTrans = listTrans;
            createStoryViewModel.listGenres = _genreService.GetAllGenre();
            return View(createStoryViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateStory([Bind(Include = "title, author_name, artist_name,type_story,trans_id,genres,synopsis,translationSource")] PostStory ps, HttpPostedFileBase file)
        {
            //[Bind(Include)] ngăn chặn người dùng tự gán các giá trị cho các thuộc tính ko có trên form ví dụ như user_id... và truyền vào biến ps
            string user_id = User.Identity.GetUserId();

            if (file != null && file.ContentLength > 0 && ModelState.IsValid)
            {
                ps.user_id = user_id;
                string _FileName = Path.GetFileName(file.FileName);
                if (IsImageFile(file))
                {
                    string _path = Path.Combine(Server.MapPath("~/avatar"), _FileName);
                    file.SaveAs(_path);
                    ps.image = _FileName;
                    try
                    {
                        _postStoryService.Insert(ps);
                        TempData["CreateStory"] = true;
                    }
                    catch (Exception ex)
                    {
                        TempData["ErrorCreateStory"] = true;
                        Console.WriteLine(ex);
                    }
                }
                TempData["ErrorImage"] = true;
            }


            return RedirectToAction("CreateStory");

        }
        public ActionResult SearchGroup()
        {
            return View();
        }
        [HttpGet]
        public ActionResult SearchGroup(string key)
        {
            string user_id = User.Identity.GetUserId();

            List<TranslationTeam> list_trans = _translationTeamService.GetByName(key);
            List<SearchGroupViewModel> list_transViewModel = new List<SearchGroupViewModel>();
            List<JoinRequest> listReqByUser = _requestService.GetByUserId(user_id);
            List<TranslationTeam> listTransByUser = list_trans.Where(h => h.user_id == user_id).ToList();
            foreach (var item in list_trans)
            {
                SearchGroupViewModel it = new SearchGroupViewModel();
                it.translation_id = item.translation_id;
                it.name = item.name;
                if (listReqByUser.Where(h => h.trans_id == item.translation_id).ToList().Count > 0)
                    it.state = statusReq.waitting;
                else if (listTransByUser.Where(h => h.translation_id == item.translation_id).ToList().Count > 0)
                    it.state = statusReq.succes;
                else
                    it.state = statusReq.not;
                it.date_create = item.dateCreated.ToString("dd/MM/yyyy");
                it.user_name = _accountService.GetUserById(item.user_id).name;
                list_transViewModel.Add(it);
            }
            return View(list_transViewModel);
        }
        public ActionResult InforGroup(int trans_id)
        {
            string user_id = User.Identity.GetUserId();

            List<UserRole> userRoles = _authenticationService.GetRole(user_id);
            List<int> listTrans_id = userRoles.Select(h => h.translation_id).Distinct().ToList();
            if (!listTrans_id.Contains(trans_id))
                return RedirectToAction("Error403", "Error");
            List<TranslationUser> listTransUs = _translationTeamService.GetMemberByTransId(trans_id);
            List<MemberTranslation> listMember = new List<MemberTranslation>();
            ViewBag.Trans_id = trans_id;
            foreach (var item in listTransUs)
            {
                MemberTranslation it = new MemberTranslation();
                it.name = _accountService.GetUserById(item.user_id).name;
                it.role = item.role == 1 ? "Trưởng nhóm" : "Thành viên";
                it.user_id = item.user_id;
                it.dateJoin = item.dateJoin;
                listMember.Add(it);
            }
            ViewBag.isCaptain = listTransUs.Where(h => h.role == 1).Select(h => h.user_id).FirstOrDefault() == user_id;
            //var listStories = db.Stories.Where(h => h.translation_id == trans_id).Select(h => new InfoGroupStoryViewModel { story_id = h.story_id, title = h.title, image = h.cover_image, state = (statusStory)h.state });
            List<Story> listStories = _storyService.GetStoryByTransId(trans_id);
            var listStoriesDTO = Mapper.Map<List<InfoGroupStoryViewModel>>(listStories);
            List<ReqJoin> listUser = _requestService.GetByTransId(trans_id).Select(h => new ReqJoin { date_req = h.time_req, user_id = h.sender_id }).ToList();
            foreach (var item in listUser)
            {
                item.user_name = _accountService.GetUserById(item.user_id).name;
            }
            InfoGroupViewModel inforGroup = new InfoGroupViewModel();
            try
            {
                inforGroup.ListStories = listStoriesDTO;
                inforGroup.MemberTranslation = listMember;
                inforGroup.ListReqJoin = listUser;
                return View(inforGroup);
            }
            catch (Exception)
            {

                return View();
            }

        }

        public string AddReqJoin(int trans_id = 0)
        {
            string user_id = User.Identity.GetUserId();

            if (trans_id != 0)
            {
                JoinRequest item = new JoinRequest();
                item.trans_id = trans_id;
                item.sender_id = user_id;
                item.time_req = DateTime.Now;
                item.state = 0;
                try
                {
                    _requestService.Insert(item);
                    return "true";
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return "Đã có lỗi xảy ra vui lòng thử lại";
        }

        public string AddMemberToTransTeam(int trans_id = 0, string user_id = "")
        {
            if (checkRoleTrans(trans_id) != 1)
                return "Bạn không được phép thực hiện chức năng này";
            else
            {
                string trans_name = _translationTeamService.GetById(trans_id).name;
                TranslationUser item = new TranslationUser();
                item.translation_id = trans_id;
                item.user_id = user_id;
                item.role = 0;
                item.dateJoin = DateTime.Now;
                try
                {
                    _translationTeamService.InsertMember(item);
                    RemoveReq(trans_id, user_id);
                    Notify notify = new Notify();
                    notify.content = User.Identity.GetUserNickName() + " đã thêm bạn vào nhóm dịch " + trans_name;
                    notify.time = DateTime.Now;
                    notify.user_id = user_id;
                    notify.url_notify = "/System/Group";
                    notify.sender_id = User.Identity.GetUserId();
                    _notifyService.Insert(notify);
                    return "true";
                }
                catch (Exception)
                {

                    throw;
                }
            }

        }
        public string RemoveReq(int trans_id, string user_id)
        {
            JoinRequest item = _requestService.GetByTransId(trans_id).Where(h => h.sender_id == user_id).FirstOrDefault();
            if (item != null)
            {
                try
                {
                    _requestService.Delete(item.req_id);
                    return "true";
                }
                catch (Exception)
                {
                    return "Đã có lỗi xảy ra vui lòng thử lại";
                }

            }
            return "Đã có lỗi xảy ra vui lòng thử lại";
        }

        public string RemoveMember(string user_id, int trans_id)
        {
            if (checkRoleTrans(trans_id) != 1)
                return "Bạn không thể thực hiện chức năng này";
            TranslationUser item = _translationTeamService.GetMemberByTransId(trans_id).Where(h => h.user_id == user_id).FirstOrDefault();
            if (item != null)
            {
                try
                {
                    _translationTeamService.DeleteMember(item);
                    return "true";
                }
                catch (Exception)
                {

                    return "Đã có lỗi xảy ra vui lòng thử lại";
                }
            }
            return "Đã có lỗi xảy ra vui lòng thử lại";
        }

        public ActionResult Series(string type, string of)
        {
            string user_id = User.Identity.GetUserId();
            int theLoai = 0;
            int role = 0;
            if (type == "translation" && of == "self")
            {
                theLoai = 1;
                role = 1;
            }
            else if (type == "translation" && of == "shared")
            {
                theLoai = 1;
                role = 0;
            }
            else if (type == "convert" && of == "self")
            {
                theLoai = 2;
                role = 1;
            }
            else if (type == "convert" && of == "shared")
            {
                theLoai = 2;
                role = 0;
            }
            else if (type == "original" && of == "self")
            {
                theLoai = 0;
                role = 1;
            }
            else if (type == "original" && of == "shared")
            {
                theLoai = 0;
                role = 0;
            }
            var listTransUser = _accountService.GetAllTransByUserId(user_id, role, theLoai);
            return View(listTransUser);
        }
        public ActionResult Story(int story_id)
        {
            if (checkRole(story_id) == -1)
                return RedirectToAction("Error403", "Home");
            StorySystemViewModel story = new StorySystemViewModel();
            string story_name = _storyService.GetById(story_id).title;
            List<Volume> listVolume = _volumeService.GetVolumesByStoryId(story_id).OrderBy(h => h.ordinalNumber).ToList();
            var list_vol = Mapper.Map<List<Volume_StorySystem>>(listVolume);
            List<Chapter> listChapter = _chapterService.GetChaptersByStoryId(story_id).OrderBy(h => h.ordinalNumber).ToList();
            var list_chap = Mapper.Map<List<Chapter_StorySystem>>(listChapter);
            story.story_id = story_id;
            story.story_name = story_name;
            story.listVol = list_vol;
            story.listChap = list_chap;
            return View(story);
        }
        public ActionResult CreateTranslation()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateTranslation(string name, string description)
        {
            string user_id = User.Identity.GetUserId();

            if (_translationTeamService.GetByName(name).Where(h => h.name == name).FirstOrDefault() == null)
            {
                TranslationTeam trans = new TranslationTeam();
                trans.name = name;
                trans.user_id = user_id;
                trans.description = description;
                try
                {
                    _translationTeamService.Insert(trans);
                    TempData["CreateTranslation"] = true;
                    return View();
                }
                catch (Exception ex)
                {
                    TempData["ErrorCreateTrans"] = ex.ToString();
                    return View();
                }
            }
            TempData["ErrorCreateTrans"] = "Tên nhóm đã tồn tại";
            return View();
        }
        [HttpGet]
        public ActionResult EditStory(int story_id)
        {
            TempData["story_id"] = story_id;
            if (checkRole(story_id) == -1)
                return RedirectToAction("Error403", "Home");
            return View(_storyService.GetById(story_id));
        }
        [HttpPost]
        public ActionResult EditStory(Story sp, HttpPostedFileBase file)
        {
            int story_id = Convert.ToInt32(TempData["story_id"]);
            Story item = _storyService.GetById(story_id);
            if (file != null && file.ContentLength > 0)
            {
                string _FileName = Path.GetFileName(file.FileName);
                string _path = Path.Combine(Server.MapPath("~/Content/imageIllustration"), _FileName);
                file.SaveAs(_path);
                item.cover_image = _FileName;
            }
            if (ModelState.IsValid)
            {
                item.state = sp.state;
                item.synopsis = sp.synopsis;
                item.title = sp.title;
                item.translation_id = sp.translation_id;
                item.type = sp.type;
                item.artist_id = sp.artist_id;
                item.author_id = sp.author_id;
                try
                {
                    _storyService.Update(item);
                    TempData["EditStory"] = true;
                    return RedirectToAction("EditStory", new { story_id = story_id });
                }
                catch (Exception)
                {

                    throw;
                }
            }
            TempData["ErrorEditStory"] = true;
            return RedirectToAction("EditStory", new { story_id = story_id });
        }
        [HttpGet]
        public ActionResult CreateVolume(int story_id)
        {
            //TempData["story_id"] = story_id;
            if (checkRole(story_id) == -1)
                return RedirectToAction("Error403", "Error");
            var number = _volumeService.GetVolumesByStoryId(story_id).OrderByDescending(h => h.ordinalNumber).FirstOrDefault();
            if (number != null)
                TempData["number"] = number.ordinalNumber + 1;
            return View();
        }
        [HttpPost]
        public ActionResult CreateVolume(Volume sp, HttpPostedFileBase file)
        {
            //int story_id = Convert.ToInt32(TempData["story_id"]);
            if (file != null && file.ContentLength > 0 && ModelState.IsValid)
            {
                string _FileName = Path.GetFileName(file.FileName);
                string _path = Path.Combine(Server.MapPath("~/image_vol"), _FileName);
                file.SaveAs(_path);
                sp.volumeImg = _FileName;
                try
                {
                    _volumeService.Insert(sp);
                    TempData["CreateVolume"] = true;
                    return RedirectToAction("CreateVolume", new { story_id = sp.story_id });
                }
                catch (Exception)
                {
                    throw;
                }        
            }
            TempData["ErrorCreateVolume"] = true;
            return RedirectToAction("CreateVolume", new { story_id = sp.story_id });
        }

        [HttpGet]
        public ActionResult EditVolume(int volume_id)
        {
            Volume volume = _volumeService.GetById(volume_id);
            if (volume == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            if (checkRole(volume.story_id) == -1)
                return RedirectToAction("Error403", "Error");
            TempData["volume_id"] = volume_id;
            return View(volume);
        }
        [HttpPost]
        public ActionResult EditVolume(Volume sp, HttpPostedFileBase file)
        {
            int volume_id = Convert.ToInt32(TempData["volume_id"]);
            Volume item = _volumeService.GetById(volume_id);
            if (file != null && file.ContentLength > 0)
            {
                string _FileName = Path.GetFileName(file.FileName);
                string _path = Path.Combine(Server.MapPath("~/Content/imageVolume"), _FileName);
                file.SaveAs(_path);
                item.volumeImg = _FileName;

            }
            if (ModelState.IsValid)
            {
                item.ordinalNumber = sp.ordinalNumber;
                item.name = sp.name;
                item.story_id = sp.story_id;
                try
                {
                    _volumeService.Update(item);
                    TempData["EditVolume"] = true;
                    return RedirectToAction("EditVolume", new { volume_id = volume_id });
                }
                catch (Exception)
                {

                    throw;
                }
            }
            TempData["ErrorEditVolume"] = true;
            return RedirectToAction("EditVolume", new { volume_id = volume_id });
        }
        [HttpGet]
        public ActionResult DeleteVolume(int volume_id)
        {
            Volume volume = _volumeService.GetById(volume_id);
            TempData["story_id"] = volume.story_id;
            if (volume == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            if (checkRole(volume.story_id) != 1)
                return RedirectToAction("Error403", "Error");
            return View(volume);
        }
        [HttpPost, ActionName("DeleteVolume")]
        public ActionResult XacNhanXoaVolume(int volume_id)
        {
            int story_id = Convert.ToInt32(TempData["story_id"]);
            try
            {
                _volumeService.Delete(volume_id);
            }
            catch (Exception)
            {

                throw;
            }

            return Redirect("/System/Story?story_id="+story_id);
        }

        [HttpGet]
        public ActionResult CreateChapter(int? story_id, int volume_id)
        {
            if (checkRole((int)story_id) == -1)
                return RedirectToAction("Error403", "Error");
            Chapter chapter = new Chapter();
            chapter.story_id = (int)story_id;
            chapter.volume_id = volume_id;
            chapter.ordinalNumber = _chapterService.GetChaptersByStoryId((int)story_id).OrderByDescending(h => h.ordinalNumber).Select(h => h.ordinalNumber).FirstOrDefault() + 1;
            return View(chapter);
        }

        [HttpPost]
        public ActionResult CreateChapter(Chapter sp)
        {
            if (ModelState.IsValid)
            {
                List<string> link_image = new List<string>();
                sp.content = sp.content.Replace("#LineBreak", "@LineBreak").Trim();
                string[] content = sp.content.Split(new string[] { "@LineBreak" }, StringSplitOptions.None);
                for (int i = 0; i < content.Length; i++)
                {
                    if (content[i].StartsWith("http"))
                        sp.content = sp.content.Replace(content[i], "");
                }
                sp.update_date = DateTime.Now;
                try
                {
                    _chapterService.Insert(sp);
                }
                catch (Exception)
                {

                    throw;
                }
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                for (int i = 0; i < content.Length; i++)
                {
                    content[i] = content[i].Replace("@LineBreak", "");
                    if (content[i].StartsWith("http"))
                    {
                        string fileExtension = Path.GetExtension(content[i]);

                        if (allowedExtensions.Contains(fileExtension.ToLower()))
                        {
                            // Kiểm tra xem file có phải là ảnh hợp lệ
                            ImageChapter imageChapter = new ImageChapter();
                            imageChapter.chapter_id = sp.chapter_id;
                            imageChapter.img_name = content[i].Trim();
                            imageChapter.rowNumber = i;
                            try
                            {                                                      
                                _imageService.Insert(imageChapter);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Lỗi: " + ex.Message);
                            }
                        }
                    }
                }
                TempData["CreateChapter"] = true;
                List<StoryFollow> listStoryFollow = _storyFollowService.GetAll().Where(h => h.story_id == sp.story_id).ToList();
                _storyFollowService.UpdateListStoryFollow(listStoryFollow);
                return RedirectToAction("CreateChapter", new { story_id = sp.story_id, volume_id = sp.volume_id });
            }
            TempData["ErrorCreateChapter"] = true;
            return RedirectToAction("CreateChapter", new { story_id = sp.story_id, volume_id = sp.volume_id });
        }

        [HttpGet]
        public ActionResult EditChapter(int chapter_id)
        {
            Chapter chapter = _chapterService.GetById(chapter_id);
            if (chapter != null)
            {
                if (checkRole(chapter.story_id) == -1)
                    return RedirectToAction("Error403", "Error");
                List<ImageChapter> listImage = _imageService.GetByChapterId(chapter_id);
                string[] content = chapter.content.Split(new string[] { "@LineBreak" }, StringSplitOptions.None);
                string result = "";

                for (int i = 0; i < content.Length; i++)
                {
                    foreach (var item in listImage)
                    {
                        if (item.rowNumber == i)
                        {
                            result += item.img_name + "@LineBreak";
                        }
                    }
                    result += content[i] + "@LineBreak";
                }
                chapter.content = result;
                return View(chapter);
            }
            return View();
        }

        [HttpPost]
        public ActionResult EditChapter(Chapter sp)
        {
            if (sp != null)
            {
                if (ModelState.IsValid)
                {
                    Chapter chapter = _chapterService.GetById(sp.chapter_id);
                    List<string> link_image = new List<string>();
                    sp.content = sp.content.Replace("#LineBreak", "@LineBreak").Trim();
                    string[] content = sp.content.Split(new string[] { "@LineBreak" }, StringSplitOptions.None);
                    for (int i = 0; i < content.Length; i++)
                    {
                        if (content[i].StartsWith("http"))
                            sp.content = sp.content.Replace(content[i], "");// xoa link anh khoi noi dung chapter
                    }
                    chapter.content = sp.content;
                    chapter.ordinalNumber = sp.ordinalNumber;
                    chapter.story_id = sp.story_id;
                    chapter.volume_id = sp.volume_id;
                    chapter.name = sp.name;
                    chapter.update_date = DateTime.Now;
                    try
                    {
                        _chapterService.Update(chapter);
                    }
                    catch (Exception)
                    {                 
                        TempData["ErrorEditChapter"] = true;
                        return RedirectToAction("EditChapter", new { chapter_id = sp.chapter_id });
                    }
                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                    List<string> list_image_name = new List<string>();
                    List<ImageChapter> list_image = _chapterService.GetImageByChapterId(sp.chapter_id);
                    for (int i = 0; i < content.Length; i++)
                    {
                        content[i] = content[i].Replace("@LineBreak", "");
                        string temp = content[i];
                        if (content[i].StartsWith("http"))
                        {
                            list_image_name.Add(content[i]);
                            ImageChapter ic = _chapterService.GetImageByChapterId(sp.chapter_id).Where(h => h.img_name == temp).FirstOrDefault();
                            if (ic == null)
                            {
                                string fileExtension = Path.GetExtension(content[i]);
                                if (allowedExtensions.Contains(fileExtension.ToLower()))
                                {
                                   // Kiểm tra xem file có phải là ảnh hợp lệ
                                    ImageChapter imageChapter = new ImageChapter();
                                    imageChapter.chapter_id = sp.chapter_id;
                                    imageChapter.img_name = content[i].Trim();
                                    imageChapter.rowNumber = i;
                                    try
                                    {
                                        _imageService.Insert(imageChapter);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex);
                                    }
                                }
                            }
                            else if (ic.rowNumber != i)
                            {
                                ic.rowNumber = i;
                                _imageService.Update(ic);
                            }
                            ImageChapter ic1 = _imageService.GetByChapterId(sp.chapter_id).Where(h => h.rowNumber == i && h.img_name != temp).FirstOrDefault();
                            if (ic1 != null)
                            {
                                ic1.img_name = temp;
                                _imageService.Update(ic1);
                            }
                        }
                    }
                    foreach (var item in list_image)
                    {
                        if (list_image_name.Contains(item.img_name) == false)
                            _imageService.Delete(item.img_id);
                    }
                    TempData["EditChapter"] = true;
                    return RedirectToAction("EditChapter", new { chapter_id = sp.chapter_id });
                }

            }
            TempData["ErrorEditChapter"] = true;
            return RedirectToAction("EditChapter", new { chapter_id = sp.chapter_id });
        }
        [HttpGet]
        public ActionResult DeleteChapter(int chapter_id)
        {
            Chapter chapter = _chapterService.GetById(chapter_id);
            if (chapter == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            else
            {
                if (checkRole(chapter.story_id) == -1)
                    return RedirectToAction("Error403", "Error");
            }
            return View(chapter);
        }
        [HttpPost, ActionName("DeleteChapter")]
        public ActionResult XacNhanXoaChapter(int chapter_id)
        {
            Chapter chapter = _chapterService.GetById(chapter_id);
            if (chapter == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            int story_id = chapter.story_id;
            try
            {
                _chapterService.Delete(chapter_id);
            }
            catch (Exception)
            {

                throw;
            }
            return Redirect("/System/Story?story_id=" + story_id);
        }

        public string UpdateOrdinalNumber(int volume_id, int ordinalNumber)
        {
            Volume item = _volumeService.GetById(volume_id);
            if (item.ordinalNumber != ordinalNumber)
            {
                item.ordinalNumber = ordinalNumber;
                try
                {
                    _volumeService.Update(item);
                    return "true";

                }
                catch (Exception)
                {
                    throw;
                }
            }
            return "false";
        }
    }
}