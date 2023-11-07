using System;
using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Stores;

namespace Nop.Plugin.Misc.SimpleLMS.Domains
{
    /// <summary>
    /// Represents Course
    /// </summary>
    public partial class Course : BaseEntity, ILocalizedEntity, IStoreMappingSupported, ISoftDeletedEntity
    {
        /// <summary>
        /// Gets or sets associated product id
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets course name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets LimitedToStores
        /// </summary>
        public bool LimitedToStores { get; set; }

        /// <summary>
        /// Gets or sets Deleted
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Instructor Id
        /// </summary>
        public int InstructorId { get; set; }

        /// <summary>
        /// Instructor Guid
        /// </summary>
        public Guid InstructorGuid { get; set; }


        /// <summary>
        /// Gets or sets the date and time of product creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of product update
        /// </summary>
        public DateTime UpdatedOnUtc { get; set; }
    }
}
