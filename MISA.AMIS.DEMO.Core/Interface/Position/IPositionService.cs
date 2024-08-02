using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public interface IPositionService : IBaseService<PositionCreateDto, PositionDto, Position>
    {
        /// <summary>
        /// Hàm kiểm tra Ví trí có tên truyền vào có tồn tại không
        /// </summary>
        /// <param name="name">Tên vị trí</param>
        /// <returns>Dto vị trí nếu tồn tại || NULL nếu không tìm thấy</returns>
        public Task<PositionDto?> CheckExistByNameAsync(string name);
    }
}
