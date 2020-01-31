using ApplicationCore.Entity;
using DotNetCore.CAP;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Snail.Core.Interface;
using System;
using System.Collections.Generic;
using Web.DTO.Sample;
using Web.Filter;

namespace Web.Controllers
{
    public class SampleEntityController : CRUDController<SampleEntity, SampleEntitySourceDto, SampleEntityResultDto, SampleEntitySaveDto, SampleEntityQueryDto>
    {
        private EntityCacheManager _entityCache;
        private IServiceProvider _serviceProvider;
        public SampleEntityController(ICRUDService<SampleEntity, SampleEntitySourceDto, string> CRUDService, EntityCacheManager entityCache, IServiceProvider serviceProvider) : base(CRUDService)
        {
            _serviceProvider = serviceProvider;
            _entityCache = entityCache;
        }

        [HttpGet]
        public List<SampleEntity> GetAllSampleEntityCache()
        {
            return _entityCache.Get<SampleEntity>();
        }
        [CacheFilter(ExpireMinute = 1)]
        public List<SampleEntity> GetAllSampleByActionFilter()
        {
            return _entityCache.Get<SampleEntity>();
        }

        [CacheFilter(ExpireMinute = 1)]
        public List<SampleEntity> GetAllSampleByActionFilter2()
        {
            return _entityCache.Get<SampleEntity>();
        }
        public void EventSub(string msg)
        {
            Console.WriteLine($"cap.event.test4:{msg}");
        }
    }
}
