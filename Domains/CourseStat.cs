using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;

namespace Nop.Plugin.Misc.SimpleLMS.Domains
{
    public class CourseStat : BaseEntity
    {
        public int EnrolledStudents { get; set; }
        public int Sections { get; set; }
        public int Lessons { get; set; }
    }
}
