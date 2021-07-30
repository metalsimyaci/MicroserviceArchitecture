using System.Collections.Generic;
using MediatR;
using Ordering.Application.Responses;

namespace Ordering.Application.Queries
{
	public class GetOrdersBySellerUserNameQuery:IRequest<IEnumerable<OrderResponse>>
	{
		public string UserName{ get; set; }

		public GetOrdersBySellerUserNameQuery(string userName)
		{
			UserName = userName;
		}
	}
}