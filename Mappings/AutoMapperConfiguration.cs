using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebLightNovel.Models.Entity;
using WebLightNovel.Models.Detail;
using WebLightNovel.Models.ModelStory;
using WebLightNovel.Models.DetailChapter;
using WebLightNovel.Models.SystemModel;
using WebLightNovel.Extensions;
using WebLightNovel.Models.Member;
using static WebLightNovel.Models.Enum.EnumModel;

namespace WebLightNovel.Mappings
{
    public class AutoMapperConfiguration
    {



        public static void Configure()
        {
            //Home
            Mapper.CreateMap<Story, StoryIndexViewModel>()
                .AfterMap((src, dest) =>
                {
                    dest.newtimeUpdate = DataConverter.ConvertTimeChapter((DateTime)src.timeUpdate);
                    dest.link_story = DataConverter.ConvertLinkStory(dest.story_id, dest.title);
                    dest.link_chapter = DataConverter.ConvertLinkChapter(dest.story_id, dest.title, (int)dest.newChapter_id, dest.chapter_name);
                });
            Mapper.CreateMap<Notify, NotifyViewModel>()
               .AfterMap((src, dest) =>
               {
                   dest.time = DataConverter.ConvertTimeLetter((DateTime)src.time);                
               });
            Mapper.CreateMap<Story, SearchStoryViewModel>()
                .AfterMap((src, dest) =>
                {
                    dest.link_story = dest.story_id + "/" + dest.title.Trim().Replace(" ", "-").Replace(".", "").ToLower();
                });
            Mapper.CreateMap<Genre, GenreViewModel>()
                .AfterMap((src, dest) =>
                {
                    dest.link_genre = "/the-loai/" + src.name.Trim().Replace(" ", "-").ToLower();
                });
            //Detail
            Mapper.CreateMap<Comment, CommentViewModel>()
                .AfterMap((src, dest) =>
                {
                dest.time = DataConverter.ConvertTimeLetter(src.time);
                });
            Mapper.CreateMap<Comment, AddCommentViewModel>().ReverseMap();
            Mapper.CreateMap<Story, StoryDetailViewModel>()
                 .AfterMap((src, dest) =>
                 {
                     dest.type_name = src.type == 0 ? "Sáng tác" : src.type == 1 ? "Truyện dịch" : "Máy dịch";
                     dest.state_name = src.state == 0 ? "Tạm ngưng" : src.state == 1 ? "Đang tiến hành" : "Hoàn thành";
                 });

            //DetailChapter
            Mapper.CreateMap<Chapter, ChapterViewModel>()
                .AfterMap((src, dest) =>
                {
                    dest.newtimeUpdate = DataConverter.ConvertTimeChapter((DateTime)src.update_date);
                });
            Mapper.CreateMap<Chapter, ChapterDetailViewModel>();
            Mapper.CreateMap<ImageChapter, ImageViewModel>();
            Mapper.CreateMap<Volume, VolumeViewModel>();
            //System
            Mapper.CreateMap<TranslationTeam, GroupViewModel>();
            Mapper.CreateMap<Volume, Volume_StorySystem>();
            Mapper.CreateMap<Chapter, Chapter_StorySystem>();
            Mapper.CreateMap<Story, InfoGroupStoryViewModel>()
                 .AfterMap((src, dest) =>
                 {
                     dest.image = src.cover_image;
                     dest.state = (statusStory)src.state;
                 });
            //Member
            Mapper.CreateMap<Story, mStoryFollow>()
                .AfterMap((src, dest) =>
                {
                    dest.link_story = DataConverter.ConvertLinkStory(dest.story_id, dest.title);
                    dest.link_chapter = DataConverter.ConvertLinkChapter(dest.story_id, dest.title, (int)dest.newChapter_id, dest.chapter_name);
                });
            ;
        }

    }
}
