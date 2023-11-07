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
    public partial class AttachmentValidator : BaseNopValidator<AttachmentModel>
    {
        public AttachmentValidator(ILocalizationService localizationService, INopDataProvider dataProvider)
        {
            RuleFor(x => x.AttachmentType).NotNull();
            RuleFor(x => x.Name).NotNull();
            RuleFor(x => x.VirtualPath).NotNull();

            SetDatabaseValidationRules<Attachment>(dataProvider);
        }
    }
}
