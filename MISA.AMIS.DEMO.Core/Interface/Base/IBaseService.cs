using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public interface IBaseService<TCreateDto, TDto, TEntity> where TEntity : class
    {
        /// <summary>
        /// Hàm xử lý dữ liệu khi lấy tất cả danh sách 1 đối tượng
        /// </summary>
        /// <returns>Danh sách 1 đối tượng</returns>
        /// CreatedBy: QuangHuy (25/12/2023)
        Task<IEnumerable<TDto>> GetAsync();

        /// <summary>
        /// Hàm xử lý dữ liệu khi lấy 1 đối tượng theo Id
        /// </summary>
        /// <param name="id">Id đối tượng</param>
        /// <returns>Thông tin 1 đối tượng</returns>
        /// CreatedBy: QuangHuy (25/12/2023)
        Task<TDto?> GetAsync(Guid id);

        /// <summary>
        /// Hàm xử lý dữ liệu khi thêm mới 1 đối tượng
        /// </summary>
        /// <param name="entity">Entity tương ứng với đối tượng</param>
        /// CreatedBy: QuangHuy (25/12/2023)
        Task InsertAsync(TCreateDto createDto);

        /// <summary>
        /// Hàm xử lý dữ liệu khi cập nhật thông tin 1 đối tượng
        /// </summary>
        /// <param name="id">Id đối tượng</param>
        /// <param name="createDto">CreateDto tương ứng với đối tượng</param>
        /// CreatedBy: QuangHuy (25/12/2023)
        Task UpdateAsync(Guid id, TCreateDto createDto);

        /// <summary>
        /// Hàm xử lý dữ liệu khi xóa thông tin 1 đối tượng theo id
        /// </summary>
        /// <param name="id">Id đối tượng</param>
        /// CreatedBy: QuangHuy (25/12/2023)
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Hàm xử lý dữ liệu 1 danh sách đối tượng để xóa
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task DeleteAsync(List<Guid> ids);
    }
}
