using System;
using AppShareDomain.DTOs.Log;

namespace AppShareApplication.Services
{
	public interface ILogClientService
	{
		public Task Add(LogDto logDto);
		public Task Update(int id, LogDto Dto);
		public Task Remove(int id);
	}
}

