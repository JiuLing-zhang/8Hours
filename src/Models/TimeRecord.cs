using _8Hours.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8Hours.Models
{
    internal class TimeRecord
    {
        public JobTypeEnum RecordType { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
