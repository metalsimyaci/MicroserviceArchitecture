using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ordering.Application.PipelineBehaviours
{
	public class UnHandledExceptionBehaviour<TRequest, TResponse>:IPipelineBehavior<TRequest, TResponse>
	{
		private readonly ILogger<TRequest> _logger;

		public UnHandledExceptionBehaviour(ILogger<TRequest> logger)
		{
			_logger = logger;
		}

		public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
		{
			try
			{
				return await next();
			}
			catch (Exception e)
			{
				var requestName = typeof(TRequest).Name;
				_logger.LogError(e,"Request: Unhandled Exception for Request {Name}{@Request}",requestName,request);

				throw;
			}
		}
	}
}