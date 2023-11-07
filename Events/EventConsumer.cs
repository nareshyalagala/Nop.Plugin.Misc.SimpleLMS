using System;
using System.Threading.Tasks;
using Nop.Services.Events;
using Nop.Services.Localization;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Models;
using Nop.Web.Models.Customer;

namespace Nop.Plugin.Misc.SimpleLMS.Events
{
    public class EventConsumer : IConsumer<ModelPreparedEvent<BaseNopModel>>
    {
        private readonly ILocalizationService _localizationService;
        public EventConsumer(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        public async Task HandleEventAsync(ModelPreparedEvent<BaseNopModel> eventMessage)
        {
            if (eventMessage.Model is not CustomerNavigationModel navigationModel)
                return;

            navigationModel.CustomerNavigationItems.Insert(0, new CustomerNavigationItemModel
            {
                RouteName = SimpleLMSDefaults.CustomerMyCoursers,
                ItemClass = "my-courses",
                Tab = SimpleLMSDefaults.CustomerMyCoursesMenuTab,
                Title = await _localizationService.GetResourceAsync("SimpleLMS.MyCourses")

            }); ;
        }
    }
}

