using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LRMSA.AppData.DALs
{
    public abstract class DALBase
    {
        protected readonly DbContext DbContext;

        protected DALBase(DbContext dbContext)
        {
            DbContext = dbContext;
        }
    }

    public class ChampionDAL : DALBase
    {
        public ChampionDAL(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
