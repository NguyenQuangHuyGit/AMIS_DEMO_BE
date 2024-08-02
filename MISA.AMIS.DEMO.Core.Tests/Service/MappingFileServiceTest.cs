using Microsoft.AspNetCore.Http;
using MISA.AMIS.DEMO.Core.Resource;
using NSubstitute;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core.UnitTests.Service
{
    [TestFixture]
    public class MappingFileServiceTest
    {
        #region Properties
        public IEmployeeRepository EmployeeRepository { get; set; }
        public IPositionRepository PositionRepository { get; set; }
        public IDepartmentRepository DepartmentRepository { get; set; }
        public IMappingFileService MappingFileServiceTestSub { get; set; }
        #endregion

        #region SetUp
        [SetUp]
        public void SetUp()
        {
            EmployeeRepository = Substitute.For<IEmployeeRepository>();
            PositionRepository = Substitute.For<IPositionRepository>();
            DepartmentRepository = Substitute.For<IDepartmentRepository>();
            MappingFileServiceTestSub = Substitute.For<MappingFileService>(EmployeeRepository, PositionRepository, DepartmentRepository);
        } 
        #endregion

        #region CheckFileImport
        [Test]
        public void CheckFileImport_ValidFile_Success()
        {
            // Arrange
            var file = Substitute.For<IFormFile>();
            Path.GetExtension(file.FileName).Returns(".xlsx");
            file.Length.Returns(30);

            // Act & Assert
            Assert.DoesNotThrow(() => MappingFileServiceTestSub.CheckFileImport(file));
        }

        [Test]
        public void CheckFileImport_InvalidFileExtension_Failure()
        {
            // Arrange
            var file = Substitute.For<IFormFile>();
            Path.GetExtension(file.FileName).Returns(".txt");
            file.Length.Returns(30);

            // Act & Assert
            var exception = Assert.Throws<FileException>(() => MappingFileServiceTestSub.CheckFileImport(file));
            Assert.That(exception.Message, Is.EqualTo(MISAResourceVN.InValidMsg_FileInvalidType));
        }

        [Test]
        public void CheckFileImport_InvalidFileLength_Failure()
        {
            // Arrange
            var file = Substitute.For<IFormFile>();
            Path.GetExtension(file.FileName).Returns(".xlsx");
            file.Length.Returns(0);

            // Act & Assert
            var exception = Assert.Throws<FileException>(() => MappingFileServiceTestSub.CheckFileImport(file));
            Assert.That(exception.Message, Is.EqualTo(MISAResourceVN.InValidMsg_FileInvalidLenght));
        } 
        #endregion
    }
}
