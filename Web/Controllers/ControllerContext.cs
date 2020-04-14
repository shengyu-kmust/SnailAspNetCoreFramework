using AutoMapper;
using Infrastructure;
using Snail.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    /// <summary>
    /// controller公共上下文，用于定义controller类的公共方法，属性等
    /// </summary>
    public class ControllerContext
    {
        public IMapper mapper;
        public IApplicationContext applicationContext;
        public AppDbContext db;
        public ControllerContext(IMapper mapper,IApplicationContext applicationContext, AppDbContext db)
        {
            this.mapper = mapper;
            this.applicationContext = applicationContext;
            this.db = db;
        }
    }
}
