using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ordering.Application.PipelineBehaviours
{
	public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	{
		private readonly Stopwatch _timer;
		private readonly ILogger<TRequest> _logger;

		public PerformanceBehaviour(ILogger<TRequest> logger)
		{
			_logger = logger;
			_timer = new Stopwatch();
		}


		public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
		{
			_timer.Start();
			var response = await next();
			_timer.Stop();

			var elapsedMilliseconds = _timer.ElapsedMilliseconds;
			
			if (elapsedMilliseconds <= 500) return response;

			var requestName = typeof(TRequest).Name;
			_logger.LogWarning("Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@Request}",requestName,elapsedMilliseconds,request);

			return response;
		}
	}
}