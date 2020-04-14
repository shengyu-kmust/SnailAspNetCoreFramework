using ApplicationCore.Dtos;
using Snail.Core.Enum;

namespace Web.DTO
{
    public class UserSaveDto: BaseDto
    {
        public string Account { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Pwd { get; set; }
        public EGender Gender { get; set; }
    }
}
