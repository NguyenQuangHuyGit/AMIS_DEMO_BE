using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public class DepartmentService : BaseService<DepartmentCreateDto, DepartmentDto, Department>, IDepartmentService
    {
        /// <summary>
        /// Hàm tạo
        /// </summary>
        /// <param name="baseRepository">Repo Phòng ban</param>
        /// <param name="mapper">AutoMapper</param>
        public DepartmentService(IDepartmentRepository baseRepository, IMapper mapper) : base(baseRepository, mapper)
        {
        }

        public async Task<DepartmentDto?> CheckExistByName(string name)
        {
            var result = await _baseRepository.FindByFieldAsync("DepartmentName", name);
            var resultDto = _mapper.Map<DepartmentDto?>(result);
            return resultDto;
        }
    }
}
