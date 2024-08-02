using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.DEMO.Core;

namespace MISA.AMIS.DEMO.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomersController : BaseController<CustomerCreateDto, CustomerDto ,Customer>
    {
        public CustomersController(ICustomerService customerService) : base(customerService)
        {
        }
    }
}
