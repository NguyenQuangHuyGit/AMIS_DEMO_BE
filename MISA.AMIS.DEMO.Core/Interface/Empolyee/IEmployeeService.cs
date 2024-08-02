using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public interface IEmployeeService : IBaseService<EmployeeCreateDto, EmployeeDto, Employee>
    {
        /// <summary>
        /// Hàm xủ lý và trả về mã nhân viên mới
        /// </summary>
        /// <returns>Mã nhân viên mới</returns>
        /// CreatedBy: Quang Huy (08/01/2024)
        public Task<string> GetNewEmployeeCodeAsync();

        /// <summary>
        /// Hàm xử lý dữ liệu, trả về danh sách nhân viên theo từng trang, lọc theo mã nhân viên || họ tên || số điện thoại
        /// </summary>
        /// <param name="pageSize">Số lượng bản ghi trong 1 trang</param>
        /// <param name="pageNumber">Số trang</param>
        /// <param name="fillterString">Chuỗi lọc bản ghi (Mã nhân viên || Họ tên || Số điện thoại</param>
        /// <returns>Danh sách thông tin nhân viên tương ứng</returns>
        /// CreatedBy: Quang Huy (08/01/2024)
        public Task<EmployeeFillterDto> GetFillterEmployeeAsync(int pageSize, int pageNumber, string? fillterString);

        /// <summary>
        /// Hàm kiểm tra tính hợp lệ của dự liệu các trường trong file Excel gửi về
        /// </summary>
        /// <param name="fileImport"></param>
        /// <returns>Danh sách nhân viên và các lỗi tương ứng với từng bản ghi</returns>
        /// CreatedBy: QuangHuy (16/01/2024)
        public Task<ImportDto<EmployeeImportDto>> ValidateImportEmployeeList(IFormFile fileImport);

        /// <summary>
        /// Hàm xử lý dữ liệu để xuất khẩu danh sách nhân viên
        /// </summary>
        /// <returns>File dữ liệu Excel xuất khẩu</returns>
        /// CreatedBy: QuangHuy (22/01/2024)
        public Task<byte[]> ExportAllEmployee();

        /// <summary>
        /// Hàm xử lý dữ liệu để xuất khẩu danh sách nhân viên đã chọn
        /// </summary>
        /// <param name="ids">Danh sách id Nhân viên muốn xuất khẩu</param>
        /// <returns>File dữ liệu Excel xuất khẩu</returns>
        /// CreatedBy: QuangHuy (06/03/2024)
        public Task<byte[]> ExportSelectedEmployee(List<Guid> ids);

        /// <summary>
        /// Hàm lấy file Excel mẫu để nhập khẩu nhân viên
        /// </summary>
        /// <returns>File Excel mẫu với các tiêu đề cột</returns>
        /// CreatedBy: QuangHuy (22/01/2024)
        public byte[] GetExampleFileEmployee();

        /// <summary>
        /// Hàm lấy file chứa danh sách nhân viên bị lỗi
        /// </summary>
        /// <param name="listEmployee">Danh sách nhân viên bị lỗi</param>
        /// <returns>File Excel</returns>
        /// CreatedBy: QuangHuy (22/01/2024)
        public byte[] GetFileErrorList(List<EmployeeImportErrorDto> listEmployee);

        /// <summary>
        /// Hàm nhập khẩu danh sách nhân viên hợp lệ
        /// </summary>
        /// <param name="keyImport">Key của memory cache để lấy dữ liệu đã lưu</param>
        /// <returns></returns>
        /// CreatedBy: QuangHuy (22/01/2024)
        public Task<List<Guid>> ImportEmployee(Guid keyImport);

        /// <summary>
        /// Hàm giải phóng giữ liệu nhập khẩu khỏi cache nếu người dùng yêu cầu hủy
        /// </summary>
        /// <param name="keyImport">Key của memory cache để lấy dữ liệu đã lưu</param>
        /// CreatedBy: QuangHuy (22/01/2024)
        public void CancelImportSession(Guid keyImport);

        /// <summary>
        /// Hàm lấy file excel chứa dữ liệu nhân viên nhập khẩu thành công
        /// </summary>
        /// <param name="ids">Danh sách id của các nhân viên</param>
        /// <returns>File Excel</returns>
        /// CreatedBy: QuangHuy (25/01/2024)
        public Task<byte[]> GetImportSuccessFile(List<Guid> ids);
    }
}
