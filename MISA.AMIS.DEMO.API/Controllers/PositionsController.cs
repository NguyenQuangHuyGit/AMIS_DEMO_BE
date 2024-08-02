using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.DEMO.Core;

namespace MISA.AMIS.DEMO.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PositionsController : BaseController<PositionCreateDto, PositionDto, Position>
    {
        public PositionsController(IPositionService baseService) : base(baseService)
        {
        }
    }
}
