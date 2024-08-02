using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        /// <summary>
        /// Hàm lấy mã nhân viên lớn nhất có trong DB
        /// </summary>
        /// <returns>Mã nhân viên lớn nhất</returns>
        /// CreatedBy: QuangHuy(08/01/2024)
        public Task<string?> GetLastestEmployeeCode();

        /// <summary>
        /// Hàm lấy danh sách nhân viên lọc theo Mã || Họ tên || Số điện thoại theo từng trang
        /// </summary>
        /// <param name="pageSize">Số bản ghi trong 1 trang</param>
        /// <param name="pageNumber">Số trang</param>
        /// <param name="fillterString">Chuỗi lọc các bản ghi (Mã || Họ tên || Số điện thoại)</param>
        /// <returns>Danh sách nhân viên tương ứng</returns>
        /// CreatedBy: QuangHuy(08/01/2024)
        public Task<EmployeeFillterModel> GetFillterEmployeeAsync(int pageSize, int pageNumber, string? fillterString);

        /// <summary>
        /// Hàm lấy danh sách nhân viên theo Modal gồm phòng ban và vị trí
        /// </summary>
        /// <returns>Danh sách EmployeeModel</returns>
        /// CreatedBy: QuangHuy (22/01/2024)
        public Task<IEnumerable<EmployeeModel>> GetAllEmployeeModelAsync();

        /// <summary>
        /// Hàm lấy danh sách nhân viên theo Modal gồm phòng ban và vị trí theo danh sách Id
        /// </summary>
        /// <param name="ids">Danh sách Id</param>
        /// <returns>Danh sách nhân viên</returns>
        /// CreatedBy: QuangHuy (25/01/2024)
        public Task<IEnumerable<EmployeeModel>> GetEmployeeModelAsync(List<Guid> ids);
    }
}
