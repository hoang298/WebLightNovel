using AutoMapper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebLightNovel.Extensions;
using WebLightNovel.Models.Entity;
using WebLightNovel.Models.ModelStory;
using WebLightNovel.Service.Stories;

namespace WebLightNovel.Controllers
{
    public class SearchController : Controller
    {
        private readonly ImageService _imageService;
        private readonly IChapterService _chapterService;
        private readonly IStoryService _storyService;
        public SearchController(
            IStoryService storyService
        )
        {
        _storyService = storyService;
            _imageService = new ImageService();
            _chapterService = new ChapterService();
        }
        // GET: Search
        public Bitmap Lightness(Bitmap hinhgoc)
        {
            int width = hinhgoc.Width;
            int height = hinhgoc.Height;
            Bitmap hinhxam = new Bitmap(width, height);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Color pixel = hinhgoc.GetPixel(i, j);
                    int gray = (int)(pixel.R * 0.3 + pixel.G * 0.59 + pixel.B * 0.11);
                    hinhxam.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
                }
            }
            return hinhxam;
        }

        public Bitmap RemoveBgr(Bitmap bitmap)
        {
            // Tìm kích thước của phần nội dung ảnh
            int a = 0, b = 0, width = bitmap.Width, height = bitmap.Height;
            Color backgroundColor = Color.FromArgb(255, 255, 255); // Màu nền (trắng)

            // Xoá các dòng đen ở phía trên
            for (int i = 0; i < bitmap.Height; i++)
            {
                bool isRowBlank = true;
                for (int j = 0; j < bitmap.Width; j++)
                {
                    if (bitmap.GetPixel(j, i) != backgroundColor)
                    {
                        isRowBlank = false;
                        break;
                    }
                }
                if (!isRowBlank)
                {
                    b = i;
                    break;
                }
            }

            // Xoá các dòng đen ở phía dưới
            for (int i = bitmap.Height - 1; i >= 0; i--)
            {
                bool isRowBlank = true;
                for (int j = 0; j < bitmap.Width; j++)
                {
                    if (bitmap.GetPixel(j, i) != backgroundColor)
                    {
                        isRowBlank = false;
                        break;
                    }
                }
                if (!isRowBlank)
                {
                    height = i - b + 1;
                    break;
                }
            }

            // Xoá các cột đen ở phía trái
            for (int i = 0; i < bitmap.Width; i++)
            {
                bool isColumnBlank = true;
                for (int j = 0; j < height; j++)
                {
                    if (bitmap.GetPixel(i, j + b) != backgroundColor)
                    {
                        isColumnBlank = false;
                        break;
                    }
                }
                if (!isColumnBlank)
                {
                    a = i;
                    break;
                }
            }

            // Xoá các cột đen ở phía phải
            for (int i = bitmap.Width - 1; i >= 0; i--)
            {
                bool isColumnBlank = true;
                for (int j = 0; j < height; j++)
                {
                    if (bitmap.GetPixel(i, j + b) != backgroundColor)
                    {
                        isColumnBlank = false;
                        break;
                    }
                }
                if (!isColumnBlank)
                {
                    width = i - a + 1;
                    break;
                }
            }

            // Tạo ảnh mới với kích thước đã được cắt bỏ phần nền
            Bitmap croppedImage = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(croppedImage);
            g.DrawImage(bitmap, new Rectangle(0, 0, width, height), new Rectangle(a, b, width, height), GraphicsUnit.Pixel);
            return croppedImage;
        }
        public bool IsLinkImage(string url)
        {
            url = url.ToLower();
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif",".bmp",".tiff",".webp",".svg" };
            return url.Contains(allowedExtensions[0]) ||
                url.Contains(allowedExtensions[1]) ||
                url.Contains(allowedExtensions[2]) ||
                url.Contains(allowedExtensions[3]) ||
                url.Contains(allowedExtensions[4]) ||
                url.Contains(allowedExtensions[5]) ||
                url.Contains(allowedExtensions[6]);
        }
        public List<ModelSearchChapter> compareImage(Bitmap bitmap)
        {
            bitmap = new Bitmap(RemoveBgr(bitmap), 32, 32);
            Bitmap grayImage = Lightness(bitmap);
            Double[,] dctValues = new Double[32, 32];
            List<ImageChapter> listImage = _imageService.GetAll().Where(h => h.pHashImage != null).ToList();
            List<ImageChapter> listImageResult = new List<ImageChapter>();
            for (int x = 0; x < 32; x++)
            {
                for (int y = 0; y < 32; y++)
                {
                    dctValues[x, y] = 0.0;

                    for (int i = 0; i < 32; i++)
                    {
                        for (int j = 0; j < 32; j++)
                        {
                            dctValues[x, y] += (grayImage.GetPixel(i, j).R) * Math.Cos((2 * i + 1) * x * Math.PI / 64) * Math.Cos((2 * j + 1) * y * Math.PI / 64);
                        }
                    }
                    dctValues[x, y] *= ((x == 0 ? 1 / Math.Sqrt(2.0) : 1) * (y == 0 ? 1 / Math.Sqrt(2.0) : 1) * 0.25);
                }
            }


            Double sum = 0.0;
            for (int x = 0; x < 8; x++)
                for (int y = 0; y < 8; y++)
                    sum += dctValues[x, y];
            sum -= dctValues[0, 0];
            Double mean = sum / 63.0;
            String hash = "";
            for (int x = 0; x < 8; x++)
                for (int y = 0; y < 8; y++)
                    hash += (dctValues[x, y] > mean ? "1" : "0");
            string r = "";
            for (int i = 0; i < 16; i++)
            {
                string temp = hash.Substring(0, 4);
                hash = hash.Remove(0, 4);
                int intValue = Convert.ToInt32(temp, 2);
                string hexValue = intValue.ToString("X");
                r += hexValue;
            }
            hash = r;
            Dictionary<int, string> matchRate = new Dictionary<int, string>();
            foreach (var item in listImage)
            {
                long valueItem = Convert.ToInt64(item.pHashImage, 16);
                long valueItemHash = Convert.ToInt64(hash, 16);
                long resultXor = valueItem ^ valueItemHash;
                string resultXorBit = Convert.ToString(resultXor, 2);
                resultXorBit = resultXorBit.Replace("0", "");
                int distance = resultXorBit.Length;
                if(distance < 10)
                {
                    listImageResult.Add(item);
                    matchRate[item.chapter_id]=((64 - distance) / 64.0 * 100).ToString("0.00");
                }
            }
            List<int> listChapterId = listImageResult.Select(h => h.chapter_id).ToList();
            List<Chapter> listChapter = _chapterService.GetChaptersByListChapterId(listChapterId);
            List<int> listStoryId = listChapter.Select(h => h.story_id).ToList();
            List<Story> listStory = _storyService.GetStoriesByListStoryId(listStoryId);
            var result = from item in listImageResult
                         from item1 in listChapter
                         where item.chapter_id == item1.chapter_id
                         select new ModelSearchChapter
                         {
                             chapter_id = item.chapter_id,
                             chapter_name = item1.name,
                             image_name = item.img_name,
                             link_chapter = DataConverter.ConvertLinkChapter(item1.story_id, listStory.Where(h => h.story_id == item1.story_id).Select(h => h.title).FirstOrDefault(), item1.chapter_id, item1.name),
                             matchRate = matchRate[item1.chapter_id]
                         };
            return result.ToList();
        }

        public ActionResult Search(string key, int? page)
        {
            ViewBag.key = key;
            int sotrang = 16;
            int pageNumber = (page ?? 1);
            ViewBag.page = pageNumber;
            List<Story> listStory = _storyService.GetStoryByName(key);
            ViewBag.countPage = listStory.Count / sotrang + 1;
            List<Story> listStoryPage = listStory.Skip(sotrang * (pageNumber - 1)).Take(16).ToList();
            var listStoryDTO = Mapper.Map<List<StoryIndexViewModel>>(listStoryPage);
            return View(listStoryPage);
        }
        public ActionResult SearchAdvanced()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchAdvanced(HttpPostedFileBase file, string url)
        {
            Bitmap bitmap;
            if (url != "")
            {
                if (!IsLinkImage(url))
                {
                    ViewBag.Exception = true;
                    return View();
                }
                using (WebClient webClient = new WebClient())
                {
                    try
                    {
                        // đọc ảnh từ URL
                        byte[] data = webClient.DownloadData(url);
                        // đưa ảnh vào dạng Bitmap
                        using (MemoryStream memoryStream = new MemoryStream(data))
                        {
                            bitmap = new Bitmap(memoryStream);
                            if (bitmap != null)
                            {
                                return View(compareImage(bitmap));
                            }
                            // hiển thị ảnh
                        }
                    }
                    catch (Exception)
                    {
                        ViewBag.Exception = true;
                        return View();
                    }
                   
                }
            }
            else
            {
                try
                {
                    if (file != null && file.ContentLength > 0 && ModelState.IsValid)
                    {
                        bitmap = new Bitmap(file.InputStream);
                        if (bitmap != null)
                            return View(compareImage(bitmap));
                    }
                }
                catch (Exception)
                {
                    ViewBag.Exception = true;
                }
            }

            return View();
        }
    }
}