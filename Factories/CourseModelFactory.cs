using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Plugin.Misc.SimpleLMS.Services;
using Nop.Plugin.Misc.SimpleLMS.Domains;
using Nop.Plugin.Misc.SimpleLMS.Models;
using Nop.Web.Framework.Models.Extensions;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Services.Media;
using Nop.Core.Domain.Customers;

namespace Nop.Plugin.Misc.SimpleLMS
{
    public partial class CourseModelFactory
    {
        private readonly CourseService _courseService;
        private readonly ICustomerService _customerService;

        private readonly ILocalizationService _localizationService;
        private readonly IProductService _productService;
        private readonly IWorkContext _workContext;

        private readonly IPictureService _pictureService;


        public CourseModelFactory(
            CourseService courseService,
            ICustomerService customerService,
            ILocalizationService localizationService,
            IProductService productService,
            IWorkContext workContext,
            IPictureService pictureService)
        {

            _courseService = courseService;
            _customerService = customerService;
            _localizationService = localizationService;
            _productService = productService;
            _workContext = workContext;
            _pictureService = pictureService;
        }

        public async Task<CourseSearchModel> PrepareCourseSearchModelAsync(CourseSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            searchModel.CourseOverviewList = await PrepareCourseOverviewListModelAsync(searchModel);

            searchModel.SetGridPageSize();

            return searchModel;
        }




        public async Task<CourseOverviewListModel> PrepareCourseOverviewListModelAsync(CourseSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            var customer = await _workContext.GetCurrentCustomerAsync();


            var courses = await _courseService.SearchCoursesAsync(pageIndex: searchModel.PageNumber - 1,
                pageSize: searchModel.PageSize > 0 ? searchModel.PageSize : 10,
                keyword: searchModel.SearchCourseName,
                userId: customer.Id);


            var model = new CourseOverviewListModel();


            model.Courses = (await PrepareCourseOverviewModelsAsyns(courses, customer)).ToList();
            model.LoadPagedList(courses);

            return model;
        }

        private async Task<IList<CourseOverviewModel>> PrepareCourseOverviewModelsAsyns(IPagedList<Course> courses, Customer customer)
        {
            if (courses == null)
                throw new ArgumentNullException(nameof(courses));


            var models = new List<CourseOverviewModel>();
            foreach (var course in courses)
            {

                var model = course.ToModel<CourseOverviewModel>();

                var product = await _productService.GetProductByIdAsync(course.ProductId);
                model.ParentProductName = product.Name;

                model.CourseProgress = await _courseService.GetCourseProgressPercentByUserId(course.Id, customer.Id);
                var pictures = await _pictureService.GetPicturesByProductIdAsync(product.Id);


                if (pictures != null && pictures.Count() > 0)
                    model.ProductMainImage = (await _pictureService.GetPictureUrlAsync(pictures[0])).Url;


                models.Add(model);
            }

            return models;
        }


        public async Task<CourseDetail> PrepareCourseDetailModelAsync(CourseDetail courseDetail, Course course, IList<LessonProgress> lessonProgresses)
        {

            if (courseDetail == null)
                throw new ArgumentNullException(nameof(courseDetail));

            if (course != null)
            {
                //fill in model values from the entity
                if (course != null)
                {
                    courseDetail = course.ToModel<CourseDetail>();
                }

                var parentProduct = await _productService.GetProductByIdAsync(course.ProductId);

                if (parentProduct != null)
                {
                    courseDetail.ParentProductName = parentProduct.Name;
                    courseDetail.ProductId = parentProduct.Id;
                }

                courseDetail.Sections = (await _courseService.GetSectionsByCourseIdAsync(courseDetail.Id)).OrderBy(p => p.DisplayOrder).Select(p =>
                                          new SectionDetail
                                          {
                                              CourseName = courseDetail.Name,
                                              DisplayOrder = p.DisplayOrder,
                                              Id = p.Id,
                                              IsFree = p.IsFree,
                                              Title = p.Title,
                                              CourseId = course.Id,
                                              Lessons = _courseService.GetLessonsBySectionIdAsync(p.Id)
                                                          .GetAwaiter().GetResult().Select(l => l.ToModel<LessonDetail>()).ToList()
                                          }).ToList();





                for (int i = 0; i < courseDetail.Sections.Count; i++)
                {
                    var sectionLessons = await _courseService.GetSectionLessonsBySectionIdAsync(courseDetail.Sections[i].Id);


                    for (int j = 0; j < courseDetail.Sections[i].Lessons.Count; j++)
                    {
                        courseDetail.Sections[i].Lessons[j].SectionId = courseDetail.Sections[i].Id;
                        courseDetail.Sections[i].Lessons[j].CourseId = courseDetail.Id;
                        courseDetail.Sections[i].Lessons[j].DisplayOrder
                            = sectionLessons.Where(p => p.LessonId == courseDetail.Sections[i].Lessons[j].Id).SingleOrDefault().DisplayOrder;
                        courseDetail.Sections[i].Lessons[j].Duration = courseDetail.Sections[i].Lessons[j].LessonType == LessonType.Video ?
                            _courseService.GetVideoByLessonIdAsync(courseDetail.Sections[i].Lessons[j].Id).GetAwaiter().GetResult().Duration : 0;

                        var lessonProgress = lessonProgresses.Where(p => p.LessonId == courseDetail.Sections[i].Lessons[j].Id).SingleOrDefault();

                        courseDetail.Sections[i].Lessons[j].IsCompleted = (lessonProgress != null ? lessonProgress.IsCompleted : false);

                        if (courseDetail.Sections[i].Lessons[j].Video != null && lessonProgress.CurrentlyAt > 0)
                        {
                            courseDetail.Sections[i].Lessons[j].Video.TimeCode =
                                (courseDetail.Sections[i].Lessons[j].Video.VideoType == VideoType.Vimeo ?
                                lessonProgress.CurrentlyAt / 60 + "m" + (lessonProgress.CurrentlyAt % 60) + "s" : lessonProgress.CurrentlyAt.ToString());
                        }

                        if (courseDetail.CurrentLesson == 0 && !courseDetail.Sections[i].Lessons[j].IsCompleted)
                        {
                            courseDetail.CurrentLesson = courseDetail.Sections[i].Lessons[j].Id;
                            courseDetail.CurrentSection = courseDetail.Sections[i].Id;
                        }
                    }


                    courseDetail.Sections[i].Lessons = courseDetail.Sections[i].Lessons.OrderBy(p => p.DisplayOrder).ToList();
                }

                if (courseDetail.CurrentLesson == 0)
                {
                    var currSection = (from p in courseDetail.Sections
                                       where p.Lessons.Count > 0
                                       select p).FirstOrDefault();
                    if (currSection != null)
                    {
                        courseDetail.CurrentLesson = currSection.Lessons.FirstOrDefault().Id;
                        courseDetail.CurrentSection = currSection.Id;
                    }
                }

            }

            return courseDetail;
        }

        public async Task<LessonDetail> PrepareLessonModelAsync(LessonDetail lessonDetail, Lesson lesson)
        {
            if (lessonDetail == null)
                throw new ArgumentNullException(nameof(lessonDetail));


            if (lesson != null)
            {
                lessonDetail = lesson.ToModel<LessonDetail>();
                if (lesson.LessonType == LessonType.Video)
                {
                    lessonDetail.Video = (await _courseService.GetVideoByLessonIdAsync(lessonDetail.Id)).ToModel<VideoDetail>();
                }
            }


            return lessonDetail;

        }
    }
}
