using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public interface ICustomerService : IBaseService<CustomerCreateDto, CustomerDto, Customer>
    {
    }
}
