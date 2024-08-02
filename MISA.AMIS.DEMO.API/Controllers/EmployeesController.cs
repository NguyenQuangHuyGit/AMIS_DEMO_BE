using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.DEMO.Core;
using System.Security.Cryptography.X509Certificates;

namespace MISA.AMIS.DEMO.API.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : BaseController<EmployeeCreateDto, EmployeeDto, Employee>
    {
        public EmployeesController(IEmployeeService baseService) : base(baseService)
        {
        }

        /// <summary>
        /// Hàm lấy mã nhân viên mới
        /// </summary>
        /// <returns>Mã StatusCode 200OK && Mã nhân viên mới</returns>
        /// CreatedBy: QuangHuy (08/01/2024)
        [HttpGet("NewEmployeeCode")]
        public async Task<IActionResult> GetNewEmployeeCode()
        {
            var result = await ((IEmployeeService)_baseService).GetNewEmployeeCodeAsync();
            return StatusCode(StatusCodes.Status200OK, result);
        }

        /// <summary>
        /// Hàm trả về danh sách nhân viên theo từng trang, lọc theo mã nhân viên || họ tên || số điện thoại
        /// </summary>
        /// <param name="pageSize">Số lượng bản ghi trong 1 trang</param>
        /// <param name="pageNumber">Số trang</param>
        /// <param name="fillterString">Chuỗi lọc bản ghi (Mã nhân viên || Họ tên || Số điện thoại</param>
        /// <returns>Danh sách thông tin nhân viên tương ứng</returns>
        /// CreatedBy: QuangHuy (08/01/2024)
        [HttpGet("Fillter")]
        public async Task<IActionResult> GetFillter([FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 1, [FromQuery] string? fillterString = "")
        {
            var result = await ((IEmployeeService)_baseService).GetFillterEmployeeAsync(pageSize, pageNumber, fillterString);
            return StatusCode(StatusCodes.Status200OK, result);
        }

        /// <summary>
        /// Hàm xuất khẩu danh sách thông tin tất cả nhân viên
        /// </summary>
        /// <returns>File excel chứa tất cả nhân viên</returns>
        /// CreatedBy: QuangHuy (22/01/2024)
        [HttpGet("Export")]
        public async Task<IActionResult> ExportAllEmployee()
        {
            var result = await ((IEmployeeService)_baseService).ExportAllEmployee();
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        /// <summary>
        /// Hàm nhận và trả về file excel mẫu để nhập khẩu
        /// </summary>
        /// <returns>File Excel mẫu nhập khẩu</returns>
        /// CreatedBy: QuangHuy (22/01/2024)
        [HttpGet("ExampleFile")]
        public IActionResult GetExampleImportFile()
        {
            var result = ((IEmployeeService)_baseService).GetExampleFileEmployee();
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        /// <summary>
        /// Hàm xuất khẩu danh sách thông tin các nhân viên đã chọn
        /// </summary>
        /// <param name="ids">Danh sách id nhân viên muốn xuất khẩu</param>
        /// <returns>File excel chứa tất cả nhân viên</returns>
        /// CreatedBy: QuangHuy (06/03/2024)
        [HttpPost("Export")]
        public async Task<IActionResult> ExportSelectedEmployee([FromBody] List<Guid> ids)
        {
            var result = await ((IEmployeeService)_baseService).ExportSelectedEmployee(ids);
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        /// <summary>
        /// Hàm trả về danh sách nhân viên muốn nhập khẩu đã được kiểm tra tính hợp lệ và các lỗi cảu từng bản ghi
        /// </summary>
        /// <param name="fileImport">File Excel chứa dữ liệu</param>
        /// <returns>Danh sách nhân viên, trạng thái, danh sách lỗi</returns>
        /// CreatedBy: QuangHuy (22/01/2024)
        [HttpPost("ValidateExcel")]
        public async Task<IActionResult> ValidateExcel(IFormFile fileImport)
        {
            var result = await ((IEmployeeService)_baseService).ValidateImportEmployeeList(fileImport);
            return StatusCode(StatusCodes.Status200OK, result);
        }

        /// <summary>
        /// Hàm trả về file Excel chứa danh sách nhân viên bị lỗi thông tin
        /// </summary>
        /// <param name="listEmployee">Danh sách nhân viên bị lỗi</param>
        /// <returns>File Excel</returns>
        /// CreatedBy: QuangHuy (22/01/2024)
        [HttpPost("ErrorFile")]
        public IActionResult GetFileErrorList([FromBody] List<EmployeeImportErrorDto> listEmployee)
        {
            var result = ((IEmployeeService)_baseService).GetFileErrorList(listEmployee);
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        /// <summary>
        /// Hàm trả về file chứa dữ liệu nhân viên nhập khẩu thành công
        /// </summary>
        /// <param name="ids">Danh sách id nhân viên nhập khẩu thành công</param>
        /// <returns>File Excel</returns>
        /// CreatedBy: QuangHuy (25/01/2024)
        [HttpPost("ImportSuccess")]
        public async Task<IActionResult> GetSuccessFileDataImport([FromBody] List<Guid> ids)
        {
            var file = await ((IEmployeeService)_baseService).GetImportSuccessFile(ids);
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        /// <summary>
        /// Hàm nhập khẩu danh sách nhân viên hợp lệ
        /// </summary>
        /// <param name="employees">Danh sách nhân viên</param>
        /// <returns>Trạng thái code 200</returns>
        /// CreatedBy: QuangHuy (22/01/2024)
        [HttpPost("Import/{id}")]
        public async Task<IActionResult> ImportEmployee(Guid id)
        {
            var result = await ((IEmployeeService)_baseService).ImportEmployee(id);
            return StatusCode(StatusCodes.Status200OK, result);
        }

        /// <summary>
        /// Hàm nhận yếu cầu hủy phiên nhập khẩu dữ liệu
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Trạng thái 204</returns>
        /// CreatedBy: QuangHuy (03/03/2024)
        [HttpDelete("Import/{id}")]
        public ActionResult CancelImport(Guid id)
        {
            ((IEmployeeService)_baseService).CancelImportSession(id);
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
