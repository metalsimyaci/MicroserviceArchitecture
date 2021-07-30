using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands.OrderCreate;
using Ordering.Application.Queries;
using Ordering.Application.Responses;

namespace ESourcing.Order.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		private readonly ILogger<OrderController> _logger;
		private readonly IMediator _mediator;

		public OrderController(ILogger<OrderController> logger, IMediator mediator)
		{
			_logger = logger;
			this._mediator = mediator;
		}

		[HttpGet("GetOrdersByUserName/{userName}")]
		[ProducesResponseType(typeof(IEnumerable<OrderResponse>),(int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersByUserName(string userName)
		{
			var query = new GetOrdersBySellerUserNameQuery(userName);
			var orders = await _mediator.Send(query);
			if (orders.Count() == decimal.Zero)
			{
				_logger.LogWarning("Order not found");
				return NotFound();
			}
			return Ok(orders);
		}

		[HttpPost]
		[ProducesResponseType(typeof(OrderResponse), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		public async Task<ActionResult<OrderResponse>> OrderCreate([FromBody] OrderCreateCommand command)
		{
			var order = await _mediator.Send(command);
			if (order==null)
			{
				_logger.LogWarning("Order not Created ");
				return BadRequest();
			}
			return Ok(order);
		}
	}
}
