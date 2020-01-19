namespace Web.Services
{
    public class ResourceService
    {
        //private DatabaseContext _db;
        //private static object _lock=new Object();
        //private ResourceRepository _resourceRepository;
        //public ResourceService(DatabaseContext db, ResourceRepository resourceRepository)
        //{
        //    _db = db;
        //    _resourceRepository = resourceRepository;
        //}

        ///// <summary>
        ///// 获所有webapi的action设置为权限资源
        ///// </summary>
        //public void InitWebapiResource()
        //{
        //    var allController = Assembly.GetExecutingAssembly().ExportedTypes
        //        .Where(a => a.IsSubclassOf(typeof(ControllerBase)) && !Attribute.IsDefined(a, typeof(AllowAnonymousAttribute))).ToList();
        //    var resources = new List<Resource>();
        //    allController.ForEach(controller =>
        //    {
        //        controller.GetMethods(BindingFlags.Instance |  BindingFlags.DeclaredOnly | BindingFlags.Public).Where(a=>!a.IsConstructor && !Attribute.IsDefined(a, typeof(AllowAnonymousAttribute))).ToList().ForEach(
        //            action =>
        //            {
        //                var description = ((DescriptionAttribute)action.GetCustomAttribute(typeof(DescriptionAttribute)))?.Description;
        //                var resourceKey = ResourceKeyGenerate(controller.Name, action.Name);
        //                resources.Add(new Resource()
        //                {
        //                    Category = ResourceCategory.Webapi.ToString(),
        //                    Description = description,
        //                    Key = resourceKey,
        //                    Value = resourceKey
        //                });
        //            });
        //    });
        //    //对resource的key做唯一限制 
        //    resources = resources.GroupBy(a => a.Key).SelectMany(a=>new List<Resource>(){a.FirstOrDefault()}).ToList();
        //    lock (_lock)
        //    {
        //        var existResourceKeys = _db.Resources.Select(a=>a.Key).ToList();
        //        var canAddResources = resources.Where(a => !existResourceKeys.Contains(a.Key));
        //        foreach (var canAddResource in canAddResources)
        //        {
        //            _db.Resources.Add(new Resource()
        //            {
        //                Category=canAddResource.Category,
        //                CreateTime=DateTime.Now,
        //                Description=canAddResource.Description,
        //                IsDeleted=false,
        //                Key=canAddResource.Key,
        //                ParentId=0,
        //                Value=canAddResource.Value,
        //                UpdateTime=DateTime.Now
        //            });
        //        }

        //        _db.SaveChanges();
        //    }
            
        //}

        //public static string ResourceKeyGenerate(ControllerActionDescriptor controllerActionDescriptor)
        //{
        //    return ResourceKeyGenerate(controllerActionDescriptor.ControllerName,
        //        controllerActionDescriptor.ActionName);
        //}

        //public static string ResourceKeyGenerate(string controllerName, string actionName)
        //{
        //    return $"{controllerName}_{actionName}";
        //}

        //public void AddResource(string key,string value,ResourceCategory category,int parentId=0)
        //{
        //    var existResource = _resourceRepository.FirstOrDefault(a => a.Key == key);
        //    if (existResource!=null)
        //    {
        //        throw new Exception("已经有同名的资源，不能重复");
        //    }

        //    if (parentId>0)
        //    {
        //        var parentResourc = _resourceRepository.FirstOrDefault(a => a.ParentId == parentId);
        //        if (parentResourc == null)
        //        {
        //            throw new Exception("父资源不存在");
        //        }
        //        _resourceRepository.Add(new Resource()
        //        {
        //            Category=category.ToString(),
        //            Key=key,
        //            Value=value,
        //            ParentId=parentId
        //        });
        //    }
            

        //    #region 避免父子循环

            

        //    #endregion

        //}
    }
}
