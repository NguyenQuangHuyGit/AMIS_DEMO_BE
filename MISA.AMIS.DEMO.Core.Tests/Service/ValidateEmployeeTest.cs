using MISA.AMIS.DEMO.Core.Resource;
using MISA.AMIS.DEMO.Infrastructure;
using NSubstitute;
using NSubstitute.Core;
using NSubstitute.ReturnsExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core.UnitTests.Service
{
    [TestFixture]
    public class ValidateEmployeeTest
    {
        #region Properties
        public IEmployeeRepository EmployeeRepository { get; set; }
        public IPositionRepository PositionRepository { get; set; }
        public IDepartmentRepository DepartmentRepository { get; set; }
        public IValidateEmployee ValidateEmployeeSub { get; set; } 
        #endregion

        #region SetUp
        [SetUp]
        public void SetUp()
        {
            EmployeeRepository = Substitute.For<IEmployeeRepository>();
            PositionRepository = Substitute.For<IPositionRepository>();
            DepartmentRepository = Substitute.For<IDepartmentRepository>();
            ValidateEmployeeSub = Substitute.For<ValidateEmployee>(EmployeeRepository, PositionRepository, DepartmentRepository);
        } 
        #endregion

        #region ValidateDuplicateEmployeeData
        [Test]
        public async Task ValidateDuplicateEmployeeData_NoDuplicateCode_Success()
        {
            // Arrange
            var createDto = new EmployeeCreateDto();
            EmployeeRepository.FindByFieldAsync("EmployeeCode", createDto.EmployeeCode).ReturnsNull();

            // Act & Assert
            Assert.DoesNotThrowAsync(async () => await ValidateEmployeeSub.ValidateDuplicateEmployeeData(createDto));
            await EmployeeRepository.Received(1).FindByFieldAsync("EmployeeCode", createDto.EmployeeCode);
        }

        [Test]
        public async Task ValidateDuplicateEmployeeData_DuplicateCode_Failture()
        {
            // Arrange
            var createDto = new EmployeeCreateDto();
            EmployeeRepository.FindByFieldAsync("EmployeeCode", createDto.EmployeeCode).Returns(new Employee());

            // Act & Assert
            var exception = Assert.ThrowsAsync<DuplicateException>(async () => await ValidateEmployeeSub.ValidateDuplicateEmployeeData(createDto));
            Assert.That(exception.UserMessage, Is.EqualTo(MISAResourceVN.InValidMsg_DuplicateInput));
            await EmployeeRepository.Received(1).FindByFieldAsync("EmployeeCode", createDto.EmployeeCode);

        }
        #endregion

        #region ValidateReferenceData
        [Test]
        public async Task ValidateReferenceData_DataExist_Success()
        {
            // Arrange
            var createDto = new EmployeeCreateDto()
            {
                PositionName = "quang",
                DepartmentName = "huy"
            };
            PositionRepository.FindByFieldAsync("PositionName", Arg.Any<string>()).Returns(new Position());
            DepartmentRepository.FindByFieldAsync("DepartmentName", Arg.Any<string>()).Returns(new Department());

            // Act & Assert
            Assert.DoesNotThrowAsync(async () => await ValidateEmployeeSub.ValidateReferenceData(createDto));
            await PositionRepository.Received(1).FindByFieldAsync("PositionName", Arg.Any<string>());
            await DepartmentRepository.Received(1).FindByFieldAsync("DepartmentName", Arg.Any<string>());
        }

        [Test]
        public async Task ValidateReferenceData_DataNotExist_Failture()
        {
            // Arrange
            var createDto = new EmployeeCreateDto()
            {
                PositionName = "quang",
                DepartmentName = "huy"
            };
            PositionRepository.FindByFieldAsync("PositionName", Arg.Any<string>()).ReturnsNull();

            // Act & Assert
            var exception = Assert.ThrowsAsync<NotExistException>(async () => await ValidateEmployeeSub.ValidateReferenceData(createDto));
            Assert.That(exception.UserMessage, Is.EqualTo(MISAResourceVN.InValidMsg_NotExistInput));
            await PositionRepository.Received(1).FindByFieldAsync("PositionName", Arg.Any<string>());
            await DepartmentRepository.Received(1).FindByFieldAsync("DepartmentName", Arg.Any<string>());
        } 
        #endregion
    }
}
