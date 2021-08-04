using System.Resources;
using System.Threading.Tasks;
using ESourcing.Core.Entities;
using ESourcing.UI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ESourcing.UI.Controllers
{
	public class HomeController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;

		public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}
		[Authorize]
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel loginModel, string returnUrl)
		{
			returnUrl ??= Url.Content("~/");

			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(loginModel.Email);
				if (user != null)
				{
					await _signInManager.SignOutAsync();
					var result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, false, false);
					if (result.Succeeded)
					{
						HttpContext.Session.SetString("IsAdmin",user.IsAdmin.ToString());
						//return RedirectToAction("Index");
						LocalRedirect(returnUrl);
					}
					else
						ModelState.AddModelError("", "Email address or password is not valid");
				}
				else
					ModelState.AddModelError("", "Email address or password is not valid");

			}
			return View(loginModel);
		}

		public IActionResult SignUp()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SignUp(AppUserViewModel signUpModel)
		{
			if (ModelState.IsValid)
			{
				AppUser user = new AppUser
				{
					FirstName = signUpModel.FirstName,
					UserName = signUpModel.UserName,
					Email = signUpModel.Email,
					LastName = signUpModel.LastName,
					IsBuyer = signUpModel.UserSelectTypeId==1,
					IsSeller = signUpModel.UserSelectTypeId==2,
					
					
				};

				var result =await _userManager.CreateAsync(user, signUpModel.Password);

				if (result.Succeeded)
					return RedirectToAction("Login");
				else
				{
					foreach (var item in result.Errors)
					{
						ModelState.AddModelError("",item.Description);
					}
				}
			}
			return View(signUpModel);
		}

		public async  Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Login");
		}
	}
}