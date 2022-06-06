using AppShareServices.DataAccess.Persistences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerInfrastructure.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WorkerContext _context;
        private bool _disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public UnitOfWork(WorkerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing) _context?.Dispose();

            _disposed = true;
        }

        /// <summary>
        /// Commits this instance.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public int Commit()
        {
            if (_disposed) throw new ObjectDisposedException(GetType().FullName);
            return _context.SaveChanges();
        }

        /// <summary>
        /// Rollbacks this instance.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Rollback()
        {
            Dispose();
        }
    }
}
