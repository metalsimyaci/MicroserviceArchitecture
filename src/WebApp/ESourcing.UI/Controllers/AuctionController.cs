using System.Collections.Generic;
using ESourcing.UI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESourcing.UI.Controllers
{
	[Authorize]
	public class AuctionController : Controller
	{

		public AuctionController()
		{
		}
		public IActionResult Index()
		{
			var model = new List<AuctionViewModel>();
			return View(model);
		}

		public IActionResult Detail()
		{
			var model = new AuctionViewModel();
			return View(model);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		//[HttpPost]
		//public IActionResult Create(AuctionViewModel model)
		//{


		//}
	}
}