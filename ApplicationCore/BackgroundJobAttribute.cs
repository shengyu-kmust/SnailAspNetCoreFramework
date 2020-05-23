using ApplicationCore.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore
{
    public class BackgroundJobAttribute : Attribute
    {
        /// <summary>
        /// 定时任务的类型
        /// </summary>
        public EBackgroundJobType JobType { get; set; }
        /// <summary>
        /// 定时任务类型为Schedule时，延迟执行的时间
        /// </summary>
        public string DelayTimeSpan { get; set; }
        /// <summary>
        /// 定时任务类型为Recurring时，循环执行的时间配置
        /// </summary>
        public string Cron { get; set; }
    }
}
