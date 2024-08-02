using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.AMIS.DEMO.Core;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace MISA.AMIS.DEMO.Infrastructure
{
    /// <summary>
    /// Lớp thao tác với MariaDB
    /// </summary>
    public class MariaDbContext : IMISADbContext
    {
        #region Fields
        private readonly DbConnection _connection;
        private DbTransaction? _transaction = null;
        #endregion

        #region Contructor
        public MariaDbContext(IConfiguration configuration)
        {
            _connection = new MySqlConnection(configuration.GetConnectionString("MariaConnection"));
        }
        #endregion

        #region Properties
        // Connection
        public DbConnection Connection => _connection;

        // Transaction
        public DbTransaction? Transaction { get => _transaction; set => _transaction = value; }
        #endregion

        #region Methods
        public async Task<IEnumerable<TEntity>> GetAsync<TEntity>()
        {
            var sql = $"SELECT * FROM {GetTableName<TEntity>()}";
            var result = await Connection.QueryAsync<TEntity>(sql);
            return result;
        }

        public async Task<TEntity?> GetAsync<TEntity>(Guid id)
        {
            var sql = $"SELECT * FROM {GetTableName<TEntity>()} where {GetPrimaryKey<TEntity>()} = @id";
            var param = new DynamicParameters();
            param.Add("id", id);
            var result = await Connection.QueryFirstOrDefaultAsync<TEntity>(sql, param);
            return result;
        }

        public async Task<IEnumerable<TEntity>?> GetAsync<TEntity>(List<Guid> ids)
        {
            var sql = new StringBuilder($"SELECT * FROM {GetTableName<TEntity>()} where {GetPrimaryKey<TEntity>()} IN(");
            var param = new DynamicParameters();
            int i = 1;
            foreach(var id in ids)
            {
                var paramName = $"@id{i}";
                param.Add(paramName, id);
                sql.Append($"{paramName},");
                i++;
            }
            sql.Remove(sql.Length - 1, 1);
            sql.Append(')');
            var result = await Connection.QueryAsync<TEntity>(sql.ToString(),param);
            return result;
        }

        public async Task<TEntity?> GetAsync<TEntity>(string fields, object value)
        {
            var sql = $"SELECT * FROM {GetTableName<TEntity>()} where {fields} = @value";
            var param = new DynamicParameters();
            param.Add("value", value);
            var result = await Connection.QueryFirstOrDefaultAsync<TEntity>(sql, param);
            return result;
        }

        public async Task InsertAsync<TEntity>(TEntity entity)
        {
            var propertyNames = GetListProperties(entity);
            string sql = $"INSERT INTO {GetTableName<TEntity>()} ({String.Join(',', propertyNames.ToArray())}) VALUES (";
            var param = new DynamicParameters();
            foreach (var property in propertyNames)
            {
                string paramString = $"@{property}";
                sql += $"{paramString},";
                var value = typeof(TEntity).GetProperty(property)?.GetValue(entity);
                param.Add(property, value);
            }
            sql = sql.TrimEnd(',');
            sql += ")";
            await Connection.ExecuteAsync(sql, param);
        }

        public async Task<object?> InsertAsync<TEntity>(List<TEntity> entities)
        {
            var propertyNames = GetListProperties(entities[0]);
            var param = new DynamicParameters();
            var sql = new StringBuilder($"INSERT INTO {GetTableName<TEntity>()} ({String.Join(',', propertyNames.ToArray())}) VALUES ");
            int i = 1;
            foreach(var entity in entities)
            {
                sql.Append('(');
                foreach (var property in propertyNames)
                {
                    string paramString = $"@{property}{i}";
                    sql.Append($"{paramString},");
                    var value = typeof(TEntity).GetProperty(property)?.GetValue(entity);
                    param.Add(paramString, value);
                }
                sql.Remove(sql.Length - 1, 1);
                sql.Append("),");
                i++;
            }
            sql.Remove(sql.Length - 1, 1);
            sql.Append(';');
            var result = await Connection.ExecuteAsync(sql.ToString(), param, Transaction);
            return result;
        }

        public async Task UpdateAsync<TEntity>(Guid id, TEntity entity)
        {
            var propertyNames = GetListProperties(entity);
            string sql = $"UPDATE {GetTableName<TEntity>()} SET ";
            var param = new DynamicParameters();
            foreach (var property in propertyNames)
            {
                if(property == GetPrimaryKey<TEntity>() || property == "CreatedDate" || property == "CreatedBy")
                {
                    continue;
                }
                string paramString = $"@{property}";
                sql += $"{property} = {paramString},";
                var value = typeof(TEntity).GetProperty(property)?.GetValue(entity);
                param.Add(property, value);
            }
            sql = sql.TrimEnd(',');
            sql += $" WHERE {GetPrimaryKey<TEntity>()} = @id";
            param.Add("id", id);
            await Connection.ExecuteAsync(sql, param);
        }

        public async Task DeleteAsync<TEntity>(Guid id)
        {
            var sql = $"DELETE FROM {GetTableName<TEntity>()} WHERE {GetPrimaryKey<TEntity>()} = @id";
            var param = new DynamicParameters();
            param.Add("id", id);
            await Connection.ExecuteAsync(sql, param);
        }

        public async Task DeleteAsync<TEntity>(List<Guid> ids)
        {
            string sql = $"DELETE FROM {GetTableName<TEntity>()} WHERE {GetPrimaryKey<TEntity>()} IN(";
            int index = 0;
            var param = new DynamicParameters();
            foreach(var id in ids)
            {
                var paramName = $"@id{index++}";
                sql += paramName;
                sql += ",";
                param.Add(paramName, id);
            }
            sql = sql.TrimEnd(',');
            sql += ");";
            await Connection.ExecuteAsync(sql, param);
        }

        /// <summary>
        /// Hàm lấy tên TABLE trong DB tương ứng với Entity
        /// </summary>
        /// <typeparam name="TEntity">Entity tương ứng</typeparam>
        /// <typeparam name="TENtity">Entity tương ứng</typeparam>
        /// <returns>Tên TABLE</returns>
        /// CreatedBy: QuangHuy (05/01/2024)
        private static string GetTableName<TENtity>()
        {
            return typeof(TENtity).Name;
        }

        /// <summary>
        /// Hàm lấy tên PRIMAREY ơng ứng với Entity
        /// </summary>
        /// <typeparam name="TENtity">Entity tương ứng</typeparam>
        /// <returns>Tên TABLE</returns>
        ////CreatedBy: QuangHuy (05/01/2024)
        private static string GetPrimaryKey<TENtity>()
        {
            return $"{typeof(TENtity).Name}Id";
        }

        /// <summary>
        /// Hàm lấy danh sách các Property của 1 Entity
        /// </summary>
        /// <typeparam name="TEntity">Entity tương ứng</typeparam>
        /// <param name="entity">Entity tương ứng</param>
        /// <returns>1 List các Property</returns>
        /// Author: QuangHuy (05/01/2023)
        private static List<string> GetListProperties<TEntity>(TEntity entity)
        {
            List<string> propertyNames = new();
            if (entity != null)
            {
                var properties = entity.GetType().GetProperties();
                foreach (var property in properties)
                {
                    propertyNames.Add(property.Name.ToString());
                }
            }
            return propertyNames;
        }
        #endregion
    }
}
