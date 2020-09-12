using ApplicationCore.Dtos;
using Snail.Core.Enum;

namespace ApplicationCore.Dtos
{
    public class ConfigSaveDto: BaseDto
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public string ExtraInfo { get; set; }
    }
}
