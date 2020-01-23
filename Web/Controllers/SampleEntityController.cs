using ApplicationCore.Entity;
using Snail.Core.Interface;
using Web.DTO.Sample;

namespace Web.Controllers
{
    public class SampleEntityController : CRUDController<SampleEntity, SampleEntitySourceDto, SampleEntityResultDto, SampleEntitySaveDto, SampleEntityQueryDto>
    {
        public SampleEntityController(ICRUDService<SampleEntity, SampleEntitySourceDto, string> CRUDService) : base(CRUDService)
        {
        }
    }
}
