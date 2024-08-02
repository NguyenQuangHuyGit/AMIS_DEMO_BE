using AutoMapper;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace MISA.AMIS.DEMO.Core.UnitTests.Service
{
    [TestFixture]
    public class BaseServiceTest
    {
        #region Properties
        public IBaseRepository<Employee> BaseRepository { get; set; }
        public IMapper Mapper { get; set; }
        public IBaseService<EmployeeCreateDto, EmployeeDto, Employee> BaseServiceSub { get; set; }
        #endregion

        #region SetUp
        [SetUp]
        public void SetUp()
        {
            BaseRepository = Substitute.For<IBaseRepository<Employee>>();
            Mapper = Substitute.For<IMapper>();
            BaseServiceSub = Substitute.For<BaseService<EmployeeCreateDto, EmployeeDto, Employee>>(BaseRepository, Mapper);
        }
        #endregion
    }
}
