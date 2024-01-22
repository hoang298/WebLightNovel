using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WebLightNovel.Extensions
{
    public static class DataConverter
    {
        public static string ConvertLinkStory(int story_id, string title)
        {
            title = Regex.Replace(title, @"[^\w\s]", "");// xoa cac ki tu dac biet tru dau cach
            return story_id + "/" + title.Trim().Replace(" ", "-").ToLower();
        }
        public static string ConvertLinkChapter(int story_id, string title, int newChapter_id, string chapter_name)
        {
            title = Regex.Replace(title, @"[^\w\s]", "");
            chapter_name = Regex.Replace(chapter_name, @"[^\w\s]", "");
            return story_id + "/" + title.Trim().Replace(" ", "-").ToLower() + "/" + newChapter_id + "/" + chapter_name.Trim().Replace(" ", "-").ToLower();
        }
        public static string ConvertTimeChapter(DateTime dateTime)
        {
            string convertTime = "";
            DateTime startTime = dateTime;
            DateTime endTime = DateTime.Now;
            TimeSpan duration = endTime - startTime;
            int days = duration.Days;
            int hours = duration.Hours;
            int minutes = duration.Minutes;         
            if (days > 30)
                convertTime = dateTime.ToString("dd-MM-yyyy");
            else if (days > 0)
                convertTime = days + " ngày";
            else if (hours > 0)
                convertTime = hours.ToString() + " giờ";
            else
                convertTime = minutes.ToString() + " phút";
            return convertTime;
        }

        public static string ConvertTimeLetter(DateTime dateTime)
        {
            string convertTime = "";
            DateTime startTime = dateTime;
            DateTime endTime = DateTime.Now;
            TimeSpan duration = endTime - startTime;
            int days = duration.Days;
            int hours = duration.Hours;
            int minutes = duration.Minutes;
            int week = days / 7;
            if (week > 0)
                convertTime = week.ToString() + " tuần";
            else if (days > 0)
                convertTime = days + " ngày";
            else if (hours > 0)
                convertTime = hours.ToString() + " giờ";
            else
                convertTime = minutes.ToString() + " phút";
            return convertTime;
        }
    }
}