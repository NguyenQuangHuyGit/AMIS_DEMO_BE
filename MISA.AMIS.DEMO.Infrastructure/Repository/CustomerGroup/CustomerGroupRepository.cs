using MISA.AMIS.DEMO.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Infrastructure
{
    public class CustomerGroupRepository : BaseRepository<CustomerGroup>, ICustomerGroupRepository
    {
        public CustomerGroupRepository(IMISADbContext dbContext) : base(dbContext)
        {
        }
    }
}
