using System;
using System.Collections.Generic;
using _8Hours.Enums;

namespace _8Hours.Models
{
    internal class JobReport
    {
        public List<DateReport> DayReport { get; set; }
    }
    internal class DateReport
    {
        /// <summary>
        /// format yyyy-MM-dd
        /// </summary>
        public string Day { get; set; }
        public JobReportDetail Details { get; set; }
    }

    internal class JobReportDetail
    {
        public JobTypeEnum JobType { get; set; }
        public double TimeTotal { get; set; }
    }
}
