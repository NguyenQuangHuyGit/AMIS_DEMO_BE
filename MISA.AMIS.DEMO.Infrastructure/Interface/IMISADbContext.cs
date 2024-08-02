using MISA.AMIS.DEMO.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Infrastructure
{
    public interface IMISADbContext
    {
        #region Properties
        DbConnection Connection { get; }
        DbTransaction? Transaction { get; set; } 
        #endregion

        #region Methods
        /// <summary>
        /// Hàm thao tác với DB lấy tất cả danh sách 1 đối tượng
        /// </summary>
        /// <typeparam name="TEntity">Entity tương ứng</typeparam>
        /// <returns>Danh sách 1 đối tượng</returns>
        /// Author: QuangHuy (04/01/2024)
        public Task<IEnumerable<TEntity>> GetAsync<TEntity>();

        /// <summary>
        /// Hàm thao tác với DB lấy ra 1 đối tượng theo Id
        /// </summary>
        /// <typeparam name="TEntity">Entity tương ứng</typeparam>
        /// <param name="id">Id đối tượng</param>
        /// <returns>Thông tin 1 đối tượng</returns>
        /// Author: QuangHuy (04/01/2024)
        public Task<TEntity?> GetAsync<TEntity>(Guid id);

        /// <summary>
        /// Hàm thao tác với DB lấy danh sách đối tượng theo danh sách ID
        /// </summary>
        /// <typeparam name="TEntity">Kiểu đối tượng</typeparam>
        /// <param name="ids">Danh sách Id</param>
        /// <returns>Danh sách đối tượng</returns>
        /// Author: QuangHuy (25/01/2024)
        public Task<IEnumerable<TEntity>?> GetAsync<TEntity>(List<Guid> ids);

        /// <summary>
        /// Hàm thao tác với DB lấy ra 1 đối tượng theo 1 trường bất kỳ
        /// </summary>
        /// <typeparam name="TEntity">Entity tương ứng</typeparam>
        /// <param name="fields">Tên trường</param>
        /// <param name="value">Giá trị trường đó</param>
        /// <returns>Đối tượng tìm được tương ứng || NULL</returns>
        /// Author: QuangHuy (04/01/2024)
        public Task<TEntity?> GetAsync<TEntity>(string fields, object value);

        /// <summary>
        /// Hàm thao tác với DB thêm mới 1 đối tượng
        /// </summary>
        /// <typeparam name="TEntity">Entity tương ứng</typeparam>
        /// <param name="entity">Entity tương ứng với đối tượng</param>
        /// Author: QuangHuy (04/01/2024)
        public Task InsertAsync<TEntity>(TEntity entity);

        /// <summary>
        /// Hàm thao tác DB thêm mới nhiều bản ghi cùng 1 lúc
        /// </summary>
        /// <typeparam name="TEntity">Kiểu đối tượng</typeparam>
        /// <param name="entities">Danh sách đối tượng</param>
        /// <returns></returns>
        /// Author: QuangHuy (04/01/2024)
        public Task<object?> InsertAsync<TEntity>(List<TEntity> entities);

        /// <summary>
        /// Hàm thao tác với DB cập nhật thông tin 1 đối tượng
        /// </summary>
        /// <typeparam name="TEntity">Entity tương ứng</typeparam>
        /// <param name="id">Id đối tượng</param>
        /// <param name="entity">Entity tương ứng với đối tượng</param>
        /// Author: QuangHuy (04/01/2024)
        public Task UpdateAsync<TEntity>(Guid id, TEntity entity);

        /// <summary>
        /// Hàm thao tác với DB xóa thông tin 1 đối tượng theo id
        /// </summary>
        /// <typeparam name="TEntity">Entity tương ứng</typeparam>
        /// <param name="id">Id đối tượng</param>
        /// Author: QuangHuy (04/01/2024)
        public Task DeleteAsync<TEntity>(Guid id);

        /// <summary>
        /// Hàm thao tác với DB xóa thông tin nhiều đối tượng theo id
        /// </summary>
        /// <typeparam name="TEntity">Entity tương ứng</typeparam>
        /// <param name="id">Danh sách Id các đối tượng</param>
        /// Author: QuangHuy (04/01/2024)
        public Task DeleteAsync<TEntity>(List<Guid> ids); 
        #endregion
    }
}
