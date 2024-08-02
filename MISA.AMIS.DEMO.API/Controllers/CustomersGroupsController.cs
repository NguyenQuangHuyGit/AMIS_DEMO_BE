using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.DEMO.Core;

namespace MISA.AMIS.DEMO.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomersGroupsController : BaseController<CustomerGroupCreateDto, CustomerGroupDto, CustomerGroup>
    {
        public CustomersGroupsController(ICustomerGroupService customerGroupService) : base(customerGroupService)
        {
        }
    }
}
