using System;
using Refit;

namespace AppShareApplication.Services
{
	public interface IOperationClientService
	{
		/// <summary>
		/// Fire event performed operations
		/// </summary>
		/// <param name="ids"></param>
		/// <returns>true is succes otherwise</returns>
        [Get("/operations/performed")]
        public Task<bool> PerformedOperations(Guid[] ids);
	}
}

