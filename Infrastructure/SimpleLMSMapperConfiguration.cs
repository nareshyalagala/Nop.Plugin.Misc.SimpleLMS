using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Nop.Core.Infrastructure.Mapper;
using Nop.Plugin.Misc.SimpleLMS.Areas.Admin.Models;
using Nop.Plugin.Misc.SimpleLMS.Domains;
using Nop.Plugin.Misc.SimpleLMS.Models;

namespace Nop.Plugin.Misc.SimpleLMS.Infrastructure
{
    public class SimpleLMSMapperConfiguration : Profile, IOrderedMapperProfile
    {
        public SimpleLMSMapperConfiguration()
        {
            CreateMaps();
        }

        private void CreateMaps()
        {
            CreateMap<Course, CourseModel>();
            CreateMap<Attachment, AttachmentModel>();
            CreateMap<Lesson, LessonModel>();
            CreateMap<Section, SectionModel>();
            CreateMap<Video, VideoModel>();

            CreateMap<CourseModel, Course>();
            CreateMap<AttachmentModel, Attachment>();
            CreateMap<LessonModel, Lesson>();
            CreateMap<SectionModel, Section>();
            CreateMap<VideoModel, Video>();

            CreateMap<CourseOverviewModel, Course>();
            CreateMap<Course, CourseOverviewModel>();

            CreateMap<Course, CourseDetail>();
            CreateMap<CourseDetail, Course>();

            CreateMap<SectionDetail, Section>();
            CreateMap<Section, SectionDetail>();


            CreateMap<LessonDetail, Lesson>();
            CreateMap<Lesson, LessonDetail>();

            CreateMap<VideoDetail, Video>();
            CreateMap<Video, VideoDetail>();

        }

        public int Order => 99;
    }
}
