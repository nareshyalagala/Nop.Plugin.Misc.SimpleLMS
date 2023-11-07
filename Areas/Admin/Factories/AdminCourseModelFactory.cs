using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework.Models.Extensions;
using Nop.Plugin.Misc.SimpleLMS.Areas.Admin.Models;
using Nop.Plugin.Misc.SimpleLMS.Domains;
using Nop.Plugin.Misc.SimpleLMS.Services;

namespace Nop.Plugin.Misc.SimpleLMS.Areas.Admin.Factories
{
    public partial class AdminCourseModelFactory
    {

        private readonly CourseService _courseService;
        private readonly ICustomerService _customerService;
        private readonly ILocalizationService _localizationService;
        private readonly IProductService _productService;
        private readonly IWorkContext _workContext;

        public AdminCourseModelFactory(
            IProductService productService,
            ILocalizationService localizationService,
            IWorkContext workContext,
            ICustomerService customerService,
            CourseService courseService)
        {
            _productService = productService;
            _workContext = workContext;
            _localizationService = localizationService;
            _customerService = customerService;
            _courseService = courseService;
        }

        public async Task<CourseListModel> PrepareCourseListModelAsync(CourseSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));


            //get courses
            var courses = await _courseService.SearchCoursesAsync(pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize,
                keyword: searchModel.SearchCourseName,
                instructorGUID: searchModel.InstructorGUID);

            //prepare list model
            var model = await new CourseListModel().PrepareToGridAsync(searchModel, courses, () =>
            {
                return courses.SelectAwait(async course =>
                {
                    //fill in model values from the entity
                    var courseModel = course.ToModel<CourseModel>();


                    //fill in additional values (not existing in the entity)
                    courseModel.Category = string.Empty;

                    var product = await _productService.GetProductByIdAsync(course.ProductId);
                    courseModel.ParentProductName = product.Name;
                    courseModel.Price = product.Price;
                    courseModel.Status = product.Published ? "Published" : "Unpublished";

                    var customer = await _customerService.GetCustomerByIdAsync(course.InstructorId);

                    courseModel.Instructor = customer.Username;

                    var courseStat = await _courseService.GetCourseStatsByCourseIdAsync(course.Id);

                    courseModel.LessonsTotal = courseStat.Lessons;
                    courseModel.SectionsTotal = courseStat.Sections;
                    courseModel.EnrolledStudents = courseStat.EnrolledStudents;



                    return courseModel;
                });
            });

            return model;
        }

        public async Task<CourseModel> PrepareCourseModelAsync(CourseModel courseModel, Course course)
        {
            if (courseModel == null)
                throw new ArgumentNullException(nameof(courseModel));

            if (course != null)
            {
                //fill in model values from the entity
                if (course != null)
                {
                    courseModel = course.ToModel<CourseModel>();
                }

                var parentProduct = await _productService.GetProductByIdAsync(course.ProductId);

                if (parentProduct != null)
                {
                    courseModel.ParentProductName = parentProduct.Name;
                    courseModel.ProductId = parentProduct.Id;
                }

                courseModel.Sections = (await _courseService.GetSectionsByCourseIdAsync(courseModel.Id)).OrderBy(p => p.DisplayOrder).Select(p =>
                                          new SectionModel
                                          {
                                              CourseName = courseModel.Name,
                                              DisplayOrder = p.DisplayOrder,
                                              Id = p.Id,
                                              IsFree = p.IsFree,
                                              Title = p.Title,
                                              CourseId = course.Id,
                                              Lessons = _courseService.GetLessonsBySectionIdAsync(p.Id)
                                                          .GetAwaiter().GetResult().Select(l => l.ToModel<LessonModel>()).ToList()
                                          }).ToList();




                for (int i = 0; i < courseModel.Sections.Count; i++)
                {
                    var sectionLessons = await _courseService.GetSectionLessonsBySectionIdAsync(courseModel.Sections[i].Id);

                    for (int j = 0; j < courseModel.Sections[i].Lessons.Count; j++)
                    {
                        courseModel.Sections[i].Lessons[j].SectionId = courseModel.Sections[i].Id;
                        courseModel.Sections[i].Lessons[j].CourseId = courseModel.Id;
                        courseModel.Sections[i].Lessons[j].DisplayOrder
                            = sectionLessons.Where(p => p.LessonId == courseModel.Sections[i].Lessons[j].Id).SingleOrDefault().DisplayOrder;
                    }

                    courseModel.Sections[i].Lessons = courseModel.Sections[i].Lessons.OrderBy(p => p.DisplayOrder).ToList();
                }

            }

            courseModel.AvailableProducts = await _courseService.GetProductsByCurrentUserAsync();
            courseModel.AvailableProducts.Insert(0, new SelectListItem { Text = await _localizationService.GetResourceAsync("SimpleLMS.Select"), Value = "" });

            return courseModel;
        }

        protected IList<LessonModel> GetLessonsBySectionIdForAdmin(int id)
        {
            var task = _courseService.GetLessonsBySectionIdAsync(id);
            task.RunSynchronously();
            var result = task.Result.Select(p => p.ToModel<LessonModel>());
            return result.ToList();
        }


        public async Task<CourseSearchModel> PrepareCourseSearchModelAsync(CourseSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            var customer = await _workContext.GetCurrentCustomerAsync();
            searchModel.InstructorGUID = customer.CustomerGuid.ToString();

            searchModel.SetGridPageSize();

            return searchModel;
        }

        public async Task<SectionModel> PrepareSectionModelAsync(SectionModel sectionModel, Course course, Section section, bool loadLessons = false)
        {
            if (sectionModel == null)
                throw new ArgumentNullException(nameof(sectionModel));

            if (course == null)
                throw new ArgumentNullException(nameof(course));

            if (section != null)
            {
                sectionModel = section.ToModel<SectionModel>();
                sectionModel.CourseId = section.CourseId;

                //if (loadLessons)
                //{

                //}
            }
            else
            {
                sectionModel.CourseId = course.Id;

                //load display order
                var sections = await _courseService.GetSectionsByCourseIdAsync(course.Id);
                if (sections != null && sections.Count > 0)
                {
                    sectionModel.DisplayOrder = sections.Max(p => p.DisplayOrder) + 1;
                }
                if (sectionModel.DisplayOrder == 0)
                {
                    sectionModel.DisplayOrder = 1;
                }
            }

            return sectionModel;
        }

        public async Task<LessonModel> PrepareLessonModelAsync(LessonModel lessonModel, Section section, Course course, Lesson lesson)
        {
            if (lessonModel == null)
                throw new ArgumentNullException(nameof(lessonModel));

            if (course == null)
                throw new ArgumentNullException(nameof(course));

            if (section == null)
                throw new ArgumentNullException(nameof(section));

            if (lesson != null)
            {
                lessonModel = lesson.ToModel<LessonModel>();

                if (lessonModel.LessonType == LessonType.Video)
                {
                    var video = await _courseService.GetVideoByLessonIdAsync(lessonModel.Id);
                    lessonModel.VideoIdFromProvider = video.VideoIdFromProvider;
                    lessonModel.VideoType = video.VideoType;
                    lessonModel.Duration = video.Duration;
                }
            }

            var sls = await _courseService.GetSectionLessonsBySectionIdAsync(section.Id);
            if (sls != null && sls.Count > 0)
            {
                if (lesson != null)
                {
                    lessonModel.DisplayOrder = sls.Where(p => p.LessonId == lesson.Id).SingleOrDefault().DisplayOrder;
                }
                else
                {
                    lessonModel.DisplayOrder = sls.Max(p => p.DisplayOrder) + 1;
                    if (lessonModel.DisplayOrder == 0)
                    {
                        lessonModel.DisplayOrder = 1;
                    }
                }
            }

            if (lessonModel.DisplayOrder == 0)
            {
                lessonModel.DisplayOrder = 1;
            }




            lessonModel.SectionId = section.Id;
            lessonModel.CourseId = course.Id;


            lessonModel.AvailableLessonTypes.Add(new SelectListItem { Value = ((int)LessonType.Video).ToString(), Text = LessonType.Video.ToString() });
            lessonModel.AvailableLessonTypes.Add(new SelectListItem { Value = ((int)LessonType.Text).ToString(), Text = LessonType.Text.ToString() });
            //lessonModel.AvailableLessonTypes.Add(new SelectListItem { Value = ((int)LessonType.Document).ToString(), Text = LessonType.Document.ToString() });


            lessonModel.AvailableVideoTypes.Add(new SelectListItem { Value = ((int)VideoType.Youtube).ToString(), Text = VideoType.Youtube.ToString() });
            lessonModel.AvailableVideoTypes.Add(new SelectListItem { Value = ((int)VideoType.Vimeo).ToString(), Text = VideoType.Vimeo.ToString() });
            //lessonModel.AvailableVideoTypes.Add(new SelectListItem { Value = ((int)VideoType.VdoCipher).ToString(), Text = VideoType.VdoCipher.ToString() });
            //lessonModel.AvailableVideoTypes.Add(new SelectListItem { Value = ((int)VideoType.Hosted).ToString(), Text = VideoType.Hosted.ToString() });
            //lessonModel.AvailableVideoTypes.Add(new SelectListItem { Value = ((int)VideoType.AWS).ToString(), Text = VideoType.AWS.ToString() });

            lessonModel.AvailableAttachmentTypes.Add(new SelectListItem { Value = ((int)AttachmentType.Pdf).ToString(), Text = AttachmentType.Pdf.ToString() });
            //lessonModel.AvailableAttachmentTypes.Add(new SelectListItem { Value = ((int)AttachmentType.Others).ToString(), Text = AttachmentType.Others.ToString() });

            return lessonModel;
        }

        public async Task<SortableEntity> PrepareSortableEntityModelAsync(SortableEntity sortableEntity, IEnumerable<IDisplayOrderRecord> models, SortRecordType sortRecordType, int parentId)
        {
            if (sortableEntity == null)
                throw new ArgumentNullException(nameof(sortableEntity));

            sortableEntity.ParentId = parentId;
            sortableEntity.SortRecords = await (from p in models
                                                orderby p.DisplayOrder
                                                select new SortRecord
                                                {
                                                    Id = p.Id,
                                                    ExistingSortOrder = p.DisplayOrder,
                                                    NewSortOrder = p.DisplayOrder,
                                                    DisplayText = p.DisplayText
                                                }).ToListAsync();

            sortableEntity.SortRecordType = sortRecordType;

            return sortableEntity;
        }
    }

}
