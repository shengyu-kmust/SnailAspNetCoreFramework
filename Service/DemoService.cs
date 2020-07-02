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
    public class DemoService : BaseService<Demo>
    {
        public DemoService(ServiceContext serviceContext) : base(serviceContext)
        {
        }
    }
}
