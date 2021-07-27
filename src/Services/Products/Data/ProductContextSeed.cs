using System.Collections.Generic;
using System.Linq;
using Bogus;
using ESourcing.Products.Entities;
using MongoDB.Driver;

namespace ESourcing.Products.Data
{
	public static class ProductContextSeed
	{

		public static void SeedData(IMongoCollection<Product> productCollection)
		{
			var existProduct = productCollection.Find(p => true).Any();
			if (!existProduct)
			{
				productCollection.InsertManyAsync(GetConfigureProducts());
			}
		}

		private static IEnumerable<Product> GetConfigureProducts()
		{
			var fakeProductGenerator = new Faker<Product>("tr")
				.RuleFor(s => s.Name, s => s.Commerce.ProductName())
				.RuleFor(s => s.Description, s => s.Commerce.ProductDescription())
				.RuleFor(s => s.Category, s => s.Commerce.Categories(1).FirstOrDefault())
				.RuleFor(s => s.ImageFie, s => s.Internet.Avatar())
				.RuleFor(s => s.Price, s => decimal.Parse(s.Commerce.Price()))
				.RuleFor(s => s.Summary, s => s.Commerce.Product());
			return fakeProductGenerator.Generate(20);
		}
	}
}
