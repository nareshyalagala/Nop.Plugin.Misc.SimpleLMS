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
    public partial class LessonValidator : BaseNopValidator<LessonModel>
    {
        public LessonValidator(ILocalizationService localizationService, INopDataProvider dataProvider)
        {
            RuleFor(x => x.DisplayOrder).NotNull().GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.LessonType).NotEmpty();

            RuleFor(x => x.AttachmentType).NotEmpty().When(x => x.LessonType == LessonType.Document);
            RuleFor(x => x.Attachments).Must(coll => coll.Any()).When(x => x.LessonType == LessonType.Document);

            RuleFor(x => x.CourseId).NotNull().GreaterThan(0);

            RuleFor(x => x.Duration).NotNull().When(x => x.LessonType == LessonType.Video)
                .GreaterThan(0).When(x => x.LessonType == LessonType.Video);

            RuleFor(x => x.IsFreeLesson).NotNull();
            RuleFor(x => x.LessonContents).Length(1, 4000).When(x => x.LessonType == LessonType.Text);
            RuleFor(x => x.SectionId).NotNull();
            RuleFor(x => x.VideoIdFromProvider).NotEmpty().When(x => x.LessonType == LessonType.Video);

            RuleFor(x => x.VideoType).NotEmpty().When(x => x.LessonType == LessonType.Video);

            SetDatabaseValidationRules<Lesson>(dataProvider);
        }
    }
}
