using System;
using AppShareDomain.DTOs.Specification;
using Refit;

namespace AppShareApplication.Services
{
	public interface IOperationClientService
	{
		/// <summary>
		/// Fire event to ready performed operations by humance or auto
		/// </summary>
		/// <param name="ids"></param>
		/// <returns>true is succes otherwise</returns>
        [Get("/operations/performed")]
        public Task<bool> FireOperations(Guid[] ids);

		public Task<OperationDto> Find(int id);
	}
}

