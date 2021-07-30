using Microsoft.AspNetCore.Mvc;

namespace ESourcing.UI.Controllers
{
	public class HomeController : Controller
	{
		// GET
		public IActionResult Index()
		{
			return View();
		}
	}
}