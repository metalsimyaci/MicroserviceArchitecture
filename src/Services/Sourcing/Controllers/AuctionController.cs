using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using ESourcing.Sourcing.Entities;
using ESourcing.Sourcing.Repositories.Interfaces;
using EventBusRabbitMq.Core;
using EventBusRabbitMq.Events;
using EventBusRabbitMq.Producer;
using Microsoft.Extensions.Logging;

namespace ESourcing.Sourcing.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class AuctionController : ControllerBase
	{
		#region Variables

		private readonly IAuctionRepository _auctionRepository;
		private readonly ILogger<AuctionController> _logger;
		private readonly IBidRepository _bidRepository;
		private readonly IMapper _mapper;
		private readonly EventBusRabbitMqProducer _eventBus;

		#endregion

		#region Constructor

		public AuctionController(IAuctionRepository auctionRepository, ILogger<AuctionController> logger, IBidRepository bidRepository,
			IMapper mapper, EventBusRabbitMqProducer eventBus)
		{
			_auctionRepository = auctionRepository;
			_logger = logger;
			_bidRepository = bidRepository;
			_mapper = mapper;
			_eventBus = eventBus;
		}

		#endregion

		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<Auction>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<IEnumerable<Auction>>> GetAuctions()
		{
			var auctions = await _auctionRepository.GetAuctions();
			return Ok(auctions);
		}

		[HttpGet("{id:length(24)}", Name = "GetAuction")]
		[ProducesResponseType(typeof(Auction), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public async Task<ActionResult<Auction>> GetAuction(string id)
		{
			var auction = await _auctionRepository.GetAuction(id);

			if (auction == null)
			{
				_logger.LogError($"Auction with id:{id}, hasn't found in database");
				return NotFound();
			}
			return Ok(auction);
		}

		[HttpPost]
		[ProducesResponseType(typeof(Auction), (int)HttpStatusCode.Created)]
		public async Task<ActionResult<Auction>> CreateAuction([FromBody] Auction auction)
		{
			await _auctionRepository.Create(auction);
			return CreatedAtRoute("GetAuction", new { id = auction.Id }, auction);
		}

		[HttpPut]
		[ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<bool>> UpdateAuction([FromBody] Auction auction)
		{
			var result = await _auctionRepository.Update(auction);
			return Ok(result);
		}

		[HttpDelete("{id:length(24)}")]
		[ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<bool>> DeleteAuction(string id)
		{
			var result = await _auctionRepository.Delete(id);
			return Ok(result);
		}

		[HttpPost("CompleteAuction/{id:length(24)}", Name = "CompleteAuction")]
		[ProducesResponseType((int)HttpStatusCode.Accepted)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		public async Task<ActionResult> CompleteAuction(string id)
		{
			var auction = await _auctionRepository.GetAuction(id);

			if (auction == null)
			{
				_logger.LogError($"Auction with id:{id}, hasn't found in database");
				return NotFound();
			}

			if (auction.Status != (int)Status.Active)
			{
				_logger.LogError($"Auction can not be completed");
				return BadRequest();
			}

			var bid = await _bidRepository.GetBidsByAuctionId(id);
			if (bid == null)
				return NotFound();

			var orderCreateEvent = _mapper.Map<OrderCreateEvent>(bid);
			orderCreateEvent.Quantity = auction.Quantity;

			auction.Status = (int)Status.Closed;
			var updateResponse = await _auctionRepository.Update(auction);
			if (!updateResponse)
			{
				_logger.LogError("Auction can not updated");
				return BadRequest();
			}

			try
			{
				_eventBus.Publish(EventBusConstants.ORDER_CREATE_QUEUE, orderCreateEvent);
			}
			catch (Exception e)
			{
				_logger.LogError(e, " ERROR Publishing integration event: {EventId} from {AppName}", orderCreateEvent.RequestId, "Sourcing");
				throw;
			}

			return Accepted();
		}

		[HttpPost("TestEvent")]
		[ProducesResponseType(typeof(OrderCreateEvent), (int)HttpStatusCode.Accepted)]
		public ActionResult<OrderCreateEvent> TestEvent()
		{
			var eventMessage = new OrderCreateEvent
			{
				AuctionId = "dummy1",
				ProductId = "dummy_product_1",
				Price = 20,
				Quantity = 19,
				SellerUserName = "test@test.com"
			};

			try
			{
				_eventBus.Publish(EventBusConstants.ORDER_CREATE_QUEUE, eventMessage);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "ERROR Publishing integration event: {EventId} from {AppName}", eventMessage.Id, "Sourcing");
				throw;
			}
			return Accepted(eventMessage);
		}
	}
}
