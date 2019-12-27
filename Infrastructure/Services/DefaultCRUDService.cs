﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Snail.Core.Entity;
using Snail.CRUD;
using Snail.DAL;

namespace Infrastructure.Services
{
    public class DefaultCRUDService<TEntity, TQuerySource> : EFCRUDService<TEntity, TQuerySource, string> where TEntity : class, IEntityId<string>
    {
        private IQueryableSourceService _queryableSourceService;
        private readonly IHttpContextAccessor _context;

        public DefaultCRUDService(UniversalQueueContext db, IMapper mapper, IQueryableSourceService queryableSourceService, IHttpContextAccessor context) : base(db, mapper)
        {
            _queryableSourceService = queryableSourceService;
            _context = context;
            InitQuerySource();
        }

        public override void InitQuerySource()
        {
            QuerySource = _queryableSourceService.GetQueryable<TQuerySource>(db as UniversalQueueContext);
        }

        public override string GetCurrentUserId()
        {
            return _context?.HttpContext?.User?.Identity?.Name??"admin";// todo:验证httpcontext user
        }
    }
}
