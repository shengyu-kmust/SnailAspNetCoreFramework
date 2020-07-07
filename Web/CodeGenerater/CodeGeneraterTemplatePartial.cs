using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.CodeGenerater
{
    public partial class ControllerTemplate
    {
        public string Name { get; set; }
    }
    public partial class DtoTemplate
    {
        public DtoModel Dto { get; set; }
    }
    public partial class EntityConfigTemplate
    {
        public string Name { get; set; }
        public string TableName { get; set; }
    }
    public partial class EntityTemplate
    {
        public EntityModel Entity { get; set; }
    }
    public partial class ServiceTemplate
    {
        public string Name { get; set; }
    }
    public partial class AppDbContextTemplate
    {
        public List<string> EntityNames { get; set; }
    }
    public partial class VueTemplate
    {
        public VueModel Vue { get; set; }
    }
    public partial class VueApiTemplate
    {
        public List<string> EntityNames { get; set; }
    }
    public partial class VueRouterTemplate
    {
        public List<VueRouteModel> VueRouteModels { get; set; }
    }
}
