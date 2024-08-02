using Microsoft.Extensions.Configuration;
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
    public class UnitOfWork : IUnitOfWork
    {
        #region Fields
        private readonly IMISADbContext _context;
        private readonly IBaseRepository<Employee> _employeeRepository;
        #endregion

        #region Contructor
        public UnitOfWork(IMISADbContext context, IBaseRepository<Employee> employeeRepository)
        {
            _context = context;
            _employeeRepository = employeeRepository;
        }
        #endregion

        #region Properties
        public IBaseRepository<Employee> EmployeeRepository => _employeeRepository; 
        #endregion

        #region Methods
        public async Task BeginTransactionAsync()
        {
            if (_context.Connection.State != ConnectionState.Open)
            {
                await _context.Connection.OpenAsync();
            }
            _context.Transaction = await _context.Connection.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (_context.Transaction != null)
            {
                await _context.Transaction.CommitAsync();
            }
            await DisposeAsync();
        }

        public async Task RollbackAsync()
        {
            if (_context.Transaction != null)
            {
                await _context.Transaction.RollbackAsync();
            }
            await DisposeAsync();
        }

        public async ValueTask DisposeAsync()
        {
            if (_context.Transaction != null)
            {
                await _context.Transaction.DisposeAsync();
            }
            _context.Transaction = null;
            await _context.Connection.CloseAsync();
        } 
        #endregion
    }
}
