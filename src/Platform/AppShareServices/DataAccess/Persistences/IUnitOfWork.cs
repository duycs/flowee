using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShareServices.DataAccess.Persistences
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Commits this instance.
        /// </summary>
        int Commit();

        /// <summary>
        /// Rollbacks this instance.
        /// </summary>
        void Rollback();
    }
}
