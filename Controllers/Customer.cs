using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Misc.SimpleLMS.Models;
using Nop.Plugin.Misc.SimpleLMS.Services;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Security;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Misc.SimpleLMS.Controllers
{
    public partial class CustomerController : BaseController
    {
        private readonly CourseService _courseService;
        private readonly ISettingService _settingService;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IPermissionService _permissionService;
        private readonly ILocalizationService _localizationService;
        private readonly ICustomerService _customerService;
        private readonly CourseModelFactory _courseModelFactory;


        public CustomerController(IAclService aclService, IProductService productService, ISettingService settingService
            , IWorkContext workContext, IStoreContext storeContext
            , IPermissionService permissionService, ILanguageService languageService
            , ILocalizationService localizationService, ICustomerActivityService customerActivityService, ICustomerService customerService
            , CourseModelFactory courseModelFactory
            , CourseService courseService) : base()
        {
            _settingService = settingService;
            _workContext = workContext;
            _storeContext = storeContext;
            _permissionService = permissionService;
            _localizationService = localizationService;
            _customerService = customerService;
            _courseModelFactory = courseModelFactory;
            _courseService = courseService;
        }

        public virtual async Task<IActionResult> Courses()
        {
            if (!await _customerService.IsRegisteredAsync(await _workContext.GetCurrentCustomerAsync()))
                return Challenge();

            var customer = await _workContext.GetCurrentCustomerAsync();

            //prepare model
            var model = await _courseModelFactory.PrepareCourseSearchModelAsync(new CourseSearchModel());

            return View("~/Plugins/Misc.SimpleLMS/Views/Customer/List.cshtml", model);
        }


        //public virtual async Task<I>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> CourseOverviewList([FromBody]CourseSearchModel searchModel)
        {
            if (!await _customerService.IsRegisteredAsync(await _workContext.GetCurrentCustomerAsync()))
                return Challenge();


            //prepare model
            var model = await _courseModelFactory.PrepareCourseOverviewListModelAsync(searchModel);

            return PartialView("~/Plugins/Misc.SimpleLMS/Views/Customer/_MyCourseList.cshtml", model);
        }

        public virtual async Task<IActionResult> CoursesDetails(int courseId)
        {
            if (!await _customerService.IsRegisteredAsync(await _workContext.GetCurrentCustomerAsync()))
                return Challenge();

            var customer = await _workContext.GetCurrentCustomerAsync();
            if (_courseService.IsUserCourse(courseId, customer.Id))
            {
                var courseExisting = await _courseService.GetCourseByProductIdAsync(courseId);
                var lessonProgresses = await _courseService.GetCourseProgressByUserId(courseId, customer.Id);
                var model = await _courseModelFactory.PrepareCourseDetailModelAsync(new CourseDetail(), courseExisting, lessonProgresses);
                return View("~/Plugins/Misc.SimpleLMS/Views/Customer/CoursesDetails.cshtml", model);
            }
            else
            {
                return Unauthorized();
            }


        }

        public virtual async Task<IActionResult> LessonContent(int lessonId, int courseId)
        {
            if (!await _customerService.IsRegisteredAsync(await _workContext.GetCurrentCustomerAsync()))
                return Challenge();


            var customer = await _workContext.GetCurrentCustomerAsync();
            if (_courseService.IsUserCourse(courseId, customer.Id))
            {
                var lessonExisting = await _courseService.GetLessonByCourseIdAndLessonIdAsync(courseId, lessonId);
                var model = await _courseModelFactory.PrepareLessonModelAsync(new LessonDetail(), lessonExisting);

                var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
                var simpleLMSSettings = await _settingService.LoadSettingAsync<SimpleLMSSettings>(storeScope);
                ViewData["simpleLMSSettings"] = simpleLMSSettings;


                return View("~/Plugins/Misc.SimpleLMS/Views/Customer/_LessonContent.cshtml", model);
            }
            else
            {
                return Unauthorized();
            }

        }


        public virtual async Task<IActionResult> UpdateLessonStatus(LessonStatusModel lessonStatus)
        {
            if (!await _customerService.IsRegisteredAsync(await _workContext.GetCurrentCustomerAsync()))
                return Challenge();


            var customer = await _workContext.GetCurrentCustomerAsync();
            if (_courseService.IsUserCourse(lessonStatus.CourseId, customer.Id)
                && _courseService.ValidateCourseSectionLessonCombination(lessonStatus.LessonId, lessonStatus.SectionId, lessonStatus.CourseId))
            {

                await _courseService.InsertOrUpdateLessonProgressByCourseLessonAndUser(lessonStatus.CourseId, customer.Id, lessonStatus.LessonId, lessonStatus.IsCompleted, lessonStatus.CurrentlyAt);

               

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

    }
}
