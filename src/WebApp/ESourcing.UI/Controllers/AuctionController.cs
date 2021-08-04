using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ESourcing.Core.ResultModels;
using ESourcing.Infrastructure.Repositories.Abstract;
using ESourcing.UI.Clients;
using ESourcing.UI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ESourcing.UI.Controllers
{
	[Authorize]
	public class AuctionController : Controller
	{
		private readonly IUserRepository _userRepository;
		private readonly ProductClient _productClient;
		private readonly AuctionClient _auctionClient;
		private readonly BidClient _bidClient;

		public AuctionController(IUserRepository userRepository, ProductClient productClient, AuctionClient auctionClient, BidClient bidClient)
		{
			_userRepository = userRepository;
			_productClient = productClient;
			_auctionClient = auctionClient;
			_bidClient = bidClient;
		}
		public async Task<IActionResult> Index()
		{
			var auctionList = await _auctionClient.GetAuctions();
			return auctionList.IsSuccess ? View(auctionList.Data) : View();
		}

		public async Task<IActionResult> Detail(string id)
		{
			var model = new AuctionBidsViewModel();

			var auctionResponse = await _auctionClient.GetAuctionById(id);
			var bidsResponse = await _bidClient.GelAllBidsByAuctionId(id);

			model.SellerUserName = HttpContext.User?.Identity.Name;
			model.AuctionId = auctionResponse.Data.Id;
			model.ProductId = auctionResponse.Data.ProductId;
			model.Bids = bidsResponse.Data;
			var isAdmin = HttpContext.Session.GetString("IsAdmin");
			model.IsAdmin = Convert.ToBoolean(isAdmin);

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			var userList = await _userRepository.GetAllAsync();
			ViewBag.UserList = userList;

			var productList = await _productClient.GetProducts();
			ViewBag.ProductList = productList;

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(AuctionViewModel model)
		{
			model.Status = default(int);
			model.CreatedAt = DateTime.Now;
			model.IncludedSellers.Add(model.SellerId);
			var createAuction = await _auctionClient.CreateAuction(model);
			if (createAuction.IsSuccess)
				return RedirectToAction("Index");
			return View(model);

		}

		[HttpPost]
		public async Task<Result<string>> SendBid(BidViewModel model)
		{
			model.CreateAt = DateTime.Now;
			var sendBidResponse = await _bidClient.SendBid(model);
			return sendBidResponse;
		}

		[HttpPost]
		public async Task<Result<string>> CompleteBid(string id)
		{
			var completeBidResponse = await _auctionClient.CompleteBid(id);
			return completeBidResponse;
		}
	}
}