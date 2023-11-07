using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
//using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Misc.SimpleLMS.Areas.Admin.Factories;
using Nop.Plugin.Misc.SimpleLMS.Areas.Admin.Models;
using Nop.Plugin.Misc.SimpleLMS.Domains;
using Nop.Plugin.Misc.SimpleLMS.Services;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Misc.SimpleLMS.Areas.Admin.Controllers
{
    public partial class CourseController : BaseAdminController
    {
        private readonly IWorkContext _workContext;
        private readonly IPermissionService _permissionService;
        private readonly ILocalizationService _localizationService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly AdminCourseModelFactory _courseModelFactory;
        private readonly CourseService _courseService;
        private readonly INotificationService _notificationService;

        public CourseController(IPermissionService permissionService
            , ILocalizationService localizationService
            , ICustomerActivityService customerActivityService
            , ICustomerService customerService
            , AdminCourseModelFactory courseModelFactory
            , CourseService courseService
            , INotificationService notificationService
            , IWorkContext workContext)
        {
            _permissionService = permissionService;
            _localizationService = localizationService;
            _customerActivityService = customerActivityService;
            _courseModelFactory = courseModelFactory;
            _courseService = courseService;
            _notificationService = notificationService;
            _workContext = workContext;
        }

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual async Task<IActionResult> List()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var courseSearchModel = new CourseSearchModel();

            //prepare model
            var model = await _courseModelFactory.PrepareCourseSearchModelAsync(courseSearchModel);

            return View("~/Plugins/Misc.SimpleLMS/Areas/Admin/Views/Course/List.cshtml", model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> CourseList(CourseSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return await AccessDeniedDataTablesJson();

            var customer = await _workContext.GetCurrentCustomerAsync();

            searchModel.InstructorGUID = customer.CustomerGuid.ToString();

            //prepare model
            var model = await _courseModelFactory.PrepareCourseListModelAsync(searchModel);

            return Json(model);
        }


        public virtual async Task<IActionResult> Create()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var model = await _courseModelFactory.PrepareCourseModelAsync(new CourseModel(), null);

            return View("~/Plugins/Misc.SimpleLMS/Areas/Admin/Views/Course/Create.cshtml", model);
        }

        public virtual async Task<IActionResult> Sections(int courseId)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var courseExisting = await _courseService.GetCourseById(courseId);
            var customer = await _workContext.GetCurrentCustomerAsync();

            if (courseExisting.InstructorGuid != customer.CustomerGuid)
                return await AccessDeniedDataTablesJson();



            var model = await _courseModelFactory.PrepareCourseModelAsync(new CourseModel(), courseExisting);

            return PartialView("~/Plugins/Misc.SimpleLMS/Areas/Admin/Views/Course/_CreateOrUpdate.Sections.cshtml", model.Sections);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Create(CourseModel model, bool continueEditing)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var customer = await _workContext.GetCurrentCustomerAsync();
                var course = model.ToEntity<Course>();

                course.InstructorId = customer.Id;
                course.InstructorGuid = customer.CustomerGuid;
                course.CreatedOnUtc = DateTime.UtcNow;
                course.UpdatedOnUtc = DateTime.UtcNow;

                await _courseService.InsertOrUpdateCourseAsync(course);

                //foreach (var sectionModel in model.Sections)
                //{
                //    var section = sectionModel.ToEntity<Section>();
                //    section.CourseId = course.Id;
                //    await _courseService.InsertOrUpdateSectionAsync(section);
                //    foreach (var lessonModel in sectionModel.Lessons)
                //    {
                //        var lesson = lessonModel.ToEntity<Lesson>();
                //        await _courseService.InsertOrUpdateLessonAsync(lesson);
                //        await _courseService.InsertOrUpdateSectionLessonAsync(new SectionLesson
                //        {
                //            CourseId = course.Id,
                //            DisplayOrder = lessonModel.DisplayOrder,
                //            LessonId = lesson.Id,
                //            IsFreeLesson = lessonModel.IsFreeLesson,
                //            SectionId = section.Id
                //        });

                //        if (lessonModel.LessonType == LessonType.Document)
                //        {
                //            foreach (var attachmentModel in lessonModel.Attachments)
                //            {
                //                var attachment = attachmentModel.ToEntity<Attachment>();
                //                attachment.InstructorId = customer.Id;
                //                attachment.InstructorGuid = customer.CustomerGuid;
                //                await _courseService.InsertOrUpdateAttachmentAsync(attachment);
                //                await _courseService.InsertOrUpdateLessonAttachmentAsync(new LessonAttachment
                //                {
                //                    AttachmentId = attachment.Id,
                //                    LessonId = lesson.Id
                //                });
                //            }
                //        }
                //        else if (lessonModel.LessonType == LessonType.Video)
                //        {
                //            if (lessonModel.Video != null)
                //            {
                //                var video = lessonModel.Video.ToEntity<Video>();
                //                await _courseService.InsertOrUpdateVideoAsync(video);
                //                lesson.VideoId = video.Id;
                //                await _courseService.InsertOrUpdateLessonAsync(lesson);
                //            }
                //        }

                //    }
                //}

                if (!continueEditing)
                    return RedirectToAction("List");

                model = await _courseModelFactory.PrepareCourseModelAsync(new CourseModel(), null);

                return RedirectToAction("Edit", new { id = course.Id });
            }

            return View(model);
        }


        public virtual async Task<IActionResult> Edit(int id)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return await AccessDeniedDataTablesJson();


            var course = await _courseService.GetCourseById(id);

            var customer = await _workContext.GetCurrentCustomerAsync();

            if (course.InstructorGuid != customer.CustomerGuid && course.Id != id)
                return await AccessDeniedDataTablesJson();
            //prepare model
            var model = await _courseModelFactory.PrepareCourseModelAsync(new CourseModel(), course);

            return View("~/Plugins/Misc.SimpleLMS/Areas/Admin/Views/Course/Edit.cshtml", model);
        }


        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Edit(CourseModel model, bool continueEditing)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var courseExisting = await _courseService.GetCourseById(model.Id);
            var customer = await _workContext.GetCurrentCustomerAsync();

            if (courseExisting.InstructorGuid != customer.CustomerGuid && courseExisting.Id != model.Id)
                return await AccessDeniedDataTablesJson();

            var courseNew = courseExisting;

            if (ModelState.IsValid)
            {
                var course = model.ToEntity<Course>();
                courseNew = course;
                course.CreatedOnUtc = courseExisting.CreatedOnUtc;
                course.UpdatedOnUtc = DateTime.UtcNow;

                course.InstructorId = customer.Id;
                course.InstructorGuid = customer.CustomerGuid;

                await _courseService.InsertOrUpdateCourseAsync(course);

                //var existingSections = await _courseService.GetSectionsByCourseIdAsync(course.Id);



                //var lstSectionsToDelete = existingSections.Select(p => p.Id).ToList();

                //foreach (var sectionModel in model.Sections)
                //{
                //    if (sectionModel.Id > 0)
                //    {
                //        lstSectionsToDelete.Remove(sectionModel.Id);
                //    }

                //    var section = sectionModel.ToEntity<Section>();
                //    section.CourseId = course.Id;
                //    await _courseService.InsertOrUpdateSectionAsync(section);

                //    foreach (var lessonModel in sectionModel.Lessons)
                //    {
                //        var lesson = lessonModel.ToEntity<Lesson>();
                //        await _courseService.InsertOrUpdateLessonAsync(lesson);
                //        await _courseService.InsertOrUpdateSectionLessonAsync(new SectionLesson
                //        {
                //            CourseId = course.Id,
                //            DisplayOrder = lessonModel.DisplayOrder,
                //            LessonId = lesson.Id,
                //            IsFreeLesson = lessonModel.IsFreeLesson,
                //            SectionId = section.Id
                //        });

                //        if (lessonModel.LessonType == LessonType.Document)
                //        {
                //            foreach (var attachmentModel in lessonModel.Attachments)
                //            {
                //                var attachment = attachmentModel.ToEntity<Attachment>();
                //                attachment.InstructorId = customer.Id;
                //                attachment.InstructorGuid = customer.CustomerGuid;
                //                await _courseService.InsertOrUpdateAttachmentAsync(attachment);
                //                await _courseService.InsertOrUpdateLessonAttachmentAsync(new LessonAttachment
                //                {
                //                    AttachmentId = attachment.Id,
                //                    LessonId = lesson.Id
                //                });
                //            }
                //        }
                //        else if (lessonModel.LessonType == LessonType.Video)
                //        {
                //            if (lessonModel.Video != null)
                //            {
                //                var video = lessonModel.Video.ToEntity<Video>();
                //                await _courseService.InsertOrUpdateVideoAsync(video);
                //                lesson.VideoId = video.Id;
                //                await _courseService.InsertOrUpdateLessonAsync(lesson);
                //            }
                //        }
                //    }
                //}

                //await NormalizeSections(lstSectionsToDelete);

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = course.Id });
            }

            //prepare model
            model = await _courseModelFactory.PrepareCourseModelAsync(model, courseNew);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Delete(int id)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var courseExisting = await _courseService.GetCourseById(id);
            var customer = await _workContext.GetCurrentCustomerAsync();

            if (courseExisting.InstructorGuid != customer.CustomerGuid && courseExisting.Id != id)
                return await AccessDeniedDataTablesJson();

            await _courseService.DeleteCourseFullByIdAsync(courseExisting.Id);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("SimpleLMS.Course") + " "
                + await _localizationService.GetResourceAsync("SimpleLMS.Deleted"));

            return RedirectToAction("List");
        }


        public virtual async Task<IActionResult> CreateSection(int courseId)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return await AccessDeniedDataTablesJson();

            var courseExisting = await _courseService.GetCourseById(courseId);
            var customer = await _workContext.GetCurrentCustomerAsync();

            if (courseExisting.InstructorGuid != customer.CustomerGuid)
                return await AccessDeniedDataTablesJson();

            var model = await _courseModelFactory.PrepareSectionModelAsync(new SectionModel(), courseExisting, null);

            return PartialView("~/Plugins/Misc.SimpleLMS/Areas/Admin/Views/Course/_CreateOrUpdate.Section_CreateOrEdit.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> CreateSection(SectionModel sectionModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return await AccessDeniedDataTablesJson();

            if (sectionModel.Id > 0)
                return AccessDeniedView();

            var courseExisting = await _courseService.GetCourseById(sectionModel.CourseId);
            var customer = await _workContext.GetCurrentCustomerAsync();

            if (courseExisting.InstructorGuid != customer.CustomerGuid)
                return await AccessDeniedDataTablesJson();


            if (ModelState.IsValid)
            {
                var section = sectionModel.ToEntity<Section>();
                await _courseService.InsertOrUpdateSectionAsync(section);

                return Json(new { success = true });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    responseText = await _localizationService.GetResourceAsync("SimpleLMS.InvalidData")
                });
            }


        }


        public virtual async Task<IActionResult> EditSection(int sectionId)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return await AccessDeniedDataTablesJson();

            var sectionExisting = await _courseService.GetSectionByIdAsync(sectionId);
            var courseExisting = await _courseService.GetCourseById(sectionExisting.CourseId);
            var customer = await _workContext.GetCurrentCustomerAsync();

            if (courseExisting.InstructorGuid != customer.CustomerGuid)
                return await AccessDeniedDataTablesJson();

            var model = await _courseModelFactory.PrepareSectionModelAsync(new SectionModel(), courseExisting, sectionExisting);

            return PartialView("~/Plugins/Misc.SimpleLMS/Areas/Admin/Views/Course/_CreateOrUpdate.Section_CreateOrEdit.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> EditSection(SectionModel sectionModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return await AccessDeniedDataTablesJson();

            if (sectionModel.Id == 0)
                return AccessDeniedView();

            var courseExisting = await _courseService.GetCourseById(sectionModel.CourseId);
            var customer = await _workContext.GetCurrentCustomerAsync();

            if (courseExisting.InstructorGuid != customer.CustomerGuid)
                return await AccessDeniedDataTablesJson();


            if (ModelState.IsValid)
            {
                var section = sectionModel.ToEntity<Section>();
                await _courseService.InsertOrUpdateSectionAsync(section);

                return Json(new { success = true });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    responseText = await _localizationService.GetResourceAsync("SimpleLMS.InvalidData")
                });
            }


        }

        public virtual async Task<IActionResult> SortLessons(int sectionId)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var sectionExisting = await _courseService.GetSectionByIdAsync(sectionId);
            var courseExisting = await _courseService.GetCourseById(sectionExisting.CourseId);
            var customer = await _workContext.GetCurrentCustomerAsync();

            if (courseExisting.InstructorGuid != customer.CustomerGuid)
                return AccessDeniedView();

            var sectionLessons = await _courseService.GetSectionLessonsBySectionIdAsync(sectionExisting.Id);
            var lessons = await _courseService.GetLessonsBySectionIdAsync(sectionExisting.Id);

            var doModels = from sl in sectionLessons
                           select new DisplayOrderRecord
                           {
                               Id = sl.Id,
                               DisplayOrder = sl.DisplayOrder,
                               DisplayText = lessons.Where(p => p.Id == sl.LessonId).Select(l => l.Name).FirstOrDefault()
                           };

            var model = await _courseModelFactory.PrepareSortableEntityModelAsync(new SortableEntity(), doModels, SortRecordType.Lesson, sectionExisting.Id);

            return PartialView("~/Plugins/Misc.SimpleLMS/Areas/Admin/Views/Course/_CreateOrUpdate.Sortable.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> SortLessons(SortableEntity sortableEntity)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var sectionExisting = await _courseService.GetSectionByIdAsync(sortableEntity.ParentId);
            var courseExisting = await _courseService.GetCourseById(sectionExisting.CourseId);
            var customer = await _workContext.GetCurrentCustomerAsync();

            if (courseExisting.InstructorGuid != customer.CustomerGuid)
                return AccessDeniedView();

            var sectionLessons = await _courseService.GetSectionLessonsBySectionIdAsync(sectionExisting.Id);

            if (sectionLessons != null && sectionLessons.Count > 0)
            {

                for (int i = 0; i < sortableEntity.NewSortOrderValues.Count; i++)
                {
                    sortableEntity.NewSortOrderValues[i] = Regex.Match(sortableEntity.NewSortOrderValues[i], @"\d+").Value;
                }

                foreach (var sl in sectionLessons)
                {
                    sl.DisplayOrder = Array.FindIndex(sortableEntity.NewSortOrderValues.ToArray()
                        , x => x == sl.Id.ToString()) + 1;
                }

                await _courseService.UpdateSectionLessonsAsync(sectionLessons);

                return Json(new { Result = true });
            }

            return Json(new { Result = false });
        }

        public virtual async Task<IActionResult> SortSections(int courseId)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var courseExisting = await _courseService.GetCourseById(courseId);
            var customer = await _workContext.GetCurrentCustomerAsync();

            if (courseExisting.InstructorGuid != customer.CustomerGuid)
                return AccessDeniedView();

            var sections = await _courseService.GetSectionsByCourseIdAsync(courseExisting.Id);

            var inputModels = from s in sections
                              select new DisplayOrderRecord
                              {
                                  Id = s.Id,
                                  DisplayOrder = s.DisplayOrder,
                                  DisplayText = s.Title
                              };

            var model = await _courseModelFactory.PrepareSortableEntityModelAsync(new SortableEntity(), inputModels, SortRecordType.Section, courseExisting.Id);

            return PartialView("~/Plugins/Misc.SimpleLMS/Areas/Admin/Views/Course/_CreateOrUpdate.Sortable.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> SortSections(SortableEntity sortableEntity)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var courseExisting = await _courseService.GetCourseById(sortableEntity.ParentId);
            var customer = await _workContext.GetCurrentCustomerAsync();

            if (courseExisting.InstructorGuid != customer.CustomerGuid)
                return AccessDeniedView();


            var sections = await _courseService.GetSectionsByCourseIdAsync(sortableEntity.ParentId);

            if (sections != null && sections.Count > 0)
            {

                for (int i = 0; i < sortableEntity.NewSortOrderValues.Count; i++)
                {
                    sortableEntity.NewSortOrderValues[i] = Regex.Match(sortableEntity.NewSortOrderValues[i], @"\d+").Value;
                }

                foreach (var sl in sections)
                {
                    sl.DisplayOrder = Array.FindIndex(sortableEntity.NewSortOrderValues.ToArray()
                        , x => x == sl.Id.ToString()) + 1;

                    await _courseService.InsertOrUpdateSectionAsync(sl);
                }



                return Json(new { Result = true });
            }

            return Json(new { Result = false });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> DeleteSection(int sectionId)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return await AccessDeniedDataTablesJson();

            var section = await _courseService.GetSectionByIdAsync(sectionId);

            var courseExisting = await _courseService.GetCourseById(section.CourseId);
            var customer = await _workContext.GetCurrentCustomerAsync();

            if (courseExisting.InstructorGuid != customer.CustomerGuid)
                return await AccessDeniedDataTablesJson();

            await _courseService.DeleteSectionAsync(sectionId);

            return Json(new { Result = true });
        }

        public virtual async Task<IActionResult> CreateLesson(int sectionId, int courseId)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return await AccessDeniedDataTablesJson();

            var section = await _courseService.GetSectionByIdAsync(sectionId);
            var customer = await _workContext.GetCurrentCustomerAsync();
            var courseExisting = await _courseService.GetCourseById(section.CourseId);

            if (courseExisting.InstructorGuid != customer.CustomerGuid)
                return await AccessDeniedDataTablesJson();

            if (courseExisting.Id != courseId)
                return await AccessDeniedDataTablesJson();

            var model = await _courseModelFactory.PrepareLessonModelAsync(new LessonModel(), section, courseExisting, null);

            return PartialView("~/Plugins/Misc.SimpleLMS/Areas/Admin/Views/Course/_CreateOrUpdate.Lesson_CreateOrEdit.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> CreateLesson(LessonModel lessonModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            if (lessonModel.Id > 0)
                return AccessDeniedView();

            var section = await _courseService.GetSectionByIdAsync(lessonModel.SectionId);
            var customer = await _workContext.GetCurrentCustomerAsync();
            var courseExisting = await _courseService.GetCourseById(section.CourseId);

            if (courseExisting.InstructorGuid != customer.CustomerGuid)
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var videoId = 0;

                if (lessonModel.LessonType == LessonType.Video)
                {
                    var video = new Video();
                    video.Duration = lessonModel.Duration;
                    video.InstructorGuid = customer.CustomerGuid;
                    video.InstructorId = customer.Id;
                    video.VideoTypeId = (int)lessonModel.VideoType;
                    video.VideoIdFromProvider = lessonModel.VideoIdFromProvider;

                    await _courseService.InsertOrUpdateVideoAsync(video);
                    videoId = video.Id;
                }


                var lesson = lessonModel.ToEntity<Lesson>();



                if (lessonModel.LessonType == LessonType.Text)
                {
                    lesson.VideoId = null;
                }

                lesson.InstructorId = customer.Id;
                lesson.InstructorGuid = customer.CustomerGuid;

                lesson.DocumentId = null;
                lesson.PictureId = null;

                lesson.LessonTypeId = (int)lessonModel.LessonType;

                if (videoId > 0)
                    lesson.VideoId = videoId;

                await _courseService.InsertOrUpdateLessonAsync(lesson);

                var sectionLesson = new SectionLesson();
                sectionLesson.SectionId = lessonModel.SectionId;
                sectionLesson.LessonId = lesson.Id;
                sectionLesson.DisplayOrder = lessonModel.DisplayOrder;
                sectionLesson.IsFreeLesson = lessonModel.IsFreeLesson;
                sectionLesson.CourseId = lessonModel.CourseId;

                await _courseService.InsertOrUpdateSectionLessonAsync(sectionLesson);

                return Ok();
            }

            return BadRequest(ModelState);

        }


        public virtual async Task<IActionResult> EditLesson(int lessonId, int sectionId, int courseId)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return await AccessDeniedDataTablesJson();

            var lesson = await _courseService.GetLessonByIdAsync(lessonId);
            var customer = await _workContext.GetCurrentCustomerAsync();

            if (lesson.InstructorGuid != customer.CustomerGuid)
                return await AccessDeniedDataTablesJson();

            var lessonSections = await _courseService.GetLessonSectionByLessonIdAsync(lessonId);

            if (!lessonSections.Select(p => p.SectionId).Contains(sectionId))
                return await AccessDeniedDataTablesJson();

            var section = await _courseService.GetSectionByIdAsync(sectionId);

            var courseExisting = await _courseService.GetCourseById(section.CourseId);

            if (courseExisting.InstructorGuid != customer.CustomerGuid)
                return await AccessDeniedDataTablesJson();

            if (courseExisting.Id != courseId)
                return await AccessDeniedDataTablesJson();


            var model = await _courseModelFactory.PrepareLessonModelAsync(new LessonModel(), section, courseExisting, lesson);

            return PartialView("~/Plugins/Misc.SimpleLMS/Areas/Admin/Views/Course/_CreateOrUpdate.Lesson_CreateOrEdit.cshtml", model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> EditLesson(LessonModel lessonModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return await AccessDeniedDataTablesJson();

            if (lessonModel.Id == 0)
                return AccessDeniedView();


            var lesson = await _courseService.GetLessonByIdAsync(lessonModel.Id);
            var customer = await _workContext.GetCurrentCustomerAsync();
            if (lesson.InstructorGuid != customer.CustomerGuid)
                return await AccessDeniedDataTablesJson();

            var section = await _courseService.GetSectionByIdAsync(lessonModel.SectionId);
            var courseExisting = await _courseService.GetCourseById(section.CourseId);

            if (courseExisting.Id != lessonModel.CourseId)
                return await AccessDeniedDataTablesJson();

            if (courseExisting.InstructorGuid != customer.CustomerGuid)
                return await AccessDeniedDataTablesJson();

            if (ModelState.IsValid)
            {
                var videoId = 0;

                var videoIdToDelete = 0;

                if (lessonModel.LessonType == LessonType.Video)
                {
                    var video = await _courseService.GetVideoByLessonIdAsync(lessonModel.Id);

                    if (video == null)
                        video = new Video();

                    video.Duration = lessonModel.Duration;
                    video.InstructorGuid = customer.CustomerGuid;
                    video.InstructorId = customer.Id;
                    video.VideoTypeId = (int)lessonModel.VideoType;
                    video.VideoIdFromProvider = lessonModel.VideoIdFromProvider;

                    await _courseService.InsertOrUpdateVideoAsync(video);
                    videoId = video.Id;
                }
                else if (lessonModel.LessonType == LessonType.Text && lesson.LessonType == LessonType.Video)
                {
                    var video = await _courseService.GetVideoByLessonIdAsync(lessonModel.Id);
                    videoIdToDelete = video.Id;
                }

                lesson = lessonModel.ToEntity<Lesson>();

                if (lessonModel.LessonType == LessonType.Text)
                {
                    lesson.VideoId = null;
                }

                lesson.InstructorId = customer.Id;
                lesson.InstructorGuid = customer.CustomerGuid;

                lesson.DocumentId = null;
                lesson.PictureId = null;

                lesson.LessonTypeId = (int)lessonModel.LessonType;

                if (videoId > 0)
                    lesson.VideoId = videoId;

                await _courseService.InsertOrUpdateLessonAsync(lesson);

                if (videoIdToDelete > 0)
                    await _courseService.DeleteVideoAsync(videoIdToDelete);

                return Ok();
            }

            return BadRequest(ModelState);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> DeleteLesson(int lessonId, int sectionId, int courseId)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var lesson = await _courseService.GetLessonByIdAsync(lessonId);
            var customer = await _workContext.GetCurrentCustomerAsync();
            if (lesson.InstructorGuid != customer.CustomerGuid)
                return AccessDeniedView();

            var section = await _courseService.GetSectionByIdAsync(sectionId);
            var courseExisting = await _courseService.GetCourseById(section.CourseId);

            if (courseExisting.Id != courseId)
                return AccessDeniedView();

            if (courseExisting.InstructorGuid != customer.CustomerGuid)
                return AccessDeniedView();

            await _courseService.DeleteSectionLessonAsync(sectionId, lessonId);

            await _customerActivityService.InsertActivityAsync(customer, "DeleteLesson",
               await _localizationService.GetResourceAsync("SimpleLMS.Lesson") + ": " + lesson.Id + ". " + lesson.Name +
               await _localizationService.GetResourceAsync("SimpleLMS.Lesson") + ": " + section.Id + ". " + section.Title);

            //_notificationService.SuccessNotification(await _localizationService.GetResourceAsync("SimpleLMS.Lesson") + ' '
            //    + await _localizationService.GetResourceAsync("SimpleLMS.Deleted"));

            return Json(new { Result = true });

        }



        private async Task NormalizeSections(List<int> lstSectionsToDelete)
        {
            foreach (var secId in lstSectionsToDelete)
            {
                //var section = await _courseService.GetSectionByIdAsync(secId);
                //var lessons = await _courseService.GetLessonsBySectionIdAsync(secId);

                //foreach (var lesson in lessons)
                //{
                //    await _courseService.DeleteLessonAttachmentsAsync(lesson, secId);
                //    await _courseService.DeleteLessonVideosAsync(lesson, secId);
                //}

                await _courseService.DeleteSectionLessonsBySecIdAsync(secId);
                await _courseService.DeleteSectionAsync(secId);
            }
        }
    }
}
