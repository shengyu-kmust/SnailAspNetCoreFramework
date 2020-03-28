using ApplicationCore.Dtos;
using ApplicationCore.Entity;
using ApplicationCore.IServices;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service
{
    public class SampleEntityService : BaseService<SampleEntity>, ISampleEntityService
    {
        public SampleEntityService(ServiceContext serviceContext) : base(serviceContext)
        {
        }

        public override IQueryable<TSource> GetQueryable<TSource>()
        {
            if (typeof(TSource) == typeof(SampleEntity))
            {
                return (IQueryable<TSource>)db.Set<SampleEntity>();
            }
            else if (typeof(TSource) == typeof(SampleEntitySourceDto))
            {
                return null;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
