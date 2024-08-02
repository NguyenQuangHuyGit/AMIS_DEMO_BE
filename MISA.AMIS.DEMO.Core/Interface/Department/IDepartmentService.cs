using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public interface IDepartmentService : IBaseService<DepartmentCreateDto, DepartmentDto, Department>
    {
        /// <summary>
        /// Hàm kiểm tra phòng ban có tên truyền vào có tồn tại không
        /// </summary>
        /// <param name="name">Tên phòng ban</param>
        /// <returns>Dto phòng ban nếu tồn tại || NULL nếu không tìm thấy</returns>
        public Task<DepartmentDto?> CheckExistByName(string name);
    }
}
