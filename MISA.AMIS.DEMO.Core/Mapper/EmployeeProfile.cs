using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeModel, EmployeeDto>()
                .ForMember((dest) => dest.DepartmentId, opt => opt.MapFrom<Guid>(src => src.Department.DepartmentId))
                .ForMember((dest) => dest.PositionId, opt => opt.MapFrom<Guid>(src => src.Position.PositionId))
                .ForMember((dest) => dest.DepartmentName, opt => opt.MapFrom<string>(src => src.Department.DepartmentName))
                .ForMember((dest) => dest.PositionName, opt => opt.MapFrom<string>(src => src.Position.PositionName));
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.DepartmentName, opt => opt.Ignore());
            CreateMap<EmployeeCreateDto, Employee>()
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore());
            CreateMap<EmployeeImportDto, Employee>()
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore());
        }
    }
}
