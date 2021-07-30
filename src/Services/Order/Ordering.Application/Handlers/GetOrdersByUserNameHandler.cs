using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using Ordering.Domain.Repositories.Abstract;

namespace Ordering.Application.Handlers
{
	public class GetOrdersByUserNameHandler:IRequestHandler<GetOrdersBySellerUserNameQuery,IEnumerable<OrderResponse>>
	{
		private readonly IOrderRepository _orderRepository;
		private readonly IMapper _mapper;

		public GetOrdersByUserNameHandler(IOrderRepository orderRepository, IMapper mapper)
		{
			_orderRepository = orderRepository;
			_mapper = mapper;
		}
		public async Task<IEnumerable<OrderResponse>> Handle(GetOrdersBySellerUserNameQuery request, CancellationToken cancellationToken)
		{
			var orders = await _orderRepository.GetOrdersBySellerUserName(request.UserName);
			var orderResponses = _mapper.Map<IEnumerable<OrderResponse>>(orders);
			return orderResponses;
		}
	}
}