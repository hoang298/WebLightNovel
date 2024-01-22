using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebLightNovel.Models.Entity;
using WebLightNovel.Models.Member;
using WebLightNovel.Models.ModelStory;
using Google.Apis.AnalyticsReporting.v4;
using Google.Apis.AnalyticsReporting.v4.Data;
using Google.Apis.Auth.OAuth2;
using System.Threading;
using Google.Apis.Services;
using WebLightNovel.Models.Account;
using Microsoft.AspNet.Identity;
using AutoMapper;
using WebLightNovel.Mappings;
using System.Data.Entity;
using WebLightNovel.Extensions;
using WebLightNovel.Models.Detail;
using WebLightNovel.Service.Stories;
using WebLightNovel.Service.Users;
using WebLightNovel.Service.Common;

namespace WebLightNovel.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStoryService _storyService;
        private readonly IGenreService _genreService;
        private readonly IAccountService _accountService;
        private readonly INotifyService _notifyService;
        private readonly IStoryFollowService _storyFollowService;
        private readonly IStoryHistoryService _storyHistoryService;
        public HomeController(
          IStoryService storyService, 
          IGenreService genreService,
          IAccountService accountService,
          INotifyService notifyService
          )
        {
            _storyService = storyService;
            _genreService = genreService;
            _accountService = accountService;
            _notifyService = notifyService;
            _storyFollowService = new StoryFollowService();
            _storyHistoryService = new StoryHistoryService();
        }
        #region Utilities

        public void AddHistory(int story_id, int chapter_id)
        {
            if (User.Identity.IsAuthenticated)
            {
                string user_id = User.Identity.GetUserId();
                _storyHistoryService.AddStoryToHistory(user_id, story_id, chapter_id);
            }
        }

        public void DeleteHistory(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                string user_id = User.Identity.GetUserId();
                _storyHistoryService.DeleteStoryHistory(user_id, id);
            }
        }
        #endregion

        #region Methods
        public ActionResult Index()
        {
            // Xác thực và tạo phiên làm việc
            /*UserCredential credential;
            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    new[] { AnalyticsReportingService.Scope.Analytics },
                    "user",
                    CancellationToken.None
                ).Result;
            }

            // Tạo dịch vụ Analytics Reporting
            var reportingService = new AnalyticsReportingService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "Your Application Name"
            });
            // Tạo yêu cầu truy vấn
            var request = new ReportRequest
            {
                ViewId = "ga:G-6MJCMFB53J",
                DateRanges = new List<DateRange> { new DateRange { StartDate = "2023-06-02", EndDate = "2023-06-03" } },
                Metrics = new List<Metric> { new Metric { Expression = "ga:sessions", Alias = "Sessions" } }
            };

            // Gửi yêu cầu truy vấn
            var getReportsRequest = new GetReportsRequest { ReportRequests = new List<ReportRequest> { request } };
            var batchRequest = reportingService.Reports.BatchGet(getReportsRequest);
            var response = batchRequest.Execute();

            // Xử lý kết quả
            var report = response.Reports.FirstOrDefault();
            var data = report?.Data;
            var totalSessions = data?.Totals.FirstOrDefault()?.Values.FirstOrDefault();*/

            // Cập nhật thông tin vào trang web
            // ...         


            return View();
        }
        [ChildActionOnly]
        public ActionResult RenderStories()
        {    
            List<Story> listTopView = _storyService.GetStoryByTag("topView");
            List<Story> listUpdate = _storyService.GetStoryByTag("lastUpdate");
            var listUpdateDTO = Mapper.Map<List<StoryIndexViewModel>>(listUpdate);
            var listTopViewDTO = Mapper.Map<List<StoryIndexViewModel>>(listTopView);
            ViewBag.ListTopView = listTopViewDTO;
            return PartialView("Main_Index", listUpdateDTO);
        }
        [ChildActionOnly]
        public ActionResult RenderTopView()
        {
            List<Story> listTopViewDay = _storyService.GetStoryByTag("topViewDay");
            var listTopViewDayDTO = Mapper.Map<List<StoryIndexViewModel>>(listTopViewDay);
            return PartialView("ContentTopView", listTopViewDayDTO);
        }
        [ChildActionOnly]
        public ActionResult InfoUser()
        {
            UserHeaderModel userHeader = new UserHeaderModel();
            if (User.Identity.IsAuthenticated)
            {
                string user_id = User.Identity.GetUserId();
                List<StoryFollow> listFollow = _storyFollowService.GetAllByUserId(user_id);
                List<Notify> listNotify = _notifyService.GetAllByUserId(user_id);
                int total_follow = 0;
                userHeader.user_id = user_id;
                userHeader.name = User.Identity.GetUserNickName();
                userHeader.avatar = User.Identity.GetUserAvatar();
                if (listFollow.Count > 0)
                    total_follow = listFollow.Select(h => h.total_unread).Sum();
                if (total_follow > 0)
                    userHeader.total_follow = total_follow;
                List<Notify> listNotify_unread = listNotify.Where(h => h.state == 0).ToList();
                if (listNotify_unread.Count > 0)
                    userHeader.total_notify = listNotify_unread.Count;   
                if (listNotify.Count > 0)
                    ViewBag.listNotify = Mapper.Map<List<NotifyViewModel>>(listNotify);
            }
            return PartialView("Header_Index", userHeader);
        }
        [ChildActionOnly]
        [PartialCacheAttribute("1DayCache")]
        public ActionResult RenderGenres()
        {
            List<Genre> genres = _genreService.GetAllGenre();
            List<GenreViewModel> genresViewModel = Mapper.Map<List<GenreViewModel>>(genres);
            return View(genresViewModel);
        }
       

        public ActionResult Convert_Story(int type)
        {
            List<Story> listStory = _storyService.GetStoryByType(type);
            var listStoryDTO = Mapper.Map<List<StoryIndexViewModel>>(listStory);
            if (type == 0)
                ViewBag.convert = "Sáng tác";
            else if(type == 1)
                ViewBag.convert = "Truyện dịch";
            else
                ViewBag.convert = "Máy dịch";
            return View(listStoryDTO);
        }

        public ActionResult Instruct()
        {
            return View();
        }
        public ActionResult History()
        {
            return View();
        }

        public ActionResult RenderHistory()
        {
            string content = "";
            List<StoryHistoryViewModel> listHistory = new List<StoryHistoryViewModel>();
            string user_id = User.Identity.GetUserId();
            if (User.Identity.IsAuthenticated)
                listHistory = _storyHistoryService.GetStoryHistory(user_id);
            foreach (var item in listHistory)
            {
                content += "<div class=\"manga__item\">" +
                            "<div class=\"manga__item-img\">" +
                                "<a href=\"/truyen/" + DataConverter.ConvertLinkStory(item.story_id, item.story_name) + "\">" +
                                    "<img src=\"/Content/imageIllustration/" + item.image + "\"</img>" +
                                "</a>" +
                                "<div class=\"manga__item-view2\">" +
                                    "<a id=\"" + item.chapter_id + "\" class=\"history__btn-delete_account history__btn-delete2\" href=\"#\">" +
                                        "<i class=\"fa fa-times\"></i>" +
                                        " Xóa" +
                                    "</a>" +
                                "</div>" +
                            "</div>" +
                            "<div class=\"manga__item-caption\">" +
                                "<h3 class=\"manga__item-title\">" + item.story_name +
                                    "<a href=\"/truyen/" +DataConverter.ConvertLinkStory(item.story_id, item.story_name) + "\">"   + "</a>" +
                                "</h3>" +
                                "<ul class=\"manga__item-history\">" +
                                    "<li class=\"item__history-chapter \">" +
                                        "<a class=\"overflowContent\" href=\"/truyen/"+ DataConverter.ConvertLinkChapter(item.story_id, item.story_name, item.chapter_id, item.chapterName) + "\">" + item.chapterName + "</a>" +
                                    "</li>" +
                                "</ul>" +
                            "</div>" +
                        "</div>";
            }

            return Content(content, "text/html");
        }

        #endregion

    }

}