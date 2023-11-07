using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Misc.SimpleLMS.Areas.Admin.Validators;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using FluentValidation.Validators;
using FluentValidation;

namespace Nop.Plugin.Misc.SimpleLMS.Areas.Admin.Models
{
    public partial record CourseModel : BaseNopEntityModel
    {
        public CourseModel()
        {
            Sections = new List<SectionModel>();
            AvailableProducts = new List<SelectListItem>();
        }

        [NopResourceDisplayName("SimpleLMS.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("SimpleLMS.ProductName")]
        public string ParentProductName { get; set; }

        [NopResourceDisplayName("SimpleLMS.ProductName")]
        public int ProductId { get; set; }
        public IList<SelectListItem> AvailableProducts { get; set; }

        [NopResourceDisplayName("SimpleLMS.Instructor")]
        public string Instructor { get; set; }

        [NopResourceDisplayName("SimpleLMS.Instructor")]
        public string Category { get; set; }

        [NopResourceDisplayName("SimpleLMS.Sections")]
        public int SectionsTotal { get; set; }

        [NopResourceDisplayName("SimpleLMS.Lessons")]
        public int LessonsTotal { get; set; }

        [NopResourceDisplayName("SimpleLMS.EnrolledStudents")]
        public int EnrolledStudents { get; set; }

        [NopResourceDisplayName("SimpleLMS.Status")]
        public string Status { get; set; }

        [NopResourceDisplayName("SimpleLMS.Price")]
        public decimal Price { get; set; }

        [NopResourceDisplayName("SimpleLMS.CreatedOn")]
        public DateTime CreatedOnUtc { get; set; }

        [NopResourceDisplayName("SimpleLMS.UpdatedOn")]
        public DateTime UpdatedOnUtc { get; set; }

        public IList<SectionModel> Sections { get; set; }


    }
}
