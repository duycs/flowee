using System;
using Refit;

namespace AppShareApplication.Services
{
	public interface IOperationClientService
	{
        [Get("/operations/performed")]
        public Task<List<string>> PerformedOperations(Guid[] ids);
	}
}

