using ApplicationCore;
using ApplicationCore.Enums;
using ApplicationCore.Services;
using Microsoft.EntityFrameworkCore;
using Snail.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Services
{
    /// <summary>
    /// 队列数据提供服务
    /// </summary>
    public class QueueDataService : IQueueDatasService
    {
        private UniversalQueueContext _db;
        public QueueDataService(UniversalQueueContext db)
        {
            _db = db;
        }

     

        public List<Queue> GetQueueInternal(DateTime date,ETimeSeg? timeSeg,EQueueType? queueType,List<string> userIds,List<string> targetIds)
        {
            var query=_db.Queue.AsNoTracking().Where(a => a.Date.Date == date.Date && !a.IsDeleted).WhereIf(timeSeg.HasValue, a => a.TimeSeg == timeSeg).WhereIf(queueType.HasValue, a => a.QueueType == queueType);
            if (userIds!=null && userIds.Count>0)
            {
                // 对==和contains进行切换
                query = query.WhereIfElse(userIds.Count == 1, a => a.UserId == userIds[0], a => userIds.Contains(a.UserId));
            }
            if (targetIds!=null && targetIds.Count>0)
            {
                query= query.WhereIfElse(targetIds.Count == 1, a => a.TargetId == targetIds[0], a => targetIds.Contains(a.TargetId));
            }
            return query.ToList();
        }
    }
}
