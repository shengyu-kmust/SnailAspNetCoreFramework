using ApplicationCore.Dtos.DataDictionary;
using ApplicationCore.Entities;
using ApplicationCore.Service;
using Snail.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.DTO.DataDictionary;

namespace Web.Controllers
{
    public class DataDictionaryController : CRUDController<DataDictionary, DataDictionaryDto, DataDictionaryDto, DataDictionaryDto, DataDictionaryQueryDto>
    {
        //private IDataDictionaryService _dataDictionaryService;
        //public DataDictionaryController(IDataDictionaryService dataDictionaryService)
        //{
        //    _dataDictionaryService = dataDictionaryService;
        //}
        public DataDictionaryController(ICRUDService<DataDictionary, DataDictionaryDto, string> CRUDService) : base(CRUDService)
        {
        }
    }
}
