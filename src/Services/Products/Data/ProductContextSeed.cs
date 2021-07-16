using System.Collections.Generic;
using ESourcing.Products.Entities;
using MongoDB.Driver;

namespace ESourcing.Products.Data
{
	public class ProductContextSeed
	{

		public static void SeedData(IMongoCollection<Product> productCollection)
		{
			bool existProduct = productCollection.Find(p => true).Any();
			if (!existProduct)
			{
				productCollection.InsertManyAsync(GetConfigureProducts());
			}
		}

		private static IEnumerable<Product> GetConfigureProducts()
		{
			return new List<Product>
			{
				new Product
				{
					Name = "Iphone X",
					Summary = "This isss",
					Description = "Description",
					ImageFie = "product-1.png",
					Price = 950.00M,
					Category = "Smart Phone"
				}
			};
		}
	}
}
