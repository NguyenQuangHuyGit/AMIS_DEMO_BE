using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public interface IMappingFileService
    {
        /// <summary>
        /// Hàm kiểm tra xem file Import có hợp lệ hay không
        /// </summary>
        /// <param name="fileImport">File Excel chứa dữ liệu</param>
        /// <exception cref="Exception">Trả ra Exception: File không được trống || File không đúng định dạng</exception>
        /// CreatedBy: QuangHuy (16/01/2024)
        public void CheckFileImport(IFormFile fileImport);

        /// <summary>
        /// Hàm chuyển dữ liệu từ file Excel vào danh sách Nhân viên
        /// Validate các trường theo DataAnnotation và theo nghiệp vụ trong DB
        /// </summary>
        /// <param name="worksheet">Worksheet cần đọc</param>
        /// <returns>Danh sách nhân viên kèm theo danh sách lỗi của từng nhân viên</returns>
        /// CreatedBy: QuangHuy (17/01/2024)
        public Task<List<EmployeeImportDto>> GetEmployeeDataAndValidateFromFile(ExcelWorksheet worksheet);

        /// <summary>
        /// Hàm gán dữ liệu nhân viên vào ô Excel tương ứng với từng trường
        /// </summary>
        /// <param name="worksheet">Sheet excel</param>
        /// <param name="employee">Thông tin nhân viên</param>
        /// <param name="row">Số dòng đang thao tác</param>
        /// CreatedBy: QuangHuy (23/01/2024)
        public void MapEmployeeDataToFile(ExcelWorksheet worksheet, EmployeeModel employee, int row);

        /// <summary>
        /// Hàm gán giá trị nhân viên bị lỗi thông tin vào các ô dữ liệu tương ứng trong excel
        /// </summary>
        /// <param name="worksheet">Sheet excel</param>
        /// <param name="employee">Thông tin nhân viên</param>
        /// <param name="row">Số dòng đang thao tác</param>
        /// CreatedBy: QuangHuy (23/01/2024)
        public void MapErrorDataToFile(ExcelWorksheet worksheet, EmployeeImportErrorDto employee, int row);
    }
}
