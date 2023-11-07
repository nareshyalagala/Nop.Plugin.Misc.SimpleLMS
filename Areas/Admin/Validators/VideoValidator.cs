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
    public partial class VideoValidator : BaseNopValidator<VideoModel>
    {
        public VideoValidator(ILocalizationService localizationService, INopDataProvider dataProvider)
        {
            RuleFor(x => x.Duration).NotNull();
            RuleFor(x => x.VideoType).NotNull();


            RuleFor(m => m.VideoUrl).NotEmpty().When(m => string.IsNullOrEmpty(m.VideoIdFromProvider) && string.IsNullOrEmpty(m.VideoEmbedCode));
            RuleFor(m => m.VideoIdFromProvider).NotEmpty().When(m => string.IsNullOrEmpty(m.VideoUrl) && string.IsNullOrEmpty(m.VideoEmbedCode));
            RuleFor(m => m.VideoEmbedCode).NotEmpty().When(m => string.IsNullOrEmpty(m.VideoIdFromProvider) && string.IsNullOrEmpty(m.VideoUrl));

            SetDatabaseValidationRules<Video>(dataProvider);
        }
    }
}
