using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ESourcing.Products.Data.Interfaces;
using ESourcing.Products.Entities;
using MongoDB.Driver;

namespace ESourcing.Products.Repositories
{
	public class ProductRepository : IProductRepository
	{
		private readonly IProductContext _context;

		public ProductRepository(IProductContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Product>> GetProducts()
		{
			return await _context.Products.Find(p => true).ToListAsync();
		}

		public async Task<Product> GetProduct(string id)
		{
			return await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<Product>> GetProductByName(string name)
		{
			var filter = Builders<Product>.Filter.ElemMatch(m => m.Name, name);
			return await _context.Products.Find(filter).ToListAsync();
		}

		public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
		{
			var filter = Builders<Product>.Filter.Eq(m => m.Category, categoryName);
			return await _context.Products.Find(filter).ToListAsync();
		}

		public async Task Create(Product product)
		{
			await _context.Products.InsertOneAsync(product);
		}

		public async Task<bool> Update(Product product)
		{
			var filter = Builders<Product>.Filter.Eq(m => m.Id, product.Id);

			var updateResult = await _context.Products.ReplaceOneAsync(filter, product);
			return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
		}

		public async Task<bool> Delete(string id)
		{
			var filter = Builders<Product>.Filter.Eq(m => m.Id, id);
			var deleteResult = await _context.Products.DeleteOneAsync(filter);
			return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
		}
	}
}
