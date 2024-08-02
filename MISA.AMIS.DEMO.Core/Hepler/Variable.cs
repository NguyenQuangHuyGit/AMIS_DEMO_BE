using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public static class Variable
    {
        public static readonly Dictionary<string, int> columnImport = new Dictionary<string, int>(){
            { "EmployeeCode", 1 },
            { "FullName", 2 },
            { "PhoneNumber", 3 },
            { "Address", 4 },
            { "Email", 5 },
            { "Gender", 6 },
            { "DepartmentName", 7 },
            { "PositionName", 8 },
            { "DateOfBirth", 9 },
            { "IdentityNumber", 10 },
            { "IdentityDate", 11 },
            { "IdentityPlace", 12 },
            { "BankAccount", 13 },
            { "BankName", 14 },
            { "BankBranch", 15 },
            { "ErrorList", 16 }
        };

        public static readonly List<CultureInfo> supportCulture = new List<CultureInfo>() { new CultureInfo("vi-VN"), new CultureInfo("en-US") };

        public static readonly Dictionary<string, List<string>> headersImport = new Dictionary<string, List<string>>()
        {
            { "vi-VN", new List<string>(){ "STT", "Mã nhân viên (*)", "Tên nhân viên (*)", "Số điện thoại", "Địa chỉ", "Email", "Giới tính", "Tên đơn vị", "Chức danh", "Ngày sinh", "Căn cước công dân", "Ngày cấp", "Nơi cấp", "Số tài khoản", "Tên ngân hàng", "Chi nhánh" } },
            { "en-US", new List<string>(){ "No.", "Employee code (*)", "Full name (*)", "Phone number", "Address", "Email", "Gender", "Department", "Position", "DateOfBirth", "ID Number", "Issuance date", "Issuance place", "Bank Account", "Bank name", "Bank Branch" } }
        };

        public static readonly Dictionary<string, string> fileImportEmployeeTitle = new Dictionary<string, string>()
        {
            { "vi-VN", "Danh sách nhân viên" },
            { "en-US", "List of employee" }
        };

        public static readonly Dictionary<string, string> statusColumn = new Dictionary<string, string>()
        {
            { "vi-VN", "Tổng hợp lỗi" },
            { "en-US", "List of errors" }
        };

        public static readonly Dictionary<string, string> fileErrorImportEmployeeTitle = new Dictionary<string, string>()
        {
            { "vi-VN", "Danh sách nhân viên bị lỗi" },
            { "en-US", "List of employees with errors" }
        };

        public static readonly Dictionary<string, string> fileSuccessImportEmployeeTitle = new Dictionary<string, string>()
        {
            { "vi-VN", "Danh sách nhân viên nhập khẩu thành công" },
            { "en-US", "List of successful imported employees" }
        };
    }
}
