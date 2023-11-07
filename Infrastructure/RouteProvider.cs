using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;
using Nop.Web.Infrastructure;

namespace Nop.Plugin.Misc.SimpleLMS.Infrastructure
{
    public partial class RouteProvider : BaseRouteProvider, IRouteProvider
    {
        public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
        {
            var lang = GetLanguageRoutePattern();

            endpointRouteBuilder.MapControllerRoute(name: SimpleLMSDefaults.AdminCourseListRoute,
         pattern: $"{lang}/Admin/Course/List",
         defaults: new { controller = "CourseController", action = "Index", area = "Admin" });

            endpointRouteBuilder.MapControllerRoute(name: SimpleLMSDefaults.AdminCourseListJsonRoute,
        pattern: $"{lang}/Admin/Course/CourseList",
        defaults: new { controller = "CourseController", action = "CourseList", area = "Admin" });

            endpointRouteBuilder.MapControllerRoute(name: SimpleLMSDefaults.AdminCourseCreateSectionRoute,
        pattern: $"{lang}/Admin/Course/CreateSection",
        defaults: new { controller = "CourseController", action = "CreateSection", area = "Admin" });


            //customer account links
            endpointRouteBuilder.MapControllerRoute(name: SimpleLMSDefaults.CustomerMyCoursers,
                pattern: $"{lang}/customer/mycourses",
                defaults: new { controller = "Customer", action = "Courses" });

            endpointRouteBuilder.MapControllerRoute(name: SimpleLMSDefaults.CustomerMyCoursers,
               pattern: $"{lang}/customer/searchmycourses",
               defaults: new { controller = "Customer", action = "CourseOverviewList" });

            endpointRouteBuilder.MapControllerRoute(name: SimpleLMSDefaults.CustomerCoursesDetails,
                pattern: $"{lang}/coursesdetail/{{productid:min(0)}}",
               defaults: new { controller = "Customer", action = "CoursesDetails" });

            endpointRouteBuilder.MapControllerRoute(name: SimpleLMSDefaults.CustomerLessonVideo,
               pattern: $"{lang}/videocontent/{{lessionId:min(0)}}",
              defaults: new { controller = "Customer", action = "VideoContent" });


        }

        public int Priority => 999;
    }
}
