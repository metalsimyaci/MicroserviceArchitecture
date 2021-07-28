using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ESourcing.Sourcing.Entities;
using ESourcing.Sourcing.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace ESourcing.Sourcing.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class AuctionController : ControllerBase
	{
		private readonly IAuctionRepository _auctionRepository;
		private readonly ILogger<AuctionController> _logger;

		public AuctionController(IAuctionRepository auctionRepository, ILogger<AuctionController> logger)
		{
			_auctionRepository = auctionRepository;
			_logger = logger;
		}

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
			var result=await _auctionRepository.Update(auction);
			return Ok(result);
		}

		[HttpDelete("{id:length(24)}")]
		[ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<bool>> DeleteAuction(string id)
		{
			var result = await _auctionRepository.Delete(id);
			return Ok(result);
		}
	}
}
