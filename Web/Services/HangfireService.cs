using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Services
{
    public class HangfireService
    {
        public void Init()
        {
            Test(new Dictionary<string, string>());
        }

        #region 定时任务
        private void Test(Dictionary<string, string> configDic)
        {

            //RecurringJob.AddOrUpdate<ITrainService>("UpdateTrainsFromTranSystem", a => a.UpdateTrainsFromTranSystem(), configDic.ContainsKey("UpdateTrainsFromTranSystem") ? configDic["UpdateTrainsFromTranSystem"] : CreateDefaultCronExpression());//每天1点执行一次培训系统大纲同步
        }

        #endregion


        public void Start(List<EHangfireJob> jobIds)
        {
            var configDic = new Dictionary<string, string>();//
            jobIds.ForEach(jobid =>
            {
                switch (jobid)
                {
                    case EHangfireJob.Test:
                        Test(configDic);
                        break;
                    default:
                        break;
                }
            });
        }

        public void Stop(List<EHangfireJob> jobIds)
        {
            jobIds.ForEach(jobId =>
            {
                RecurringJob.RemoveIfExists(jobId.ToString());
            });
        }


        /// <summary>
        /// 随机创建0:0点到6:0点的时间
        /// </summary>
        /// <remarks>如果用户没有设置定时任务的执行时间，则随机在0点到6点执行</remarks>
        /// <returns></returns>
        private string CreateDefaultCronExpression()
        {
            var randomTimeSpan = new TimeSpan(0, 0, 0) + TimeSpan.FromMinutes(new Random().Next(360));
            return Cron.Daily(randomTimeSpan.Hours, randomTimeSpan.Minutes);
        }


    }

    public enum EHangfireJob
    {
        Test
    }
}
