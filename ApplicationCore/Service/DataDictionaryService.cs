using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Dtos.DataDictionary;
using ApplicationCore.Entities;
using Snail.Core;
using Snail.Core.Interface;

namespace ApplicationCore.Service
{
    public interface IDataDictionaryService:ICRUDService<DataDictionary,DataDictionaryDto,string>
    {
        
    }
}
