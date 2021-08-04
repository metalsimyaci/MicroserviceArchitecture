using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ESourcing.Core.Repositories.Abstract;
using ESourcing.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ESourcing.Infrastructure.Repositories.Abstract
{
	public class RepositoryBase<T>:IRepository<T> where T:class,new()
	{
		private readonly WebAppContext _context;

		public RepositoryBase(WebAppContext context)
		{
			_context = context;
		}
		public async Task<IReadOnlyList<T>> GetAllAsync()
		{
			return await _context.Set<T>().ToListAsync();
		}

		public async Task<IReadOnlyList<T>> GetsAsync(Expression<Func<T, bool>> predicate)
		{
			return await _context.Set<T>().Where(predicate).ToListAsync();
		}

		public async Task<IReadOnlyList<T>> GetsAsync(Expression<Func<T, bool>> predicate = null, 
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
			string includeString = null, 
			bool disableTracking = true)
		{
			IQueryable<T> query = _context.Set<T>();

			if (disableTracking)
				query = query.AsNoTracking();

			if (predicate != null)
				query = query.Where(predicate);

			if (orderBy != null)
				query = orderBy(query);

			if (!string.IsNullOrWhiteSpace(includeString))
				query = query.Include(includeString);

			return await query.ToListAsync();
		}

		public async Task<IReadOnlyList<T>> GetsAsync(Expression<Func<T, bool>> predicate = null, 
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
			List<Expression<Func<T, object>>> includes = null, 
			bool disableTracking = true)
		{
			IQueryable<T> query = _context.Set<T>();

			if (disableTracking)
				query = query.AsNoTracking();

			if (predicate != null)
				query = query.Where(predicate);

			if (orderBy != null)
				query = orderBy(query);

			if (includes!=null)
				query = includes.Aggregate(query,(current, include)=>current.Include(include));

			return await query.ToListAsync();
		}

		public async Task<T> GetByIdAsync(int id)
		{
			return await _context.Set<T>().FindAsync(id);
		}

		public async Task<T> AddAsync(T entity)
		{
			_context.Set<T>().Add(entity);
			await _context.SaveChangesAsync();
			return entity;
		}

		public async Task UpdateAsync(T entity)
		{
			_context.Entry(entity).State = EntityState.Modified;
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(T entity)
		{
			_context.Set<T>().Remove(entity);
			await _context.SaveChangesAsync();
		}
	}
}
