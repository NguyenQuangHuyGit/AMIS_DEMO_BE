using Microsoft.Extensions.Localization;
using MISA.AMIS.DEMO.Core.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    /// <summary>
    /// Dto khi tạo mới 1 nhân viên
    /// </summary>
    public class EmployeeCreateDto
    {
        [Required(ErrorMessageResourceType = typeof(MISAResources), ErrorMessageResourceName = nameof(MISAResources.InValidMsg_EmployeeCodeRequired))]
        [MaxLength(20, ErrorMessageResourceType = typeof(MISAResources), ErrorMessageResourceName = nameof(MISAResources.InValidMsg_EmployeeCodeTooLong))]
        [PrefixCode("NV-", ErrorMessageResourceType = typeof(MISAResources), ErrorMessageResourceName = nameof(MISAResources.InValidMsg_InvalidPrefixEmployeeCode))]
        [SuffixCode(@"\d+$", ErrorMessageResourceType = typeof(MISAResources), ErrorMessageResourceName = nameof(MISAResources.InValidMsg_InvalidSuffixEmployeeCode))]
        // Mã nhân viên
        public string EmployeeCode { get; set; }

        [Required(ErrorMessageResourceType = typeof(MISAResources), ErrorMessageResourceName = nameof(MISAResources.InValidMsg_FullNameRequired))]
        [MaxLength(100, ErrorMessageResourceType = typeof(MISAResources), ErrorMessageResourceName = nameof(MISAResources.InValidMsg_FullNameTooLong))]
        // Họ tên nhân viên
        public string FullName { get; set; }

        [MaxLength(15, ErrorMessageResourceType = typeof(MISAResources), ErrorMessageResourceName = nameof(MISAResources.InValidMsg_PhoneNumberTooLong))]
        [OnlyNumber(ErrorMessageResourceType = typeof(MISAResources), ErrorMessageResourceName = nameof(MISAResources.InValidMsg_OnlyNumberPhone))]
        // Số điện thoại nhân viên
        public string? PhoneNumber { get; set; }

        [MaxLength(15, ErrorMessageResourceType = typeof(MISAResources), ErrorMessageResourceName = nameof(MISAResources.InValidMsg_PhoneNumberTooLong))]
        [OnlyNumber(ErrorMessageResourceType = typeof(MISAResources), ErrorMessageResourceName = nameof(MISAResources.InValidMsg_OnlyNumberPhone))]
        // Số điện thoại cố định
        public string? LandlineTelephone { get; set; }

        [MaxLength(255, ErrorMessageResourceType = typeof(MISAResources), ErrorMessageResourceName = nameof(MISAResources.InValidMsg_AddressTooLong))]
        // Địa chỉ nhân viên
        public string? Address { get; set; }

        [MaxLength(100, ErrorMessageResourceType = typeof(MISAResources), ErrorMessageResourceName = nameof(MISAResources.InValidMsg_EmailTooLong))]
        [Email(ErrorMessageResourceType = typeof(MISAResources), ErrorMessageResourceName = nameof(MISAResources.InValidMsg_InvalidEmail))]
        // Email nhân viên
        public string? Email { get; set; }

        // Giới tính nhân viên
        public Gender? Gender { get; set; }

        // Id đơn vị / phòng ban
        public Guid? DepartmentId { get; set; }

        [Required(ErrorMessageResourceType = typeof(MISAResources), ErrorMessageResourceName = nameof(MISAResources.InValidMsg_DepartmentRequired))]
        // Tên đơn vị / phòng ban
        public string? DepartmentName { get; set; }

        // Id vị trí / công việc
        public Guid? PositionId { get; set; }

        // Tên vị trí / công việc
        public string? PositionName { get; set; }

        [ValidateDateTime(ErrorMessageResourceType = typeof(MISAResources), ErrorMessageResourceName = nameof(MISAResources.InValidMsg_InValidDateOfBirth))]
        // Ngày sinh nhân viên
        public DateTime? DateOfBirth { get; set; }

        [MaxLength(25, ErrorMessageResourceType = typeof(MISAResources), ErrorMessageResourceName = nameof(MISAResources.InValidMsg_IdentityNumberTooLong))]
        [OnlyNumber(ErrorMessageResourceType = typeof(MISAResources), ErrorMessageResourceName = nameof(MISAResources.InValidMsg_OnlyNumberIdentityNumber))]
        // Số căn cước công dân nhân viên
        public string? IdentityNumber { get; set; }

        [MaxLength(100, ErrorMessageResourceType = typeof(MISAResources), ErrorMessageResourceName = nameof(MISAResources.InValidMsg_IdentityPlaceTooLong))]
        // Nơi cấp
        public string? IdentityPlace { get; set; }

        [ValidateDateTime(ErrorMessageResourceType = typeof(MISAResources), ErrorMessageResourceName = nameof(MISAResources.InValidMsg_InValidIdentityDate))]
        // Ngày cấp
        public DateTime? IdentityDate { get; set; }

        // Lương nhân viên
        public double? Salary { get; set; }

        [MaxLength(20, ErrorMessageResourceType = typeof(MISAResources), ErrorMessageResourceName = nameof(MISAResources.InValidMsg_BankAccountTooLong))]
        // Tài khoản ngân hàng
        public string? BankAccount { get; set; }

        [MaxLength(100, ErrorMessageResourceType = typeof(MISAResources), ErrorMessageResourceName = nameof(MISAResources.InValidMsg_BankNameTooLong))]
        // Tên ngân hàng
        public string? BankName { get; set; }

        [MaxLength(100, ErrorMessageResourceType = typeof(MISAResources), ErrorMessageResourceName = nameof(MISAResources.InValidMsg_BankBranchTooLong))]
        // Chi nhánh ngân hàng
        public string? BankBranch { get; set; }
    }
}
