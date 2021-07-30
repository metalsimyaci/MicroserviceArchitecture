using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities.Abstract;
using Ordering.Domain.Repositories.Abstract;
using Ordering.Infrastructure.Data;

namespace Ordering.Infrastructure.Repositories.Abstract
{
	public class Repository<T> : IRepository<T> where T:EntityBase
	{
		protected readonly OrderContext _dbContext;

		public Repository(OrderContext context)
		{
			_dbContext = context;
		}
		
		public async Task<IEnumerable<T>> GetAllAsync()
		{
			return await _dbContext.Set<T>().ToListAsync();
		}

		public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate)
		{
			return await _dbContext.Set<T>().Where(predicate).ToListAsync();
		}

		public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate, 
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeString = null, bool disableTracking = true)
		{
			IQueryable<T> query = _dbContext.Set<T>();

			if (disableTracking)
				query = query.AsNoTracking();

			if (!string.IsNullOrWhiteSpace(includeString))
				query = query.Include(includeString);

			if (predicate!=null)
				query= query.Where(predicate);

			if (orderBy != null)
				query = orderBy(query);
		
			return await query.ToListAsync();
		}

		public async Task<T> GetByIdAsync(int id)
		{
			return await _dbContext.Set<T>().FindAsync(id);
		}

		public async Task<T> AddAsync(T entity)
		{
			_dbContext.Set<T>().Add(entity);
			await _dbContext.SaveChangesAsync();
			return entity;
		}

		public async Task UpdateAsync(T entity)
		{
			_dbContext.Entry(entity).State = EntityState.Modified;
			await _dbContext.SaveChangesAsync();
		}

		public async Task DeleteAsync(T entity)
		{
			_dbContext.Set<T>().Remove(entity);
			await _dbContext.SaveChangesAsync();
		}
	}
}
