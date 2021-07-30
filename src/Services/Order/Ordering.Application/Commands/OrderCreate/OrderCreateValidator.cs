using FluentValidation;

namespace Ordering.Application.Commands.OrderCreate
{
	public class OrderCreateValidator:AbstractValidator<OrderCreateCommand>
	{
		public OrderCreateValidator()
		{
			RuleFor(s => s.SellerUserName)
				.EmailAddress()
				.NotEmpty();

			RuleFor(s => s.ProductionId)
				.NotEmpty();
		}
	}
}