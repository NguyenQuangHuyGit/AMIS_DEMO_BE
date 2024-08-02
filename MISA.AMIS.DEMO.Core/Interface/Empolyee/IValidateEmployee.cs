using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public interface IValidateEmployee
    {
        /// <summary>
        /// Hàm tự động điền nếu hậu tố mã nhân viên không dủ 6 số
        /// </summary>
        /// <param name="code">Mã nhân viên</param>
        /// <returns>Mã mới</returns>
        /// CreatedBy: QuangHuy (20/3/2024)
        public string AutoFillEmployeeCode(string? code);

        /// <summary>
        /// Hàm kiểm tra tính hợp lệ của các dữ liệu quan hệ bảng
        /// </summary>
        /// <param name="createDto">Dto nhân viên được tạo</param>
        /// <returns></returns>
        /// <exception cref="DuplicateException">Trả về ngoại lệ với danh sách lỗi</exception>
        /// CreatedBy: QuangHuy (24/01/2024)
        public Task ValidateReferenceData(EmployeeCreateDto createDto);

        /// <summary>
        /// Hàm kiểm tra trùng lặp các dữ liệu của nhân viên
        /// </summary>
        /// <param name="createDto">Thông tin nhân viên sẽ thêm</param>
        /// <returns></returns>
        /// <exception cref="DuplicateException">Trả về ngoại lệ trùng lặp nếu trùng dữ liệu</exception>
        /// CreatedBy: QuangHuy (23/01/2024)
        public Task ValidateDuplicateEmployeeData(EmployeeCreateDto createDto);
    }
}
