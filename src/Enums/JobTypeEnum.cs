using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiuLing.CommonLibs.ExtensionAttributes;

namespace _8Hours.Enums
{
    internal enum JobTypeEnum
    {
        [Description("工作")]
        Work = 1,
        [Description("学习")]
        Study = 2,
        [Description("摸鱼")]
        Idle = 3
    }
}
