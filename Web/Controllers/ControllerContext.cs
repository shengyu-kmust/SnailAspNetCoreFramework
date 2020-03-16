using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class ControllerContext
    {
        public IMapper mapper;
        public ControllerContext(IMapper mapper)
        {
            this.mapper = mapper;
        }
    }
}
