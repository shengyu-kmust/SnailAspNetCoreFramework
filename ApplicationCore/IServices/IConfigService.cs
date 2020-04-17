using ApplicationCore.Entities;
using ApplicationCore.Entity;
using Snail.Core.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.IServices
{
    public interface IConfigService : IBaseService<Config>
    {
        List<KeyValueDto> GetConfigKeyValue(string parentKey);
    }
}
