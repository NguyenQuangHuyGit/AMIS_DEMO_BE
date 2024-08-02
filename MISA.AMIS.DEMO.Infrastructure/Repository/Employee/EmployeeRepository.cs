using Dapper;
using Microsoft.AspNetCore.Http;
using MISA.AMIS.DEMO.Core;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace MISA.AMIS.DEMO.Infrastructure
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="dbContext">DbContect tương tác DB</param>
        /// CreatedBy: QuangHuy(03/01/2024)
        public EmployeeRepository(IMISADbContext dbContext) : base(dbContext)
        {
        }

        #region Methods
        public async Task<IEnumerable<EmployeeModel>> GetAllEmployeeModelAsync()
        {
            var sql = "SELECT * FROM View_EmployeeModel";
            var result = await _dbContext.Connection.QueryAsync<EmployeeModel, Position, Department, EmployeeModel>(sql, (employee, position, department) =>
            {
                employee.Position = position;
                employee.Department = department;
                return employee;
            },
            splitOn: "PositionId, DepartmentId");
            return result;
        }

        public async Task<string?> GetLastestEmployeeCode()
        {
            var sql = "SELECT e.EmployeeCode FROM Employee e ORDER BY e.EmployeeCode DESC";
            var result = await _dbContext.Connection.QueryFirstOrDefaultAsync<string>(sql);
            return result;
        }

        public async Task<EmployeeFillterModel> GetFillterEmployeeAsync(int pageSize, int pageNumber, string? fillterString)
        {
            string storedProcedureName = $"Proc_FillterEmployeePagingTotal";
            var offset = (pageNumber - 1) * pageSize;
            var param = new DynamicParameters();
            param.Add("fillterString", fillterString);
            param.Add("pageSize", pageSize);
            param.Add("offset", offset);
            param.Add("totalRecord", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var result = await _dbContext.Connection.QueryAsync<EmployeeModel, Position, Department, EmployeeModel>(storedProcedureName, (employee, position, department) =>
            {
                employee.Position = position;
                employee.Department = department;
                return employee;
            },
            param,
            splitOn: "PositionId, DepartmentId");
            int totalRecord = param.Get<int>("totalRecord");
            var actualResult = new EmployeeFillterModel();
            actualResult.Employees = result;
            actualResult.TotalRecord = totalRecord;
            return actualResult;
        }

        public async Task<IEnumerable<EmployeeModel>> GetEmployeeModelAsync(List<Guid> ids)
        {
            var sql = new StringBuilder($"SELECT * FROM View_EmployeeModel e WHERE e.EmployeeId IN(");
            var param = new DynamicParameters();
            int i = 1;
            foreach (var id in ids)
            {
                var paramName = $"@id{i}";
                param.Add(paramName, id);
                sql.Append($"{paramName},");
                i++;
            }
            sql.Remove(sql.Length - 1, 1);
            sql.Append(')');

            var result = await _dbContext.Connection.QueryAsync<EmployeeModel, Position, Department, EmployeeModel>(sql.ToString(), (employee, position, department) =>
            {
                employee.Position = position;
                employee.Department = department;
                return employee;
            },
            param,
            splitOn: "PositionId, DepartmentId");
            return result;
        }
        #endregion
    }
}
