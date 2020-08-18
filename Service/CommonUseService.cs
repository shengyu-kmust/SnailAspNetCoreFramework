using Microsoft.Extensions.DependencyInjection;
using Snail.Office;
using System;
using System.Collections.Generic;
using System.IO;

namespace Service
{

    /// <summary>
    /// 常用功能
    /// </summary>
    public class CommonUseService : ServiceContextBaseService
    {
        public CommonUseService(ServiceContext serviceContext) : base(serviceContext)
        {
        }
        public Stream ExportExcel()
        {
            var excelHelper=serviceContext.serviceProvider.GetService<IExcelHelper>();
            return excelHelper.ExportToExcel(new List<ExcelTestDto>() {
                new ExcelTestDto{Name="周晶",Age=32,BirthDate=new DateTime(1989,1,1),IsValide=false},
                new ExcelTestDto{Name="马娟",Age=27,BirthDate=new DateTime(1989,1,1),IsValide=false}
            },ExcelType.XLS);
        }
        public List<ExcelTestDto> ImportExcel(Stream stream)
        {
            var excelHelper = serviceContext.serviceProvider.GetService<IExcelHelper>();
            return excelHelper.ImportFromExcel<ExcelTestDto>(stream);
        }
    }


    #region 测试数据
    public class ExcelTestDto
    {
        [Excel(Name="姓名")]
        public string Name { get; set; }
        [Excel(Name="年龄")]
        public int Age { get; set; }
        [Excel(Name="是否有效")]
        public bool IsValide { get; set; }
        [Excel(Name="出生日期")]
        public DateTime BirthDate { get; set; }
    }
    #endregion
}
