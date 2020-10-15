using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snail.Core.Permission;
using Snail.Web.CodeGenerater;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Web.Controllers
{
    /// <summary>
    /// 代码生成器接口
    /// </summary>
    [ApiController]
    [Authorize(Policy = PermissionConstant.PermissionAuthorizePolicy)]
    [Route("api/[Controller]/[Action]")]
    public class CodeGeneraterController : ControllerBase
    {
        [HttpGet]
        public void Generater()
        {
            var configValue = System.IO.File.ReadAllText("./CodeGenerater/codeGeneratorTestModel.json");
            var configDto=CodeGeneraterHelper.GenerateDtoFromConfig(configValue, out List<string> errors);
            GenerateEntity(configDto);
            GenerateService(configDto);
            GenerateEntityConfig(configDto);
            GenerateDto(configDto);
            GenerateController(configDto);
            GenerateAppDbContext(configDto);
            GenerateVue(configDto);
            GenerateVueApi(configDto);
            GenerateVueRouter(configDto);
            GenerateEnumJs(configDto);
            GenerateEnum(configDto);
            
        }

        #region ApplicationCore
        private void GenerateDto(CodeGenerateDto dto)
        {
            foreach (var entity in dto.Entities)
            {
                new List<string> { "Result", "Save", "Source", "Query" }.ForEach(preFix =>
                {
                    var dtoTemplate = new DtoTemplate();
                    dtoTemplate.Dto = new DtoModel { Name = entity.Name, Fields = preFix == "Query" ? new List<EntityFieldModel>() : entity.Fields, Prefix = preFix, BaseClass = preFix == "Query" ? "BaseQueryPaginationDto" : "BaseIdDto" };
                    Directory.CreateDirectory($@"{dto.BasePath}\ApplicationCore\Dtos\{entity.Name}");
                    System.IO.File.WriteAllText($@"{dto.BasePath}\ApplicationCore\Dtos\{entity.Name}\{entity.Name}{preFix}Dto.cs", dtoTemplate.TransformText());
                });
            }
        }
        private void GenerateEntity(CodeGenerateDto dto)
        {
            foreach (var entity in dto.Entities)
            {
                var entityTemplate = new EntityTemplate();
                entityTemplate.Entity = entity;
                Directory.CreateDirectory($@"{dto.BasePath}\ApplicationCore\Entities");
                System.IO.File.WriteAllText($@"{dto.BasePath}\ApplicationCore\Entities\{entity.Name}.cs", entityTemplate.TransformText());
            }
        }

        #endregion

        #region Infrastructure
        private void GenerateEntityConfig(CodeGenerateDto dto)
        {
            foreach (var entity in dto.Entities)
            {
                var entityConfigTemplate = new EntityConfigTemplate();
                entityConfigTemplate.Name = entity.Name;
                entityConfigTemplate.TableName = entity.TableName;
                Directory.CreateDirectory($@"{dto.BasePath}\Infrastructure\Data\Config");
                System.IO.File.WriteAllText($@"{dto.BasePath}\Infrastructure\Data\Config\{entity.Name}Configuration.cs", entityConfigTemplate.TransformText());
            }
        }
        #endregion

        #region Service
        private void GenerateService(CodeGenerateDto dto)
        {
            foreach (var entity in dto.Entities)
            {
                var serviceTemplate = new ServiceTemplate();
                serviceTemplate.Name = entity.Name;
                Directory.CreateDirectory($@"{dto.BasePath}\Service");
                System.IO.File.WriteAllText($@"{dto.BasePath}\Service\{entity.Name}Service.cs", serviceTemplate.TransformText());
            }
        }
        #endregion

        #region Controllers
        private void GenerateController(CodeGenerateDto dto)
        {
            foreach (var entity in dto.Entities)
            {
                var controllerTemplate = new ControllerTemplate();
                controllerTemplate.Name = entity.Name;
                controllerTemplate.Comment = entity.Comment;
                Directory.CreateDirectory($@"{dto.BasePath}\Web\Controllers");
                System.IO.File.WriteAllText($@"{dto.BasePath}\Web\Controllers\{entity.Name}Controller.cs", controllerTemplate.TransformText());
            }
        }

        #endregion

        #region AppDbContext
        private void GenerateAppDbContext(CodeGenerateDto dto)
        {
            var appDbContextTemplate = new AppDbContextTemplate();
            appDbContextTemplate.EntityNames = dto.Entities.Select(a => a.Name).ToList();
            Directory.CreateDirectory($@"{dto.BasePath}\Infrastructure");
            System.IO.File.WriteAllText($@"{dto.BasePath}\Infrastructure\AppDbContextPartial.cs", appDbContextTemplate.TransformText());
        }

        #endregion

        #region enum
        private void GenerateEnum(CodeGenerateDto dto)
        {
            var enumTemplate = new EnumTemplate();
            foreach (var enumModel in dto.Enums)
            {
                enumTemplate.Model = enumModel;
                Directory.CreateDirectory($@"{dto.BasePath}\ApplicationCore\Enums");
                System.IO.File.WriteAllText($@"{dto.BasePath}\ApplicationCore\Enums\{enumModel.Name}.cs", enumTemplate.TransformText());

            }
        }
        #endregion


        #region Vue
        private void GenerateVue(CodeGenerateDto dto)
        {
            var vueModels = CodeGeneraterHelper.GenerateVueModelFromEntityModels(dto.Entities);
            foreach (var vue in vueModels)
            {
                var vueTemplate = new VueTemplate();
                vueTemplate.Vue = vue;
                Directory.CreateDirectory($@"{dto.BasePath}\Web\ClientApp\src\views\basic");
                System.IO.File.WriteAllText($@"{dto.BasePath}\Web\ClientApp\src\views\basic\{vue.Name}.vue", vueTemplate.TransformText());
            }
        }
        #endregion
        #region vue api
        private void GenerateVueApi(CodeGenerateDto dto)
        {
            var vueApiTemplate = new VueApiTemplate();
            vueApiTemplate.EntityNames = dto.Entities.Select(a => CodeGeneraterHelper.ToCamel(a.Name)).ToList();
            Directory.CreateDirectory($@"{dto.BasePath}\Web\ClientApp\src\api");
            System.IO.File.WriteAllText($@"{dto.BasePath}\Web\ClientApp\src\api\basic.js", vueApiTemplate.TransformText());

        }
        #endregion
        private void GenerateVueRouter(CodeGenerateDto dto)
        {
            var vueRouterTemplate = new VueRouterTemplate();
            vueRouterTemplate.VueRouteModels = dto.Entities.Select(a => new VueRouteModel { Name= CodeGeneraterHelper.ToCamel(a.Name),Comment=a.Comment}).ToList();
            Directory.CreateDirectory($@"{dto.BasePath}\Web\ClientApp\src\router");
            System.IO.File.WriteAllText($@"{dto.BasePath}\Web\ClientApp\src\router\basicRouters.js", vueRouterTemplate.TransformText());
        }

        #region js
        private void GenerateEnumJs(CodeGenerateDto dto)
        {
            var enumJsTemplate = new EnumJsTemplate();
            enumJsTemplate.Model = dto.Enums;
            Directory.CreateDirectory($@"{dto.BasePath}\Web\ClientApp\src\utils");
            System.IO.File.WriteAllText($@"{dto.BasePath}\Web\ClientApp\src\utils\enum.js", enumJsTemplate.TransformText());

        }
        #endregion


    }
}
