using MISA.AMIS.DEMO.Core.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    /// <summary>
    /// DTO nhân viên bị lỗi thông tin
    /// </summary>
    public class EmployeeImportErrorDto
    {
        // Chuỗi hiện thị ngày sinh nếu sai định dạng
        public string? DateOfBirthString { get; set; }

        // Chuỗi hiện thị ngày cấp nếu sai định dạng
        public string? IdentityDateString { get; set; }

        // Trạng thái của bản ghi (Hợp lệ || Không hợp lệ)
        public bool? Status { get; set; }

        // Chỉ định ô có lỗi
        public Dictionary<int, string>? MarkErrorCell { get; set; }

        // Danh sách lỗi ứng với từng bản ghi nhân viên
        public List<string>? ErrorList { get; set; }

        // Mã nhân viên
        public string? EmployeeCode { get; set; }

        // Họ tên nhân viên
        public string? FullName { get; set; }

        // Số điện thoại nhân viên
        public string? PhoneNumber { get; set; }

        // Địa chỉ nhân viên
        public string? Address { get; set; }

        // Email nhân viên
        public string? Email { get; set; }

        // Giới tính nhân viên
        public Gender? Gender { get; set; }

        // Id đơn vị / phòng ban
        public Guid? DepartmentId { get; set; }

        // Tên đơn vị / phòng ban
        public string? DepartmentName { get; set; }

        // Id vị trí / công việc
        public Guid? PositionId { get; set; }

        // Tên vị trí / công việc
        public string? PositionName { get; set; }

        // Ngày sinh nhân viên
        public DateTime? DateOfBirth { get; set; }

        // Số ccan cước công dân nhân viên
        public string? IdentityNumber { get; set; }

        // Nơi cấp
        public string? IdentityPlace { get; set; }

        // Ngày cấp
        public DateTime? IdentityDate { get; set; }

        // Lương nhân viên
        public double? Salary { get; set; }

        // Tài khoản ngân hàng
        public string? BankAccount { get; set; }

        // Tên ngân hàng
        public string? BankName { get; set; }

        // Chi nhánh ngân hàng
        public string? BankBranch { get; set; }
    }
}
