using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualBasic;
using MISA.AMIS.DEMO.Core.Resources;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public class EmployeeService : BaseService<EmployeeCreateDto, EmployeeDto, Employee>, IEmployeeService
    {
        #region Fields
        private readonly IExcelService _excelService;
        private readonly IValidateEmployee _validateEmployee;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _memoryCache;
        private readonly IMappingFileService _mappingFileService;
        #endregion

        #region Contructor
        public EmployeeService(IMappingFileService mappingFileService, IExcelService excelService, IValidateEmployee validateEmployee, IMemoryCache memoryCache, IEmployeeRepository baseRepository, IUnitOfWork unitOfWork, IMapper mapper) : base(baseRepository, mapper)
        {
            _excelService = excelService;
            _validateEmployee = validateEmployee;
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
            _mappingFileService = mappingFileService;
        }
        #endregion

        #region Public Methods
        public async Task<string> GetNewEmployeeCodeAsync()
        {
            var result = await ((IEmployeeRepository)_baseRepository).GetLastestEmployeeCode();
            var prefix = "NV-";
            if (result == null)
            {
                return $"{prefix}000001";
            }
            var num = result.Substring(3);
            if (Int32.TryParse(num, out int rs))
            {
                var newNum = (rs + 1).ToString();
                while (newNum.Length < 6)
                {
                    newNum = newNum.Insert(0, "0");
                }
                return $"{prefix}{newNum}";
            }
            else
            {
                throw new Exception(MISAResources.InValidMsg_CantGetNewCode);
            }
        }

        public async Task<EmployeeFillterDto> GetFillterEmployeeAsync(int pageSize, int pageNumber, string? fillterString)
        {
            // Kiểm tra và gán lại giá trị tùy chọn cho phân trang nếu là số âm
            if (pageSize <= 0)
            {
                pageSize = 10;
            }
            if (pageNumber <= 0)
            {
                pageNumber = 1;
            }
            var result = await ((IEmployeeRepository)_baseRepository).GetFillterEmployeeAsync(pageSize, pageNumber, fillterString);
            var resultDto = _mapper.Map<IEnumerable<EmployeeDto>>(result.Employees);
            var actualResult = new EmployeeFillterDto()
            {
                Employees = resultDto,
                TotalRecord = result.TotalRecord,
            };
            return actualResult;
        }

        public async Task<ImportDto<EmployeeImportDto>> ValidateImportEmployeeList(IFormFile fileImport)
        {
            // Kiểm tra file
            _mappingFileService.CheckFileImport(fileImport);

            // Lấy danh sách nhân viên từ file Excel đã validate
            var results = await _excelService.ReadExcelFile<EmployeeImportDto>(fileImport, _mappingFileService.GetEmployeeDataAndValidateFromFile);

            // Tạo id mới cho dữ liệu cần nhập khẩu
            var keyImport = Guid.NewGuid();
            var keyImportError = Guid.NewGuid();

            // Tạo mới phản hồi cho dữ liệu nhập khẩu
            var importResponse = new ImportDto<EmployeeImportDto>()
            {
                ListObject = results,
                ImportKey = keyImport,
            };

            // Map dữ liệu hợp lệ sang Entity
            var entities = _mapper.Map<List<Employee>>(results.Where(x => x.Status == true));

            // Nếu có bản ghi hợp lệ sẵn sàng nhập khẩu thì mới lưu vào cache
            if (entities != null && entities.Count > 0)
            {
                // Ghi vào Cache
                _memoryCache.Set(keyImport, entities, DateTime.Now.AddHours(1));
            }

            // Trả về giá trị
            return importResponse;
        }

        public async Task<byte[]> ExportAllEmployee()
        {
            var currentCulture = CultureInfo.CurrentCulture.ToString();

            // Lấy danh sách tất cả nhân viên để xuất khẩu
            var employees = await ((IEmployeeRepository)_baseRepository).GetAllEmployeeModelAsync();

            // Dựng file excel
            var file = _excelService.WriteExcelFile(Variable.headersImport[currentCulture], Variable.fileImportEmployeeTitle[currentCulture], employees.ToList(), _mappingFileService.MapEmployeeDataToFile);

            return file;
        }

        public async Task<byte[]> ExportSelectedEmployee(List<Guid> ids)
        {
            if (ids != null && ids.Count > 0)
            {
                var currentCulture = CultureInfo.CurrentCulture.ToString();

                // Lấy danh sách tất cả nhân viên để xuất khẩu
                var employees = await ((IEmployeeRepository)_baseRepository).GetEmployeeModelAsync(ids);

                // Dựng file excel
                var file = _excelService.WriteExcelFile(Variable.headersImport[currentCulture], Variable.fileImportEmployeeTitle[currentCulture], employees.ToList(), _mappingFileService.MapEmployeeDataToFile);

                return file;
            }
            else
            {
                throw new Exception(MISAResources.InValidMsg_SystemError);
            }
        }

        public byte[] GetExampleFileEmployee()
        {
            var currentCulture = CultureInfo.CurrentCulture.ToString();
            // Dựng file excel
            var file = _excelService.WriteExcelFile<EmployeeModel>(Variable.headersImport[currentCulture], Variable.fileImportEmployeeTitle[currentCulture], null, null);

            return file;
        }

        public byte[] GetFileErrorList(List<EmployeeImportErrorDto> listEmployee)
        {
            // Lấy ngôn ngữ hiện tại đang sử dụng
            var currentCulture = CultureInfo.CurrentCulture.ToString();
            var headers = Variable.headersImport[currentCulture].ToList();
            headers.Add(Variable.statusColumn[currentCulture]);

            // Dựng file excel
            var file = _excelService.WriteExcelFile<EmployeeImportErrorDto>(headers, Variable.fileErrorImportEmployeeTitle[currentCulture], listEmployee, _mappingFileService.MapErrorDataToFile);

            return file;
        }

        public async Task<List<Guid>> ImportEmployee(Guid keyImport)
        {
            if (_memoryCache.TryGetValue(keyImport, out var entities))
            {
                var employees = (List<Employee>)entities;
                if (employees == null || employees.Count == 0)
                {
                    _memoryCache.Remove(keyImport);
                    throw new Exception(MISAResources.InValidMsg_SystemError);
                }
                var listIds = new List<Guid>();
                foreach (var entity in employees)
                {
                    entity.EmployeeId = Guid.NewGuid();
                    entity.CreatedDate = DateTime.Now;
                    listIds.Add(entity.EmployeeId);
                }
                await _unitOfWork.BeginTransactionAsync();
                try
                {
                    await _unitOfWork.EmployeeRepository.InsertAsync(employees);
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception)
                {
                    await _unitOfWork.RollbackAsync();
                    listIds = new List<Guid>();
                    throw new Exception(MISAResources.InValidMsg_SystemError);
                }
                finally
                {
                    await _unitOfWork.DisposeAsync();
                    _memoryCache.Remove(keyImport);
                }
                return listIds;
            }
            else
            {
                throw new SessionTimeOutException()
                {
                    UserMessage = MISAResources.InValidMsg_ImportTImeOut,
                    DevMessage = MISAResources.InValidMsg_ImportDataNotExistInCache
                };
            }
        }

        public void CancelImportSession(Guid keyImport)
        {
            _memoryCache.Remove(keyImport);
        }

        public async Task<byte[]> GetImportSuccessFile(List<Guid> ids)
        {
            var currentCulture = CultureInfo.CurrentCulture.ToString();

            // Lấy danh sách nhân viên
            var employees = await ((IEmployeeRepository)_baseRepository).GetEmployeeModelAsync(ids);

            // Dựng file excel
            var file = _excelService.WriteExcelFile(Variable.headersImport[currentCulture], Variable.fileSuccessImportEmployeeTitle[currentCulture], employees.ToList(), _mappingFileService.MapEmployeeDataToFile);

            return file;
        }
        #endregion

        #region Protected Override Methods
        protected override async Task ValidateCreateBusiness(EmployeeCreateDto dto)
        {
            var tasks = new List<Task>()
            {
                _validateEmployee.ValidateDuplicateEmployeeData(dto),
                _validateEmployee.ValidateReferenceData(dto),
            };
            await Task.WhenAll(tasks);
        }

        protected override async Task ValidateUpdateBusiness(Guid id, EmployeeCreateDto dto)
        {
            var employee = await _baseRepository.GetAsync(id);
            if (employee != null)
            {
                if (!dto.EmployeeCode.Equals(employee.EmployeeCode))
                {
                    await _validateEmployee.ValidateDuplicateEmployeeData(dto);
                }
                await _validateEmployee.ValidateReferenceData(dto);
            }
            else
            {
                throw new Exception("Not Exist Employee");
            }
        }
        #endregion
    }
}
