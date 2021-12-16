using System;
using System.Collections.Generic;
using _8Hours.Enums;

namespace _8Hours.Models
{
    internal class JobReport
    {
        public List<DateReport> DayReport { get; set; } = null!;
    }
    internal class DateReport
    {
        /// <summary>
        /// format yyyy-MM-dd
        /// </summary>
        public string Day { get; set; } = null!;

        public JobReportDetail Details { get; set; } = null!;
    }

    internal class JobReportDetail
    {
        public JobTypeEnum JobType { get; set; }
        public double TimeTotal { get; set; }
    }
}
