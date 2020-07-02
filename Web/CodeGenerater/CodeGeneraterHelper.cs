using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.CodeGenerater
{
   

    public static class CodeGeneraterHelper
    {
        /// <summary>
        /// 从json配置里的entity配置节点，生成template.tt使用的model
        /// </summary>
        /// <param name="tableModels"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static List<EntityModel> GenerateEntitiesModelFromTableModels(List<EntityConfigModel> tableModels, out List<string> errors)
        {
            var result = new List<EntityModel>();
            errors = new List<string>();

            foreach (var item in tableModels)
            {
                var columns = new List<EntityFieldModel>();
                foreach (var column in item.Columns)
                {
                    columns.Add(GetFieldModel(column, ref errors));
                }
                result.Add(new EntityModel
                {
                    Name = item.Name,
                    TableName=item.TableName,
                    Fields = columns
                });
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
    }
    

}
