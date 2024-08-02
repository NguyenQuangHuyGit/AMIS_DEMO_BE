using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public class PositionService : BaseService<PositionCreateDto, PositionDto, Position>, IPositionService
    {
        /// <summary>
        /// Hàm tạo
        /// </summary>
        /// <param name="baseRepository">Repo vị trí, công việc</param>
        /// <param name="mapper">AutoMapper</param>
        public PositionService(IPositionRepository baseRepository, IMapper mapper) : base(baseRepository, mapper)
        {
        }

        public async Task<PositionDto?> CheckExistByNameAsync(string name)
        {
            var result = await _baseRepository.FindByFieldAsync("PositionName", name);
            var resultDto = _mapper.Map<PositionDto?>(result);
            return resultDto;
        }
    }
}
