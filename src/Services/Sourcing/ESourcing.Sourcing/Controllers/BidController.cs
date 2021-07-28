using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ESourcing.Sourcing.Entities;
using ESourcing.Sourcing.Repositories.Interfaces;

namespace ESourcing.Sourcing.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class BidController : ControllerBase
	{
		private readonly IBidRepository _bidRepository;

		public BidController(IBidRepository bidRepository)
		{
			_bidRepository = bidRepository;
		}

		[HttpPost]
		[ProducesResponseType((int)HttpStatusCode.Created)]
		public async Task<ActionResult<Bid>> SendBid([FromBody] Bid bid)
		{
			await _bidRepository.SendBid(bid);
			return Ok();
		}

		[HttpGet("{id:length(24)}",Name="GetBidsByAuctionId")]
		[ProducesResponseType(typeof(IEnumerable<Bid>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<IEnumerable<Bid>>> GetBidsByAuctionId(string id)
		{
			var bids = await _bidRepository.GetBidsByAuctionId(id);
			return Ok(bids);
		}
		
		[HttpGet("{id:length(24)}",Name = "GetWinnerBid")]
		[ProducesResponseType(typeof(Bid), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<Bid>> GetWinnerBid(string id)
		{
			var bid = await _bidRepository.GetWinnerBid(id);
			return Ok(bid);
		}
	}
}
