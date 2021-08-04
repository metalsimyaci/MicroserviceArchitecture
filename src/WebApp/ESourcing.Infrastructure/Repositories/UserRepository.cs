using ESourcing.Core.Entities;
using ESourcing.Infrastructure.Data;
using ESourcing.Infrastructure.Repositories.Abstract;

namespace ESourcing.Infrastructure.Repositories
{
	public class UserRepository:RepositoryBase<AppUser>, IUserRepository
	{
		public UserRepository(WebAppContext context) : base(context)
		{
		}
	}
}