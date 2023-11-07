using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Nop.Data;
using Nop.Plugin.Misc.SimpleLMS.Areas.Admin.Models;
using Nop.Plugin.Misc.SimpleLMS.Domains;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Misc.SimpleLMS.Areas.Admin.Validators
{
    public partial class SectionValidator : BaseNopValidator<SectionModel>
    {
        public SectionValidator(ILocalizationService localizationService, INopDataProvider dataProvider)
        {
            RuleFor(x => x.Title).NotNull();
            RuleFor(x => x.DisplayOrder).NotNull();
            RuleFor(x => x.CourseId).NotNull();

            SetDatabaseValidationRules<Section>(dataProvider);
        }
    }
}
