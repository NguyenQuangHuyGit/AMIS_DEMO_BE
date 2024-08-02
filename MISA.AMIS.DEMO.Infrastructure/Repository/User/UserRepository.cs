using Dapper;
using MISA.AMIS.DEMO.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Infrastructure
{
    /// <summary>
    /// User Repo thao tác với DB
    /// </summary>
    public class UserRepository : BaseRepository<UserModel>, IUserRepository
    {
        public UserRepository(IMISADbContext dbContext) : base(dbContext)
        {
        }

        public async Task<UserModel?> GetUserModel(string account)
        {
            string storedProcedureName = $"Proc_GetUser";
            var param = new DynamicParameters();
            param.Add("username", account);
            var result = await _dbContext.Connection.QueryAsync<UserModel, Role, UserModel>(storedProcedureName, (userModel, role) =>
            {
                userModel.Role = role;
                return userModel;
            },
            param,
            splitOn: "RoleId");
            return result.FirstOrDefault();
        }

        public async Task UpdateUserRefreshToken(User user)
        {
            await _dbContext.UpdateAsync<User>(user.UserId, user);
        }
    }
}
