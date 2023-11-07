using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Plugin.Misc.SimpleLMS.Infrastructure
{
    public class ViewLocationExpander : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context)
        {
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            if (context.AreaName == "Admin")
            {
                viewLocations = new[] { $"/Plugins/Misc.SimpleLMS/Areas/Admin/Views/{context.ControllerName}/{context.ViewName}.cshtml" }.Concat(viewLocations);
            }
            //else if (context.AreaName == null && context.ViewName == "Components/CustomerNavigation/Default")
            //{
            //    viewLocations = new[] { $"/Plugins/Misc.SimpleLMS/Views/Shared/Components/CustomCustomerNavigation/Default.cshtml" }.Concat(viewLocations);
            //}
            else
            {
                viewLocations = new[] { $"/Plugins/Misc.SimpleLMS/Views/{context.ControllerName}/{context.ViewName}.cshtml"
                }.Concat(viewLocations);
            }

            return viewLocations;
        }
    }
}




