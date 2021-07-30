using System;
using Ordering.Domain.Entities.Abstract;

namespace Ordering.Domain.Entities
{
    public class Order:EntityBase
    {
	    public string AuctionId { get; set; }
	    public string SellerUserName { get; set; }
	    public string ProductionId { get; set; }
	    public decimal UnitPrice { get; set; }
	    public decimal TotalPrice { get; set; }
	    public DateTime CreatedAt { get; set; }
    }
}
