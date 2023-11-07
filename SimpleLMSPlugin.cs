using Microsoft.AspNetCore.Routing;
using Nop.Services.Cms;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Menu;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.SimpleLMS
{
    /// <summary>
    /// Rename this file and change to the correct type
    /// </summary>
    public class SimpleLMSPlugin : BasePlugin, IMiscPlugin, IAdminMenuPlugin
    {
        private readonly ILocalizationService _localizationService;
        private readonly ISettingService _settingService;

        public SimpleLMSPlugin(ILocalizationService localizationService, ISettingService settingService)
        {
            _localizationService = localizationService;
            _settingService = settingService;
        }

        public override async Task InstallAsync()
        {

            await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
            {
                ["SimpleLMS.CourseList"] = "Courses",
                ["SimpleLMS.InstructorId"] = "Instructor Id",
                ["SimpleLMS.SearchCourseName"] = "Name",
                ["SimpleLMS.Name"] = "Name",
                ["SimpleLMS.Instructor"] = "Instructor",
                ["SimpleLMS.Sections"] = "Sections",
                ["SimpleLMS.Lessons"] = "Lessons",
                ["SimpleLMS.EnrolledStudents"] = "Enrolled Students",
                ["SimpleLMS.Status"] = "Status",
                ["SimpleLMS.Price"] = "Price",
                ["SimpleLMS.Category"] = "Category",
                ["SimpleLMS.ProductName"] = "Product",
                ["SimpleLMS.Info"] = "Info",
                ["SimpleLMS.Course"] = "Course",

                ["SimpleLMS.Section"] = "Section",
                ["SimpleLMS.Select"] = "Select",
                ["SimpleLMS.Add"] = "Add",
                ["SimpleLMS.Create"] = "Create",
                ["SimpleLMS.Update"] = "Update",
                ["SimpleLMS.Edit"] = "Edit",
                ["SimpleLMS.Lesson"] = "Lesson",
                ["SimpleLMS.Sort"] = "Sort",
                ["SimpleLMS.Delete"] = "Delete",
                ["SimpleLMS.BackToList"] = "Back To List",
                ["SimpleLMS.CourseContent"] = "Course Content",
                ["SimpleLMS.SaveCourseToAddContentMessage"] = "Save course to add content",
                ["SimpleLMS.IsFree"] = "Free",
                ["SimpleLMS.LessonType"] = "Lesson Type",
                ["SimpleLMS.LessonContents"] = "Lesson Contents",
                ["SimpleLMS.VideoType"] = "Video Type",
                ["SimpleLMS.VideoIdFromProvider"] = "Video Id From Provider",
                ["SimpleLMS.VideoEmbedCode"] = "Video EmbedCode",
                ["SimpleLMS.VideoUrl"] = "Video Url",
                ["SimpleLMS.Duration"] = "Duration",
                ["SimpleLMS.AttachmentType"] = "Attachment Type",
                ["SimpleLMS.NoLessonsAvailable"] = "No lessons available.",
                ["SimpleLMS.NoSectionsAvailable"] = "No sections available.",
                ["SimpleLMS.Deleted"] = "Deleted",
                ["SimpleLMS.MyCourses"] = "My Courses",
                ["SimpleLMS.MyCourses.NoCoursesToShow"] = "No courses to show.",
                ["SimpleLMS.SearchCourses"] = "Search courses.",
                ["SimpleLMS.Minutes"] = "minutes",
                ["SimpleLMS.Completed"] = "completed",
                ["SimpleLMS.Of"] = "of",
                ["SimpleLMS.Start"] = "Start",
                ["SimpleLMS.Resume"] = "Resume",
                ["SimpleLMS.Settings"] = "Settings",
                ["SimpleLMS.SimpleLMS"] = "Simple LMS",

                ["plugins.simplelms.configuration.pagetitle"] = "Configurations",
                ["simplelms.configuration.youtube"] = "YouTube",
                ["Plugins.SimpleLMS.Configuration.YouTubeApiKey.Text"] = "YouTube Api Key",
                ["simplelms.configuration.vimeo"] = "Vimeo",
                ["Plugins.SimpleLMS.Configuration.VimeoAppId.Text"] = "Vimeo App Id",
                ["Plugins.SimpleLMS.Configuration.VimeoClient.Text"] = "Vimeo Client Id",
                ["Plugins.SimpleLMS.Configuration.VimeoSecret.Text"] = "Vimeo Client secrets",
                ["Plugins.SimpleLMS.Configuration.VimeoAccess.Text"] = "Vimeo Access Token",
                ["SimpleLMS.Configuration.VdoCipher"] = "VdoCipher",
                ["Plugins.SimpleLMS.Configuration.VdoCipherKey.Text"] = "VdoCipher Key",

            });

            await base.InstallAsync();
        }



        public override async Task UninstallAsync()
        {
            await _settingService.DeleteSettingAsync<SimpleLMSSettings>();

            await _localizationService.DeleteLocaleResourcesAsync("SimpleLMS");

            await base.UninstallAsync();
        }

        public async Task ManageSiteMapAsync(SiteMapNode rootNode)
        {
            var menuItem = new SiteMapNode()
            {
                SystemName = "SimpleLMS.Courses",
                Title = await _localizationService.GetResourceAsync("SimpleLMS.CourseList"),
                ControllerName = "Course",
                ActionName = "List",
                Visible = true,
                IconClass = "far fa-dot-circle",
                RouteValues = new RouteValueDictionary() { { "area", "Admin" } },
            };

            var menuItem1 = new SiteMapNode()
            {
                SystemName = "SimpleLMS.configuration",
                Title = await _localizationService.GetResourceAsync("SimpleLMS.Settings"),
                ControllerName = "Settings",
                ActionName = "Configure",
                Visible = true,
                IconClass = "far fa-dot-circle",
                RouteValues = new RouteValueDictionary() { { "area", "Admin" } },
            };
            var pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "SimpleLMS.Root");



            if (pluginNode != null)
            {
                pluginNode.ChildNodes.Add(menuItem);
                pluginNode.ChildNodes.Add(menuItem1);
            }
            else
            {
                var pluginMenuRoot = new SiteMapNode()
                {
                    SystemName = "SimpleLMS.Root",
                    Title = await _localizationService.GetResourceAsync("SimpleLMS.SimpleLMS"),
                    ControllerName = "",
                    //ActionName = "",
                    Visible = true,
                    IconClass = "fa fa-graduation-cap"
                    //RouteValues = new RouteValueDictionary() { { "area", "Admin" } },
                };

                pluginMenuRoot.ChildNodes.Add(menuItem);
                pluginMenuRoot.ChildNodes.Add(menuItem1);
                rootNode.ChildNodes.Add(pluginMenuRoot);

                //rootNode.ChildNodes.Add(menuItem);
            }

        }
    }
}
