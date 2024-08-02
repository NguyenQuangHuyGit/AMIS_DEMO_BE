using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public class CustomerGroupService : BaseService<CustomerGroupCreateDto, CustomerGroupDto, CustomerGroup>, ICustomerGroupService
    {
        public CustomerGroupService(ICustomerGroupRepository baseRepository, IMapper mapper) : base(baseRepository, mapper)
        {
        }
    }
}
