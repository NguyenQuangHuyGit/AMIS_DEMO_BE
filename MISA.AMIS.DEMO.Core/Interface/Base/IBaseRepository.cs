using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Hàm lấy tất cả danh sách 1 đối tượng
        /// </summary>
        /// <returns>Danh sách 1 đối tượng</returns>
        /// CreatedBy: QuangHuy (25/12/2023)
        public Task<IEnumerable<TEntity>> GetAsync();

        /// <summary>
        /// Hàm lấy ra 1 đối tượng theo Id
        /// </summary>
        /// <param name="id">Id đối tượng</param>
        /// <returns>Thông tin 1 đối tượng</returns>
        /// CreatedBy: QuangHuy (25/12/2023)
        public Task<TEntity?> GetAsync(Guid id);

        /// <summary>
        /// Hàm lấy danh sách đối tượng theo danh sách Id
        /// </summary>
        /// <param name="id">Danh sách Id</param>
        /// <returns>Danh sách đối tượng</returns>
        /// CreatedBy: QuangHuy (25/01/2023)
        public Task<IEnumerable<TEntity>?> GetAsync(List<Guid> ids);

        /// <summary>
        /// Hàm lấy thông tin 1 đối tương theo 1 trường bất kỳ của đối tượng
        /// </summary>
        /// <param name="value">Giá trị của trường đối tượng tương ứng</param>
        /// <param name="field">Trường đối tượng tương ứng</param>
        /// <returns>Bản ghi đầu tiên tìm thấy được || NULL</returns>
        /// CreatedBy: QuangHuy(08/01/2024)
        public Task<TEntity?> FindByFieldAsync(string field, string value);

        /// <summary>
        /// Hàm thêm mới 1 đối tượng
        /// </summary>
        /// <param name="entity">Entity tương ứng với đối tượng</param>
        /// CreatedBy: QuangHuy (25/12/2023)
        public Task InsertAsync(TEntity entity);

        /// <summary>
        /// Hàm thêm mới 1 danh sách đối tượng
        /// </summary>
        /// <param name="entities">Danh sách đối tượng</param>
        /// CreatedBy: QuangHuy (25/12/2023)
        public Task InsertAsync(List<TEntity> entities);

        /// <summary>
        /// Hàm cập nhật thông tin 1 đối tượng
        /// </summary>
        /// <param name="id">Id đối tượng</param>
        /// <param name="entity">Entity tương ứng với đối tượng</param>
        /// CreatedBy: QuangHuy (25/12/2023)
        Task UpdateAsync(Guid id, TEntity entity);

        /// <summary>
        /// Hàm xóa thông tin 1 đối tượng theo id
        /// </summary>
        /// <param name="id">Id đối tượng</param>
        /// CreatedBy: QuangHuy (25/12/2023)
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Hàm xóa thông tin nhiều đối tượng theo id
        /// </summary>
        /// <param name="ids">Danh sách Id các đối tượng cần xóa</param>
        /// CreatedBy: QuangHuy (05/01/2024)
        Task DeleteAsync(List<Guid> ids);
    }
}
