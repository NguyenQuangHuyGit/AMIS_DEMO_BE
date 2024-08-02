using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace MISA.AMIS.DEMO.Core
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        #region Fields
        #endregion

        #region Properties
        public IBaseRepository<Employee> EmployeeRepository { get; } 
        #endregion

        #region Methods
        /// <summary>
        /// Hàm khởi tạo 1 transaction
        /// </summary>
        /// <returns></returns>
        /// CreatedBy: QuangHuy(16/01/2024)
        public Task BeginTransactionAsync();

        /// <summary>
        /// Hàm Commit 1 transaction
        /// </summary>
        /// <returns></returns>
        /// CreatedBy: QuangHuy(16/01/2024)
        public Task CommitAsync();

        /// <summary>
        /// Hàm rollback 1 transaction
        /// </summary>
        /// <returns></returns>
        /// CreatedBy: QuangHuy(16/01/2024)
        public Task RollbackAsync(); 
        #endregion
    }
}
