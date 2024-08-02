using AutoMapper;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MISA.AMIS.DEMO.Core.Resource;
using MISA.AMIS.DEMO.Infrastructure;
using Moq;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.Extensions;
using NSubstitute.ReturnsExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core.UnitTests
{
    [TestFixture]
    public class EmployeeServiceTest
    {
        #region Property
        public IEmployeeService EmployeeServiceSub { get; set; }
        public IExcelService ExcelService { get; set; }
        public IValidateEmployee ValidateEmployee { get; set; }
        public IEmployeeRepository EmployeeRepository { get; set; }
        public IUnitOfWork UnitOfWork { get; set; }
        public IMemoryCache MemoryCache { get; set; }
        public IMapper Mapper { get; set; }
        public IMappingFileService MappingFileService { get; set; } 
        #endregion

        #region SetUp
        [SetUp]
        public void Setup()
        {
            ExcelService = Substitute.For<IExcelService>();
            ValidateEmployee = Substitute.For<IValidateEmployee>();
            MappingFileService = Substitute.For<IMappingFileService>();
            EmployeeRepository = Substitute.For<IEmployeeRepository>();
            UnitOfWork = Substitute.For<IUnitOfWork>();
            MemoryCache = Substitute.For<IMemoryCache>();
            Mapper = Substitute.For<IMapper>();
            EmployeeServiceSub = Substitute.For<EmployeeService>(MappingFileService, ExcelService, ValidateEmployee, MemoryCache, EmployeeRepository, UnitOfWork, Mapper);
        } 
        #endregion

        #region Methods
        #region GetNewEmployeeCodeAsync
        [Test]
        public async Task GetNewEmployeeCodeAsync_LastestCodeNull_NewCode()
        {
            // Arrange
            EmployeeRepository.GetLastestEmployeeCode().ReturnsNull();

            // Act
            var result = await EmployeeServiceSub.GetNewEmployeeCodeAsync();

            // Assert
            Assert.That(result, Is.EqualTo("NV-000001"));
            await EmployeeRepository.Received(1).GetLastestEmployeeCode();
        }

        [Test]
        public async Task GetNewEmployeeCodeAsync_LastestCodeValid_NewCode()
        {
            // Arrange
            EmployeeRepository.GetLastestEmployeeCode().Returns("NV-000344");

            // Act
            var result = await EmployeeServiceSub.GetNewEmployeeCodeAsync();

            // Assert
            Assert.That(result, Is.EqualTo("NV-000345"));
            await EmployeeRepository.Received(1).GetLastestEmployeeCode();
        }

        [Test]
        public async Task GetNewEmployeeCodeAsync_LastestCodeValid_ThrowException()
        {
            // Arrange
            EmployeeRepository.GetLastestEmployeeCode().Returns("NV-dshajd");

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await EmployeeServiceSub.GetNewEmployeeCodeAsync());
            await EmployeeRepository.Received(1).GetLastestEmployeeCode();
        }
        #endregion

        #region GetFillterEmployeeAsync
        [Test]
        [TestCase(1, 2, "")]
        [TestCase(1, 2, null)]
        public async Task GetFillterEmployeeAsync_ValidInput_Success(int pageSize, int pageNumber, string? fillterString)
        {
            // Arrange
            EmployeeRepository.GetFillterEmployeeAsync(pageSize, pageNumber, fillterString).Returns(new EmployeeFillterModel());

            // Act
            var result = await EmployeeServiceSub.GetFillterEmployeeAsync(pageSize, pageNumber, fillterString);

            // Assert
            Assert.That(result, Is.InstanceOf<EmployeeFillterDto>());
            await EmployeeRepository.Received(1).GetFillterEmployeeAsync(pageSize, pageNumber, fillterString);
        }

        [Test]
        [TestCase(-10, 0, "")]
        public async Task GetFillterEmployeeAsync_InValidInput_Success(int pageSize, int pageNumber, string? fillterString)
        {
            // Arrange
            EmployeeRepository.GetFillterEmployeeAsync(10, 1, fillterString).Returns(new EmployeeFillterModel());

            // Act
            var result = await EmployeeServiceSub.GetFillterEmployeeAsync(pageSize, pageNumber, fillterString);

            // Assert
            Assert.That(result, Is.InstanceOf<EmployeeFillterDto>());
            await EmployeeRepository.Received(1).GetFillterEmployeeAsync(10, 1, fillterString);
        }
        #endregion

        #region ValidateImportEmployeeList
        [Test]
        public async Task ValidateImportEmployeeList_NoneValidData_NewImportDto()
        {
            // Arrange
            var file = Substitute.For<IFormFile>();
            MappingFileService.CheckFileImport(file);
            ExcelService.ReadExcelFile<EmployeeImportDto>(file, MappingFileService.GetEmployeeDataAndValidateFromFile).Returns(new List<EmployeeImportDto>());

            // Act
            var result = await EmployeeServiceSub.ValidateImportEmployeeList(file);

            // Assert
            Assert.That(result, Is.InstanceOf<ImportDto<EmployeeImportDto>>());
            await ExcelService.Received(1).ReadExcelFile<EmployeeImportDto>(file, MappingFileService.GetEmployeeDataAndValidateFromFile);
            MemoryCache.Received(0);
        }

        [Test]
        public async Task ValidateImportEmployeeList_HaveValidData_NewImportDto()
        {
            // Arrange
            var file = Substitute.For<IFormFile>();
            MappingFileService.CheckFileImport(file);
            ExcelService.ReadExcelFile<EmployeeImportDto>(file, MappingFileService.GetEmployeeDataAndValidateFromFile).Returns(new List<EmployeeImportDto>());
            var employees = new List<Employee>()
            {
                new Employee()
                {
                    FullName = "Quang Huy",
                },
            };
            Mapper.ReturnsForAll(employees);

            // Act
            var result = await EmployeeServiceSub.ValidateImportEmployeeList(file);

            // Assert
            Assert.That(result, Is.InstanceOf<ImportDto<EmployeeImportDto>>());
            await ExcelService.Received(1).ReadExcelFile<EmployeeImportDto>(file, MappingFileService.GetEmployeeDataAndValidateFromFile);
            MemoryCache.Received(1);
        }
        #endregion

        #region ExportEmployee
        [Test]
        public async Task ExportAllEmployee_ValidInput_Success()
        {
            // Arrange
            var employees = new List<EmployeeModel>();
            EmployeeRepository.GetAllEmployeeModelAsync().Returns(employees);
            ExcelService.WriteExcelFile(Variable.headersImport, Variable.fileImportEmployeeName, employees.ToList(), MappingFileService.MapEmployeeDataToFile).Returns(new byte[10]);

            // Act
            var result = await EmployeeServiceSub.ExportAllEmployee();

            // Assert
            Assert.That(result, Is.Not.Null);
            ExcelService.Received(1);
        }

        [Test]
        public async Task ExportSelectedEmployee_NotEmptyListId_Success()
        {
            // Arrange
            var ids = new List<Guid>() { Guid.NewGuid() };
            EmployeeRepository.GetEmployeeModelAsync(ids).Returns(new List<EmployeeModel>());
            ExcelService.WriteExcelFile(Variable.headersImport, Variable.fileImportEmployeeName, Arg.Any<List<EmployeeModel>>(), MappingFileService.MapEmployeeDataToFile).Returns(new byte[10]);

            // Act
            var result = await EmployeeServiceSub.ExportSelectedEmployee(ids);

            // Assert
            Assert.That(result, Is.Not.Null);
            ExcelService.Received(1).WriteExcelFile(Arg.Any<List<string>>(), Arg.Any<string>(), Arg.Any<List<EmployeeModel>>(), MappingFileService.MapEmployeeDataToFile);
            await EmployeeRepository.Received(1).GetEmployeeModelAsync(ids);
        }

        [Test]
        public async Task ExportSelectedEmployee_EmptyListId_Failture()
        {
            // Arrange
            var ids = new List<Guid>();

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await EmployeeServiceSub.ExportSelectedEmployee(ids));
            ExcelService.Received(0).WriteExcelFile(Arg.Any<List<string>>(), Arg.Any<string>(), Arg.Any<List<EmployeeModel>>(), MappingFileService.MapEmployeeDataToFile);
            await EmployeeRepository.Received(0).GetEmployeeModelAsync(ids);
        }
        #endregion

        #region GetExampleFileEmployee
        [Test]
        public void GetExampleFileEmployee_ValidInputSuccess()
        {
            // Arrange
            ExcelService.WriteExcelFile<EmployeeModel>(Variable.headersImport, Variable.fileImportEmployeeName, null, null).Returns(new byte[10]);

            // Act
            var result = EmployeeServiceSub.GetExampleFileEmployee();

            // Assert
            Assert.That(result, Is.Not.Null);
            ExcelService.Received(1);
        }
        #endregion

        #region GetFileErrorList
        [Test]
        public void GetFileErrorList_ValidInput_Sucess()
        {
            // Arrange
            var listEmployee = new List<EmployeeImportErrorDto>();
            ExcelService.WriteExcelFile(Variable.headersImport, Variable.fileImportEmployeeName, listEmployee, MappingFileService.MapErrorDataToFile).Returns(new byte[10]);

            // Act
            var result = EmployeeServiceSub.GetFileErrorList(listEmployee);

            // Assert
            Assert.That(result, Is.Not.Null);
            ExcelService.Received(1);
        }
        #endregion

        #region ImportEmployee
        [Test]
        public async Task ImportEmployee_ValidKeyImport_Success()
        {
            // Arrange
            var keyImport = Guid.NewGuid();
            MemoryCache.TryGetValue(keyImport, out Arg.Any<object>()).Returns(rs =>
            {
                rs[1] = new List<Employee>() { new Employee() };
                return true;
            });

            // Act
            var result = await EmployeeServiceSub.ImportEmployee(keyImport);

            // Assert
            Assert.That(result, Is.Not.Null);
            await UnitOfWork.EmployeeRepository.Received(1).InsertAsync(Arg.Any<List<Employee>>());
            await UnitOfWork.Received(1).CommitAsync();
            await UnitOfWork.Received(0).RollbackAsync();
            await UnitOfWork.Received(1).DisposeAsync();
            MemoryCache.Received(1).Remove(keyImport);
        }

        [Test]
        public async Task ImportEmployee_ListEmployeeIsEmpty_Failture()
        {
            // Arrange
            var keyImport = Guid.NewGuid();
            MemoryCache.TryGetValue(keyImport, out Arg.Any<object>()).Returns(rs =>
            {
                rs[1] = new List<Employee>() { };
                return true;
            });

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await EmployeeServiceSub.ImportEmployee(keyImport));
            await UnitOfWork.EmployeeRepository.Received(0).InsertAsync(Arg.Any<List<Employee>>());
            await UnitOfWork.Received(0).CommitAsync();
            MemoryCache.Received(1).Remove(keyImport);
        }

        [Test]
        public async Task ImportEmployee_ValidKeyImport_Failture()
        {
            // Arrange
            var keyImport = Guid.NewGuid();
            MemoryCache.TryGetValue(keyImport, out Arg.Any<object>()).Returns(rs =>
            {
                rs[1] = new List<Employee>() { new Employee() };
                return true;
            });
            UnitOfWork.EmployeeRepository.InsertAsync(Arg.Any<List<Employee>>()).ThrowsAsync<Exception>();

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await EmployeeServiceSub.ImportEmployee(keyImport));
            await UnitOfWork.EmployeeRepository.Received(1).InsertAsync(Arg.Any<List<Employee>>());
            await UnitOfWork.Received(0).CommitAsync();
            await UnitOfWork.Received(1).RollbackAsync();
            await UnitOfWork.Received(1).DisposeAsync();
            MemoryCache.Received(1).Remove(keyImport);
        }

        [Test]
        public async Task ImportEmployee_InValidKeyImport_Failture()
        {
            // Arrange
            var keyImport = Guid.NewGuid();
            MemoryCache.TryGetValue(keyImport, out Arg.Any<object>()).Returns(rs =>
            {
                return false;
            });

            // Act & Assert
            var exception = Assert.ThrowsAsync<SessionTimeOutException>(async () => await EmployeeServiceSub.ImportEmployee(keyImport));
            Assert.That(exception.DevMessage, Is.EqualTo(MISAResourceVN.InValidMsg_ImportDataNotExistInCache));
            await UnitOfWork.EmployeeRepository.Received(0).InsertAsync(Arg.Any<List<Employee>>());
            MemoryCache.Received(0).Remove(keyImport);
        }
        #endregion

        #region CancelImportSession
        [Test]
        public void CancelImportSession_ValidInput_Success()
        {
            // Arrange
            var keyImport = Guid.NewGuid();

            // Act
            EmployeeServiceSub.CancelImportSession(keyImport);

            // Assert
            MemoryCache.Received(1).Remove(keyImport);
        }
        #endregion

        #region GetImportSuccessFile
        [Test]
        public async Task GetImportSuccessFile_ValidInput_Success()
        {
            // Arrange
            var listId = new List<Guid>();
            var employees = new List<EmployeeModel>();
            EmployeeRepository.GetEmployeeModelAsync(listId).Returns(employees);
            ExcelService.WriteExcelFile(Variable.headersImport, Variable.fileSuccessImportEmployeeName, employees, MappingFileService.MapEmployeeDataToFile).Returns(new byte[10]);

            // Act
            var result = await EmployeeServiceSub.GetImportSuccessFile(listId);

            // Assert
            Assert.That(result, Is.Not.Null);
            await EmployeeRepository.Received(1).GetEmployeeModelAsync(listId);
        }
        #endregion 
        #endregion
    }
}
