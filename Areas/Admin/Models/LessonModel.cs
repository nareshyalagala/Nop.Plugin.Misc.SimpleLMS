using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Misc.SimpleLMS.Domains;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.SimpleLMS.Areas.Admin.Models
{
    public partial record LessonModel : BaseNopEntityModel
    {
        public LessonModel()
        {
            //Video = new VideoModel();
            Attachments = new List<AttachmentModel>();
            AvailableLessonTypes = new List<SelectListItem>();
            AvailableVideoTypes = new List<SelectListItem>();
            AvailableAttachmentTypes = new List<SelectListItem>();
        }

        [NopResourceDisplayName("SimpleLMS.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("SimpleLMS.LessonType")]
        public LessonType LessonType { get; set; }

        [NopResourceDisplayName("SimpleLMS.LessonContents")]
        public string LessonContents { get; set; }

        [NopResourceDisplayName("SimpleLMS.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [NopResourceDisplayName("SimpleLMS.Instructor")]
        public int InstructorName { get; set; }

        [NopResourceDisplayName("SimpleLMS.IsFree")]
        public bool IsFreeLesson { get; set; }

        public VideoModel Video { get; set; }

        [NopResourceDisplayName("SimpleLMS.SectionName")]
        public int SectionId { get; set; }

        [NopResourceDisplayName("SimpleLMS.CourseName")]
        public int CourseId { get; set; }

        [NopResourceDisplayName("SimpleLMS.VideoType")]
        public VideoType VideoType { get; set; }

        [NopResourceDisplayName("SimpleLMS.VideoIdFromProvider")]
        public string VideoIdFromProvider { get; set; }

        [NopResourceDisplayName("SimpleLMS.Duration")]
        public int Duration { get; set; }

        [NopResourceDisplayName("SimpleLMS.VideoUrl")]
        public string VideoUrl { get; set; }

        [NopResourceDisplayName("SimpleLMS.VideoEmbedCode")]
        public string VideoEmbedCode { get; set; }

        [NopResourceDisplayName("SimpleLMS.AttachmentType")]
        public AttachmentType AttachmentType { get; set; }

        [NopResourceDisplayName("SimpleLMS.Attachments")]
        public IList<AttachmentModel> Attachments { get; set; }

        [NopResourceDisplayName("SimpleLMS.LessonType")]
        public IList<SelectListItem> AvailableLessonTypes { get; set; }

        [NopResourceDisplayName("SimpleLMS.VideoType")]
        public IList<SelectListItem> AvailableVideoTypes { get; set; }

        [NopResourceDisplayName("SimpleLMS.AttachmentType")]
        public IList<SelectListItem> AvailableAttachmentTypes { get; set; }
    }
}
