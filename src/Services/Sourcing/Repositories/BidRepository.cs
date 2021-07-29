using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESourcing.Sourcing.Data.Interface;
using ESourcing.Sourcing.Entities;
using ESourcing.Sourcing.Repositories.Interfaces;
using MongoDB.Driver;

namespace ESourcing.Sourcing.Repositories
{
	public class BidRepository:IBidRepository
	{
		private readonly ISourcingContext _context;

		public BidRepository(ISourcingContext context)
		{
			_context = context;
		}
		public async Task SendBid(Bid bid)
		{
			await _context.Bids.InsertOneAsync(bid);
		}

		public async Task<List<Bid>> GetBidsByAuctionId(string id)
		{
			var filter = Builders<Bid>.Filter.Eq(m => m.AuctionId, id);
			var bids= await _context.Bids.Find(filter).ToListAsync();
			bids = bids.OrderByDescending(a => a.CreatedAt)
				.GroupBy(a => a.SellerUserName)
				.Select(a => new Bid
				{
					AuctionId = a.FirstOrDefault()?.AuctionId,
					Price=a.FirstOrDefault()?.Price??0,
					Id = a.FirstOrDefault()?.Id,
					CreatedAt = a.FirstOrDefault()?.CreatedAt??default(DateTime),
					ProductId= a.FirstOrDefault()?.ProductId,
					SellerUserName = a.Key
				}).ToList();
			return bids;
		}

		public async Task<Bid> GetWinnerBid(string id)
		{
			var bids = await GetBidsByAuctionId(id);
			return bids.OrderByDescending(s => s.Price).FirstOrDefault();
		}
	}
}
