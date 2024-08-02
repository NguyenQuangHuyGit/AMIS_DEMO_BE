using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
        /// <summary>
        /// Hàm lấy thông tin khách hàng bằng mã khách hàng
        /// </summary>
        /// <param name="customerCode">Mã khách hàng</param>
        /// <returns>Thông tin khách hàng || NULL</returns>
        /// CreatedBy: QuangHuy (28/12/2023)
        public Task<Customer?> GetCustomerByCodeAsync(string customerCode);
    }
}
