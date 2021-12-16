using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _8Hours.Enums;

namespace _8Hours.Models
{
    internal class AppConfig
    {
        public WindowOrientationEnum WindowOrientation { get; set; }
        public JobTypeEnum? JobType { get; set; }
    }
}
