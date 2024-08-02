using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    /// <summary>
    /// DTO nhân viên trả về khi validate file Excel Import
    /// </summary>
    public class EmployeeImportDto : EmployeeCreateDto
    {
        public EmployeeImportDto()
        {
            ErrorList = new List<string>();
            MarkErrorCell = new Dictionary<int, string>();
        }

        public Dictionary<int, string> MarkErrorCell { get; set; }

        // Chuỗi hiện thị ngày sinh nếu sai định dạng
        public string? DateOfBirthString { get; set; }

        // Chuỗi hiện thị ngày cấp nếu sai định dạng
        public string? IdentityDateString { get; set; }

        // Trạng thái của bản ghi (Hợp lệ || Không hợp lệ)
        public bool Status { get; set; }

        // Danh sách lỗi ứng với từng bản ghi nhân viên
        public List<string> ErrorList { get; }
    }
}
