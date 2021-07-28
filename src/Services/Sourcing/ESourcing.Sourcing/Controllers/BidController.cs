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
		[ProducesResponseType(typeof(Bid), (int)HttpStatusCode.Created)]
		public async Task<ActionResult<Bid>> SendBid([FromBody] Bid bid)
		{
			await _bidRepository.SendBid(bid);
			return CreatedAtRoute("GetAuction", new { id = bid.Id }, bid);
		}

		[HttpGet("{id:length(24)}")]
		[ProducesResponseType(typeof(IEnumerable<Bid>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<IEnumerable<Bid>>> GetBidsByAuctionId(string id)
		{
			var auctions = await _bidRepository.GetBidsByAuctionId(id);
			return Ok(auctions);
		}
	}
}
