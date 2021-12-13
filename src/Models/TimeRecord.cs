using _8Hours.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8Hours.Models
{
    [Table("TimeRecord")]
    internal class TimeRecord
    {
        [Key]
        public int Id { get; set; }
        public JobTypeEnum JobType { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
