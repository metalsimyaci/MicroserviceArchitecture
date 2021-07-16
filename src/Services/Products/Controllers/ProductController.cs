using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ESourcing.Products.Entities;
using ESourcing.Products.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ESourcing.Products.Controllers
{
	[Route("api/v1/{controller}")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		#region Variables

		private readonly IProductRepository _productRepository;
		private readonly ILogger<ProductController> _logger;

		#endregion

		public ProductController(IProductRepository productRepository, ILogger<ProductController> logger)
		{
			_productRepository = productRepository;
			_logger = logger;
		}


		[HttpGet]
		[ProducesResponseType(typeof(Product), (int) HttpStatusCode.OK)]
		public async Task<IActionResult<IEnumerable<Product>>> GetProducts()
		{
			var products = await _productRepository.GetProducts();
			return Ok(products);
		} 

		public IActi

		public IActionResult Index()
		{
			return new StatusCodeResult((int)HttpStatusCode.OK);
		}
	}
}