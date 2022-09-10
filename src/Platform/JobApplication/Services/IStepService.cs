using System;
namespace JobApplication.Services
{
	public interface IStepService
	{
		public Task Preprocess(int stepId);
		public Task Processing(int stepId, Guid operationId, bool statusPerformed, string? outputPerformed);
		public Task PostProcessing(int stepId);
	}
}

