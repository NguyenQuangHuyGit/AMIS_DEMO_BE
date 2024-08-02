using Microsoft.Extensions.Localization;
using MISA.AMIS.DEMO.Core.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    /// <summary>
    /// Lớp dùng kiểm tra tính hợp lệ các trường cần thiết của Nhân viên
    /// </summary>
    public class ValidateEmployee : IValidateEmployee
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPositionRepository _positionRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public ValidateEmployee(IEmployeeRepository employeeRepository, IPositionRepository positionRepository, IDepartmentRepository departmentRepository)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _positionRepository = positionRepository;
        }

        public async Task ValidateDuplicateEmployeeData(EmployeeCreateDto createDto)
        {
            // Tạo list lấy danh sách lỗi
            var errors = new Dictionary<string, string>();

            // Check trùng mã nhân viên
            var employee = await _employeeRepository.FindByFieldAsync("EmployeeCode", createDto.EmployeeCode);

            if (employee != null)
            {
                errors.Add("EmployeeCode", String.Format(MISAResources.InValidMsg_DuplicateEmployeeCode, createDto.EmployeeCode));
            }

            // Nếu có lỗi trả về Exception
            if (errors.Count > 0)
            {
                throw new DuplicateException(MISAResources.InValidMsg_DuplicateInput, errors);
            }
        }

        public async Task ValidateReferenceData(EmployeeCreateDto createDto)
        {
            // Tạo list lấy danh sách lỗi
            var errors = new Dictionary<string, string>();

            // Validate vị trí / chức vụ
            if (createDto.PositionName != null && !createDto.PositionName.Equals(""))
            {
                var position = await _positionRepository.FindByFieldAsync("PositionName", createDto.PositionName);
                if (position == null)
                {
                    errors.Add("Position", MISAResources.InValidMsg_PositionInvalid);
                }
                else
                {
                    createDto.PositionId = position.PositionId;
                }
            }

            // Validate phòng ban / đơn vị
            if (createDto.DepartmentName != null && !createDto.DepartmentName.Equals(""))
            {
                var department = await _departmentRepository.FindByFieldAsync("DepartmentName", createDto.DepartmentName);
                if (department == null)
                {
                    errors.Add("Department", MISAResources.InValidMsg_DepartmentInvalid);
                }
                else
                {
                    createDto.DepartmentId = department.DepartmentId;
                }
            }

            // Nếu có lỗi trả về Exception
            if (errors.Count > 0)
            {
                throw new NotExistException(MISAResources.InValidMsg_NotExistInput, errors);
            }
        }

        public string AutoFillEmployeeCode(string? code)
        {
            if(code != null)
            {
                var num = code.Substring(3);
                var prefix = "NV-";
                while (num.Length < 6)
                {
                    num = num.Insert(0, "0");
                }
                return $"{prefix}{num}";
            }
            return "";
        }
    }
}
