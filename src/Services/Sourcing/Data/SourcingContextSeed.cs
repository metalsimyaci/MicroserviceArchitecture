using System.Collections.Generic;
using Bogus;
using ESourcing.Sourcing.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ESourcing.Sourcing.Data
{
	public static class SourcingContextSeed
	{
		public static void SeedData(IMongoCollection<Auction> auctionCollection)
		{
			var existProduct = auctionCollection.Find(p => true).Any();
			
			if (existProduct) return;
			var auctions = GetConfigureProducts();
			auctionCollection.InsertManyAsync(auctions);
		}

		private static IEnumerable<Auction> GetConfigureProducts()
		{
			var fakeAuctionGenerator = new Faker<Auction>("tr")
				.RuleFor(s => s.Name, s => $"Auction {s.UniqueIndex}")
				.RuleFor(s => s.Description, s => s.Commerce.ProductDescription())
				.RuleFor(s => s.ProductId, s => ObjectId.GenerateNewId().ToString())
				.RuleFor(s => s.CreatedAt, s => s.Date.Past())
				.RuleFor(s => s.FinishedAt, s => s.Date.Recent())
				.RuleFor(s => s.StartedAt, s => s.Date.Past(2))
				.RuleFor(s => s.Quantity, s => s.Random.Number(10000))
				.RuleFor(s => s.IncludedSellers, s => s.Make(s.Random.Number(0,5),()=>s.Internet.Email()))
				.RuleFor(s => s.Status, s => (int)s.PickRandom<Status>());

			return fakeAuctionGenerator.Generate(10);
		}
	}
}
