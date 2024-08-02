using Dapper;
using MISA.AMIS.DEMO.Core;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace MISA.AMIS.DEMO.Infrastructure
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        #region Fields
        protected readonly IMISADbContext _dbContext; 
        #endregion

        #region Contructor
        public BaseRepository(IMISADbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Properties
        #endregion

        #region Methods
        public virtual async Task<IEnumerable<TEntity>> GetAsync()
        {
            var result = await _dbContext.GetAsync<TEntity>();
            return result;
        }

        public virtual async Task<TEntity?> GetAsync(Guid id)
        {
            var result = await _dbContext.GetAsync<TEntity>(id);
            return result;
        }

        public virtual async Task<IEnumerable<TEntity>?> GetAsync(List<Guid> ids)
        {
            var result = await _dbContext.GetAsync<TEntity>(ids);
            return result;
        }

        public virtual async Task<TEntity?> FindByFieldAsync(string field, string value)
        {
            var result = await _dbContext.GetAsync<TEntity>(field, value);
            return result;
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            await _dbContext.InsertAsync<TEntity>(entity);
        }

        public virtual async Task InsertAsync(List<TEntity> entities)
        {
            await _dbContext.InsertAsync<TEntity>(entities);
        }

        public virtual async Task UpdateAsync(Guid id, TEntity entity)
        {
            await _dbContext.UpdateAsync<TEntity>(id, entity);
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            await _dbContext.DeleteAsync<TEntity>(id);
        }

        public virtual async Task DeleteAsync(List<Guid> ids)
        {
            await _dbContext.DeleteAsync<TEntity>(ids);
        }


        #endregion
    }
}
