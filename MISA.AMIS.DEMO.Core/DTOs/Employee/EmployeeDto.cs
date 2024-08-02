using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    /// <summary>
    /// DTO Nhân viên
    /// </summary>
    public class EmployeeDto
    {
        [Key]
        public Guid EmployeeId { get; set; }

        // Mã nhân viên
        public string EmployeeCode { get; set; }

        // Họ tên nhân viên
        public string FullName { get; set; }

        // Số điện thoại nhân viên
        public string? PhoneNumber { get; set; }

        // Số điện thoại cố định
        public string? LandlineTelephone { get; set; }

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
