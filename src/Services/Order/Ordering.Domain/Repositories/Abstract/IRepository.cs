using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ordering.Domain.Entities.Abstract;

namespace Ordering.Domain.Repositories.Abstract
{
	public interface IRepository<T> where T:IEntity
	{
		Task<IEnumerable<T>> GetAllAsync();
		Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate);

		Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate, 
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			string includeString = null, 
			bool disableTracking = true);

		Task<T> GetByIdAsync(int id);
		Task<T> AddAsync(T entity);
		Task UpdateAsync(T entity);
		Task DeleteAsync(T entity);
	}
}