using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    /// <summary>
    /// Profile khi AutoMapping Position
    /// </summary>
    public class PositionProfile : Profile
    {
        public PositionProfile()
        {
            CreateMap<Position, PositionDto>();
            CreateMap<PositionCreateDto, Position>();
        }
    }
}
