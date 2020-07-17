using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Esf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.CodeGenerater
{


    public static class CodeGeneraterHelper
    {

        public static CodeGenerateDto GenerateDtoFromConfig(string val,out List<string> errors)
        {
            var result = new CodeGenerateDto();
            errors = new List<string>();
            var configDto = JsonConvert.DeserializeObject<CodeGenerateConfig>(val);
            result.BasePath = configDto.BasePath.Trim('\\');
            result.Entities = CodeGeneraterHelper.GenerateEntitiesModelFromTableModels(configDto,ref errors);
            result.Enums = GenerateEnumModelFromConfig(configDto, ref errors);
            return result;

        }
        /// <summary>
        /// 从json配置里的entity配置节点，生成template.tt使用的model
        /// </summary>
        /// <param name="tableModels"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static List<EntityModel> GenerateEntitiesModelFromTableModels(CodeGenerateConfig config, ref List<string> errors)
        {
            var result = new List<EntityModel>();
            if (errors == null) { errors = new List<string>(); }

            foreach (var item in config.Entities)
            {
                var columns = new List<EntityFieldModel>();
                foreach (var column in item.Columns)
                {
                    columns.Add(GetFieldModel(column, ref errors));
                }
                result.Add(new EntityModel
                {
                    Name = item.Name,
                    TableName = item.TableName,
                    Fields = columns
                });
            }
            return result;
        }
        public static List<EnumModel> GenerateEnumModelFromConfig(CodeGenerateConfig config, ref List<string> errors)
        {
            var result = new List<EnumModel>();
            if (errors==null)
            {
                errors = new List<string>(); 
            }
            foreach (var item in config.Enums)
            {
                GetEnumModel(item, ref errors);
            }
            return result;
        }

        private static EntityFieldModel GetFieldModel(string val, ref List<string> error)
        {
            var result = new EntityFieldModel();
            if (error == null)
            {
                error = new List<string>();
            }
            var items = val.Split(',', '，');
            if (items.Length < 3)
            {
                error.Add($"{val}少于3段");
                return null;
            }
            result.Name = items[0];
            result.Type = items[1];
            result.Comment = items[2];
            if (items.Length > 3)
            {
                var len = items[3];
                if (result.Attributes == null)
                {
                    result.Attributes = new List<string>();
                }
                result.Attributes.Add($"[MaxLength({len})]");
            }
            return result;
        }

        private static EnumModel GetEnumModel(string val, ref List<string> error)
        {
            var result = new EnumModel();
            if (error == null)
            {
                error = new List<string>();
            }
            var items = val.Split(',', '，');
            if (items.Length == 0 || items.Length % 2 == 1)
            {
                error.Add("枚举的配置参数必需大于0且为偶数");
            }
            for (int i = 0; i < items.Length; i = i + 2)
            {
                if (i == 0)
                {
                    // 第一对为枚举名和描述
                    result.Name = items[i];
                    result.Comment = items[i + 1];
                }
                else
                {
                    result.Items.Add(new EnumFieldModel
                    {
                        Name = items[i],
                        Comment = items[i + 1]
                    });
                }

            }
            return result;
        }

        public static List<VueModel> GenerateVueModelFromEntityModels(List<EntityModel> entityModels)
        {
            return entityModels.Select(a => new VueModel
            {
                Name = ToCamel(a.Name),
                Fields = a.Fields.Select(i => new VueFieldModel
                {
                    Name = ToCamel(i.Name),
                    Comment = i.Comment,
                    Type = ConvertEntityTypeToVueType(i.Type)
                }).ToList()
            }).ToList();
        }

        public static string ToCamel(string val)
        {
            return val.First().ToString().ToLower() + val.Substring(1, val.Length - 1);
        }
        public static string ConvertEntityTypeToVueType(string entityType)
        {
            //支持：string,int,datetime,date,select,multiSelect,time
            switch (entityType)
            {
                case "string":
                    return "string";
                case "DateTime":
                    return "datetime";
                case "int":
                    return "int";
                default:
                    return "string";
            }
        }
    }


}
