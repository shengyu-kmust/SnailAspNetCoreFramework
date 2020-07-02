using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Snail.Core.Permission;
using System.Collections.Generic;
using System.IO;
using Web.CodeGenerater;

namespace Web.Controllers
{
    [ApiController]
    [Authorize(Policy = PermissionConstant.PermissionAuthorizePolicy)]
    [Route("api/[Controller]/[Action]")]
    public class CodeGeneraterController : ControllerBase
    {
        public static string basePath = @"C:\Users\37308\Desktop\code";
        [HttpGet]
        public void Generater()
        {
            var config = System.IO.File.ReadAllText("./CodeGenerater/codeGeneratorTestModel.json");
            var configDto = JsonConvert.DeserializeObject<CodeGenerateConfig>(config);
            basePath = configDto.BasePath.Trim('\\');
            var entities = CodeGeneraterHelper.GenerateEntitiesModelFromTableModels(configDto.Entities, out List<string> errors);
            GenerateEntity(entities);
            GenerateService(entities);
            GenerateEntityConfig(entities);
            GenerateDto(entities);
            GenerateController(entities);
        }

        #region ApplicationCore
        private void GenerateDto(List<EntityModel> entities)
        {
            foreach (var entity in entities)
            {
                new List<string> { "Result", "Save", "Source", "Query" }.ForEach(preFix =>
                {
                    var dtoTemplate = new DtoTemplate();
                    dtoTemplate.Dto = new DtoModel { Name = entity.Name, Fields = preFix == "Query" ? new List<EntityFieldModel>() : entity.Fields, Prefix = preFix, BaseClass = preFix == "Query" ? "BaseQueryPaginationDto" : "BaseIdDto" };
                    Directory.CreateDirectory($@"{basePath}\ApplicationCore\Dtos\{entity.Name}");
                    System.IO.File.WriteAllText($@"{basePath}\ApplicationCore\Dtos\{entity.Name}\{entity.Name}{preFix}Dto.cs", dtoTemplate.TransformText());
                });
            }
        }
        private void GenerateEntity(List<EntityModel> entities)
        {
            foreach (var entity in entities)
            {
                var entityTemplate = new EntityTemplate();
                entityTemplate.Entity = entity;
                Directory.CreateDirectory($@"{basePath}\ApplicationCore\Entities");
                System.IO.File.WriteAllText($@"{basePath}\ApplicationCore\Entities\{entity.Name}.cs", entityTemplate.TransformText());
            }
        }

        #endregion

        #region Infrastructure
        private void GenerateEntityConfig(List<EntityModel> entities)
        {
            foreach (var entity in entities)
            {
                var entityConfigTemplate = new EntityConfigTemplate();
                entityConfigTemplate.Name = entity.Name;
                entityConfigTemplate.TableName = entity.TableName;
                Directory.CreateDirectory($@"{basePath}\Infrastructure\Data\Config");
                System.IO.File.WriteAllText($@"{basePath}\Infrastructure\Data\Config\{entity.Name}Configuration.cs", entityConfigTemplate.TransformText());
            }
        }
        #endregion

        #region Service
        private void GenerateService(List<EntityModel> entities)
        {
            foreach (var entity in entities)
            {
                var serviceTemplate = new ServiceTemplate();
                serviceTemplate.Name = entity.Name;
                Directory.CreateDirectory($@"{basePath}\Service");
                System.IO.File.WriteAllText($@"{basePath}\Service\{entity.Name}Service.cs", serviceTemplate.TransformText());
            }
        }
        #endregion

        #region Controllers
        private void GenerateController(List<EntityModel> entities)
        {
            foreach (var entity in entities)
            {
                var controllerTemplate = new ControllerTemplate();
                controllerTemplate.Name = entity.Name;
                Directory.CreateDirectory($@"{basePath}\Web\Controllers");
                System.IO.File.WriteAllText($@"{basePath}\Web\Controllers\{entity.Name}Controller.cs", controllerTemplate.TransformText());
            }
        }

        #endregion


    }
}
