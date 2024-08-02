using MISA.AMIS.DEMO.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Infrastructure
{
    public class PositionRepository : BaseRepository<Position>, IPositionRepository
    {
        public PositionRepository(IMISADbContext dbContext) : base(dbContext)
        {
        }
    }
}
