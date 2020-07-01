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
        public List<SampleEntity> GetSampleCache()
        {
            return GetEntityCache<SampleEntity, SampleEntity>();
        }

        public void ClearSampleCache()
        {
            ClearEntityCache<SampleEntity, SampleEntity>();
        }
    }
}
