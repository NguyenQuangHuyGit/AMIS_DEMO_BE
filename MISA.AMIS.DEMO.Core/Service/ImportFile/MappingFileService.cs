using Microsoft.AspNetCore.Http;
using MISA.AMIS.DEMO.Core.Resources;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public class MappingFileService : IMappingFileService
    {
        #region Fields
        private readonly IValidateEmployee _validateEmployee;
        private readonly IPositionRepository _positionRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IEmployeeRepository _employeeRepository;
        #endregion

        #region Contructor
        public MappingFileService(IEmployeeRepository employeeRepository, IPositionRepository positionRepository, IDepartmentRepository departmentRepository, IValidateEmployee validateEmployee)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _positionRepository = positionRepository;
            _validateEmployee = validateEmployee;
        }
        #endregion

        #region Methods
        public void CheckFileImport(IFormFile fileImport)
        {
            // Kiểm tra định dạng file
            if (!Path.GetExtension(fileImport.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                throw new FileException(MISAResources.InValidMsg_FileInvalidType);
            }
            // Kiểm tra file rỗng
            if (fileImport == null || fileImport.Length <= 0)
            {
                throw new FileException(MISAResources.InValidMsg_FileInvalidLenght);
            }
        }

        public async Task<List<EmployeeImportDto>> GetEmployeeDataAndValidateFromFile(ExcelWorksheet worksheet)
        {
            #region Chuẩn bị dữ liệu để validate
            // Tiêu đề cột để kiểm tra
            var currentCulture = CultureInfo.CurrentCulture.ToString();
            var headers = Variable.headersImport[currentCulture].ToList();
            headers.RemoveAt(0);

            //Lấy danh sách phòng ban, vị trí và nhân viên để kiểm tra
            var task1 = _departmentRepository.GetAsync();
            var task2 = _positionRepository.GetAsync();
            var task3 = _employeeRepository.GetAsync();
            await Task.WhenAll(task1, task2, task3);
            var listDepartment = task1.GetAwaiter().GetResult();
            var listPosition = task2.GetAwaiter().GetResult();
            var listEmployee = task3.GetAwaiter().GetResult();

            //Tạo mới danh sách nhân viên
            var employees = new List<EmployeeImportDto>();

            // Lấy số dòng của Worksheet
            if (worksheet.Dimension == null || worksheet.Dimension.Rows <= 0)
            {
                throw new FileException(MISAResources.InValidMsg_FileInvalidLenght);
            }
            var rowCount = worksheet.Dimension.Rows;
            #endregion

            // Lặp qua từng dòng
            for (var row = 3; row <= rowCount; row++)
            {
                #region Kiểm tra xem tiêu đề cột có khớp với file đã cung cấp không
                if (row == 3)
                {
                    for (var i = 0; i < headers.Count; i++)
                    {
                        // Lấy ra tiêu đề của cột
                        var title = worksheet.Cells[row, i + 1]?.Value?.ToString()?.Trim();

                        // Nếu có cột STT thì remove cột đó
                        if (title != null && (Common.CompareRawString(title, "STT") || Common.CompareRawString(title, "No.")))
                        {
                            worksheet.DeleteColumn(i + 1);
                            i -= 1;
                        }
                        else
                        {
                            // Kiểm tra tiều đề của các cột có khớp vơi file mẫu không
                            if (title == null || !Common.CompareRawString(headers[i], title))
                            {
                                var isValidTitle = false;

                                // Kiểm tra trường hợp của các ngôn ngữ khác
                                foreach (var otherLangHeaders in Variable.headersImport.Keys)
                                {
                                    if (otherLangHeaders.Equals(currentCulture))
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        // +1 do có cột STT
                                        var otherTitle = Variable.headersImport[otherLangHeaders][i + 1];
                                        if (title != null && Common.CompareRawString(title, otherTitle))
                                        {
                                            isValidTitle = true;
                                            break;
                                        }
                                    }
                                }

                                // Nếu không khớp thì thông báo lỗi không đúng file
                                if (isValidTitle == false)
                                {
                                    throw new FileException(MISAResources.InValidMsg_FileInvalidFormat);

                                }
                            }
                        }
                    }
                    continue;
                }
                #endregion

                #region Chọn ra các trường để format lại
                var dateOfBirth = worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("DateOfBirth")]?.Value?.ToString()?.Trim();
                var identityDate = worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("IdentityDate")]?.Value?.ToString()?.Trim();
                var gender = worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("Gender")]?.Value?.ToString()?.Trim();
                #endregion

                #region Tạo mới một đối tương Nhân viên
                var employee = new EmployeeImportDto
                {
                    EmployeeCode = worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("EmployeeCode")]?.Value?.ToString()?.Trim(),
                    FullName = worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("FullName")]?.Value?.ToString().Trim(),
                    PhoneNumber = worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("PhoneNumber")]?.Value?.ToString()?.Trim(),
                    Address = worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("Address")]?.Value?.ToString()?.Trim(),
                    Email = worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("Email")]?.Value?.ToString()?.Trim(),
                    Gender = Common.ConvertEnumGender(gender),
                    DepartmentName = worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("DepartmentName")]?.Value?.ToString()?.Trim(),
                    PositionName = worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("PositionName")]?.Value?.ToString()?.Trim(),
                    DateOfBirth = Common.ValidateDateTime(dateOfBirth),
                    IdentityNumber = worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("IdentityNumber")]?.Value?.ToString()?.Trim(),
                    IdentityDate = Common.ValidateDateTime(identityDate),
                    IdentityPlace = worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("IdentityPlace")]?.Value?.ToString()?.Trim(),
                    BankAccount = worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("BankAccount")]?.Value?.ToString()?.Trim(),
                    BankName = worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("BankName")]?.Value?.ToString()?.Trim(),
                    BankBranch = worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("BankBranch")]?.Value?.ToString()?.Trim(),
                };
                #endregion

                #region Validate các trường là Reference table
                // Nếu tên vị trống => bỏ qua
                if (employee.PositionName != null)
                {

                    // Validate Vị trí công việc có tồn tại trong DB không
                    foreach (var position in listPosition)
                    {
                        if (Common.CompareRawString(position.PositionName, employee.PositionName))
                        {
                            employee.PositionId = position.PositionId;
                            employee.PositionName = position.PositionName;
                            break;
                        }
                    }

                    // Gán lỗi nghiệp vụ DB nếu có
                    if (employee.PositionId == null)
                    {
                        employee.MarkErrorCell.Add(Variable.columnImport.GetValueOrDefault("PositionName") + 1, MISAResources.InValidMsg_PositionInvalid);
                        employee.ErrorList.Add(MISAResources.InValidMsg_PositionInvalid);
                    }
                }

                // Nếu tên phòng ban trống => bỏ qua
                if (employee.DepartmentName != null)
                {

                    // Validate Phòng ban có tồn tại trong DB không
                    foreach (var department in listDepartment)
                    {
                        if (Common.CompareRawString(department.DepartmentName, employee.DepartmentName))
                        {
                            employee.DepartmentId = department.DepartmentId;
                            employee.DepartmentName = department.DepartmentName;
                            break;
                        }
                    }

                    // Gán lỗi nghiệp vụ DB nếu có
                    if (employee.DepartmentId == null)
                    {
                        if (!employee.ErrorList.Contains(MISAResources.InValidMsg_DepartmentRequired))
                        {
                            employee.MarkErrorCell.Add(Variable.columnImport.GetValueOrDefault("DepartmentName") + 1, MISAResources.InValidMsg_DepartmentInvalid);
                            employee.ErrorList.Add(MISAResources.InValidMsg_DepartmentInvalid);
                        }
                    }
                }
                #endregion

                #region Validate Datetime
                // Nếu ngày sinh null => kiểm tra
                if (employee.DateOfBirth == null)
                {
                    // Nếu người dùng có nhập mà null => không khớp định dạng
                    if (dateOfBirth != null)
                    {
                        employee.DateOfBirthString = dateOfBirth;
                        employee.MarkErrorCell.Add(Variable.columnImport.GetValueOrDefault("DateOfBirth") + 1, MISAResources.InValidMsg_DateOfBirthInvalid);
                        employee.ErrorList.Add(MISAResources.InValidMsg_DateOfBirthInvalid);
                    }
                }

                // Nếu Ngày cấp mà null => kiểm tra
                if (employee.IdentityDate == null)
                {
                    // Nếu người dùng có nhập mà null => không khớp định dạng
                    if (identityDate != null)
                    {
                        employee.IdentityDateString = identityDate;
                        employee.MarkErrorCell.Add(Variable.columnImport.GetValueOrDefault("IdentityDate") + 1, MISAResources.InValidMsg_DateIdentityInvalid);
                        employee.ErrorList.Add(MISAResources.InValidMsg_DateIdentityInvalid);
                    }
                }
                #endregion

                #region Validate DataAnnotation và lấy danh sách lỗi
                // Tạo mới một đối tượng ValidationContext để validate bằng DataAnnotations
                var validationResults = new List<ValidationResult>();
                var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(employee, null, null);
                var isValid = Validator.TryValidateObject(employee, validationContext, validationResults, true);

                // Gán lỗi DataAnnotation nếu có
                if (!isValid)
                {
                    foreach (var validationResult in validationResults)
                    {
                        if (validationResult.ErrorMessage != null)
                        {
                            var field = validationResult.MemberNames.FirstOrDefault();
                            if(!employee.MarkErrorCell.ContainsKey(Variable.columnImport.GetValueOrDefault(field) + 1))
                            {
                                employee.MarkErrorCell.Add(Variable.columnImport.GetValueOrDefault(field) + 1, validationResult.ErrorMessage);
                                employee.ErrorList.Add(validationResult.ErrorMessage);
                            }                         
                        }
                    }
                }
                #endregion

                #region Gán lại trường mã nhân viên nếu không có lỗi
                if (!employee.MarkErrorCell.ContainsKey(Variable.columnImport.GetValueOrDefault("EmployeeCode") + 1)){
                    var newCode = _validateEmployee.AutoFillEmployeeCode(employee.EmployeeCode);
                    employee.EmployeeCode = newCode;
                }
                #endregion

                #region Check trùng lặp trường Mã nhân viên
                // Nếu mã nhân viên trống => bỏ qua
                if (employee.EmployeeCode != null)
                {
                    // Check mã nhân viên đã tồn tại trong hệ thống hay chưa
                    var employeeTemp = listEmployee.FirstOrDefault(x => x.EmployeeCode == employee.EmployeeCode);
                    if (employeeTemp != null)
                    {
                        employee.MarkErrorCell.Add(Variable.columnImport.GetValueOrDefault("EmployeeCode") + 1, MISAResources.InValidMsg_EmployeeCodeInvalidImport);
                        employee.ErrorList.Add(MISAResources.InValidMsg_EmployeeCodeInvalidImport);
                    }
                    else
                    {
                        // Check mã nhân viên đã tồn tại trong file Excel của các dòng trước đó hay chưa
                        if (employees.Exists(emp => employee.EmployeeCode.Equals(emp.EmployeeCode)))
                        {
                            employee.MarkErrorCell.Add(Variable.columnImport.GetValueOrDefault("EmployeeCode") + 1, MISAResources.InValidMsg_EmployeeCodeInvalidImportInFile);
                            employee.ErrorList.Add(MISAResources.InValidMsg_EmployeeCodeInvalidImportInFile);
                        }
                    }
                }
                #endregion

                // Nếu không có lỗi nào thì gán mác là true => sẵn sang để Insert
                if (employee.ErrorList.Count == 0)
                {
                    employee.Status = true;
                }
                else
                {
                    employee.Status = false;
                }

                // Thêm vào danh sách nhân viên
                employees.Add(employee);
            }
            return employees;
        }

        public void MapEmployeeDataToFile(ExcelWorksheet worksheet, EmployeeModel employee, int row)
        {
            // Cộng 1 vì có thêm cột STT ở đầu
            // Gán giá trị tương ứng với từng cột giá trị của nhân viên
            worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("EmployeeCode") + 1].Value = employee.EmployeeCode;
            worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("FullName") + 1].Value = employee.FullName;
            worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("PhoneNumber") + 1].Value = employee.PhoneNumber;
            worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("Address") + 1].Value = employee.Address;
            worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("Email") + 1].Value = employee.Email;
            worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("Gender") + 1].Value = Common.ConvertEnumGenderToString(employee.Gender);
            worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("DepartmentName") + 1].Value = employee.Department.DepartmentName;
            worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("PositionName") + 1].Value = employee.Position?.PositionName;
            worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("DateOfBirth") + 1].Value = Common.ConvertDateTimeToString(employee.DateOfBirth);
            worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("IdentityNumber") + 1].Value = employee.IdentityNumber;
            worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("IdentityDate") + 1].Value = Common.ConvertDateTimeToString(employee.IdentityDate);
            worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("IdentityPlace") + 1].Value = employee.IdentityPlace;
            worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("BankAccount") + 1].Value = employee.BankAccount;
            worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("BankName") + 1].Value = employee.BankName;
            worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("BankBranch") + 1].Value = employee.BankBranch;

            // Căn giữa cột Ngày sinh
            worksheet.Column(Variable.columnImport.GetValueOrDefault("DateOfBirth") + 1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Column(Variable.columnImport.GetValueOrDefault("IdentityDate") + 1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            // Tự động điều chỉnh độ rộng cột
            //worksheet.Cells.AutoFitColumns();

            //Tạo border
            using (var range = worksheet.Cells[row, 1, row, worksheet.Dimension.End.Column])
            {
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            }
        }

        public void MapErrorDataToFile(ExcelWorksheet worksheet, EmployeeImportErrorDto employee, int row)
        {
            #region Gán giá trị tương ứng với từng cột giá trị của nhân viên
            // +1 vì có thêm cột STT
            worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("EmployeeCode") + 1].Value = employee.EmployeeCode;
            worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("FullName") + 1].Value = employee.FullName;
            worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("PhoneNumber") + 1].Value = employee.PhoneNumber;
            worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("Address") + 1].Value = employee.Address;
            worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("Email") + 1].Value = employee.Email;
            worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("Gender") + 1].Value = Common.ConvertEnumGenderToString(employee.Gender);
            worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("DepartmentName") + 1].Value = employee.DepartmentName;
            worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("PositionName") + 1].Value = employee.PositionName;

            // Kiểm tra tính hợp lệ để có cách hiển thị phù hợp cho ngày sinh
            if (employee.DateOfBirth != null)
            {
                worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("DateOfBirth") + 1].Value = Common.ConvertDateTimeToString(employee.DateOfBirth);
            }
            else
            {
                worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("DateOfBirth") + 1].Value = employee.DateOfBirthString;
            }
            worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("IdentityNumber") + 1].Value = employee.IdentityNumber;

            // Kiểm tra tính hợp lệ để có cách hiển thị phù hợp cho ngày cấp CCCD
            if (employee.IdentityDate != null)
            {
                worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("IdentityDate") + 1].Value = Common.ConvertDateTimeToString(employee.IdentityDate);
            }
            else
            {
                worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("IdentityDate") + 1].Value = employee.IdentityDateString;
            }

            worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("IdentityPlace") + 1].Value = employee.IdentityPlace;
            worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("BankAccount") + 1].Value = employee.BankAccount;
            worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("BankName") + 1].Value = employee.BankName;
            worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("BankBranch") + 1].Value = employee.BankBranch;
            #endregion

            #region Gán giá trị cột Danh sách lỗi 
            if (employee.ErrorList != null)
            {
                worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("ErrorList") + 1].Value = String.Join("\n", employee.ErrorList);
                // Set màu đỏ cho ô lỗi
                worksheet.Cells[row, Variable.columnImport.GetValueOrDefault("ErrorList") + 1].Style.Font.Color.SetColor(System.Drawing.Color.Red);

                // Set lại chiều cao của các dòng
                worksheet.Row(row).CustomHeight = true;
                worksheet.Row(row).Height = worksheet.Row(row).Height * employee.ErrorList.Count;
                worksheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }
            #endregion

            #region Căn chỉnh đẹp cho bảng excel
            worksheet.Column(Variable.columnImport.GetValueOrDefault("DateOfBirth") + 1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Column(Variable.columnImport.GetValueOrDefault("IdentityDate") + 1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            using (var range = worksheet.Cells[row, 1, row, worksheet.Dimension.End.Column])
            {
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            }

            if (employee.MarkErrorCell != null)
            {
                foreach (var markError in employee.MarkErrorCell)
                {
                    // Set border đỏ cho các ô có lỗi
                    worksheet.Cells[row, markError.Key].Style.Border.BorderAround(ExcelBorderStyle.Thick, Color.Red);

                    // Thêm comment lỗi cho từng ô
                    var comment = worksheet.Cells[row, markError.Key].AddComment(markError.Value);
                    comment.AutoFit = true;
                    comment.Font.Bold = true;
                    comment.Font.Color = Color.Red;
                }
            }

            //worksheet.View.FreezePanes(5, 1);
            #endregion
        }
        #endregion
    }
}
