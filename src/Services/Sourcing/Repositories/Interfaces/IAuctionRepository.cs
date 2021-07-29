using System.Collections.Generic;
using System.Threading.Tasks;
using ESourcing.Sourcing.Entities;

namespace ESourcing.Sourcing.Repositories.Interfaces
{
	public interface IAuctionRepository
	{
		public Task<IEnumerable<Auction>> GetAuctions();
		public Task<Auction> GetAuction(string id);
		public Task<Auction> GetAuctionByName(string name);
		public Task Create(Auction auction);
		public Task<bool> Update(Auction auction);
		public Task<bool> Delete(string id);
	}
}