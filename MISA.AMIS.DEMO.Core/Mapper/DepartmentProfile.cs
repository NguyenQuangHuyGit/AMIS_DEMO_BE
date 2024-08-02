using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    /// <summary>
    /// Profile của phòng ban dùng khi AutoMapping
    /// </summary>
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<Department, DepartmentDto>();
            CreateMap<DepartmentCreateDto, Department>();
        }
    }
}
