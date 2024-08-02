using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    /// <summary>
    /// Service khách hàng
    /// </summary>
    public class CustomerService : BaseService<CustomerCreateDto, CustomerDto, Customer>, ICustomerService
    {
        /// <summary>
        /// Hàm tạo
        /// </summary>
        /// <param name="baseRepository">Repo khách hàng</param>
        /// <param name="mapper">AutoMapper</param>
        /// CreatedBy: QuangHuy (05/01/2024)
        public CustomerService(ICustomerRepository baseRepository, IMapper mapper) : base(baseRepository, mapper)
        {
        }
    }
}
