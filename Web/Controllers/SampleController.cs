using ApplicationCore.Entity;
using Snail.Core.Interface;
using Web.DTO.Sample;

namespace Web.Controllers
{
    public class SampleController : CRUDController<SampleEntity, SampleSourceDto, SampleResultDto, SampleSaveDto, SampleQueryDto>
    {
        public SampleController(ICRUDService<SampleEntity, string> CRUDService) : base(CRUDService)
        {
        }
    }
}
