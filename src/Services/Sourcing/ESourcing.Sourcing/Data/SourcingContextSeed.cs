using System.Collections.Generic;
using Bogus;
using ESourcing.Sourcing.Entities;
using MongoDB.Driver;

namespace ESourcing.Sourcing.Data
{
	public static class SourcingContextSeed
	{
		public static void SeedData(IMongoCollection<Auction> auctionCollection)
		{
			var existProduct = auctionCollection.Find(p => true).Any();
			if (!existProduct)
			{
				auctionCollection.InsertManyAsync(GetConfigureProducts());
			}
		}

		private static IEnumerable<Auction> GetConfigureProducts()
		{
			var fakeEmails = new Faker<string>("tr").RuleFor(s => s, s => s.Internet.Email());
			var fakeAuctionGenerator = new Faker<Auction>("tr")
				.RuleFor(s => s.Name, s => $"Auction {s.UniqueIndex}")
				.RuleFor(s => s.Description, s => s.Commerce.ProductDescription())
				.RuleFor(s => s.ProductId, s => s.UniqueIndex.ToString())
				.RuleFor(s => s.CreatedAt, s => s.Date.Past())
				.RuleFor(s => s.FinishedAt, s => s.Date.Recent())
				.RuleFor(s => s.StartedAt, s => s.Date.Past(2))
				.RuleFor(s => s.Quantity, s => s.Random.Number(10000))
				.RuleFor(s => s.IncludedSellers, s => fakeEmails.Generate(s.Random.Number(0,5)))
				.RuleFor(s => s.Status, s => (int) s.PickRandom<Status>());

			return fakeAuctionGenerator.Generate(10);
		}
	}
}
