using Dapper;
using MISA.AMIS.DEMO.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Infrastructure
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        /// <summary>
        /// Hàm tạo
        /// </summary>
        /// <param name="dbContext">Lớp thao tác với DB</param>
        public CustomerRepository(IMISADbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Hàm lấy thông tin khách hàng bằng mã khách hàng
        /// </summary>
        /// <param name="customerCode">Mã khách hàng</param>
        /// <returns>Thông tin khách hàng || NULL</returns>
        /// CreatedBy: QuangHuy (28/12/2023)
        public async Task<Customer?> GetCustomerByCodeAsync(string customerCode)
        {
            var result = await _dbContext.GetAsync<Customer>("CustomerCode", customerCode);
            return result;
        }

        /// <summary>
        /// Hàm lọc danh sách khách hàng
        /// </summary>
        /// <param name="pageSize">Kích thước trang muốn giới hạn</param>
        /// <param name="pageNumber">Số trang</param>
        /// <param name="searchText">Chuỗi muốn lọc theo</param>
        /// <returns>Danh sách khách hàng đã lọc ra</returns>
        /// CreatedBy: QuangHuy (05/01/2024)
        public Task<IEnumerable<Customer>?> Fillter(int pageSize, int pageNumber, string searchText)
        {
            throw new Exception();
        }
    }
}
