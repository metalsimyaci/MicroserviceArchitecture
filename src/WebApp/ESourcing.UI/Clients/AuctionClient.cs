using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ESourcing.Core.Common;
using ESourcing.Core.ResultModels;
using ESourcing.UI.ViewModel;
using Newtonsoft.Json;

namespace ESourcing.UI.Clients
{
	public class AuctionClient
	{
		private readonly HttpClient _client;

		public AuctionClient(HttpClient client)
		{
			_client = client;
			_client.BaseAddress = new Uri(CommonInfo.BaseAddress);
		}

		public async Task<Result<List<AuctionViewModel>>> GetAuctions()
		{
			var response = await _client.GetAsync("/Auction");
			if (!response.IsSuccessStatusCode) 
				return new Result<List<AuctionViewModel>>(false, ResultConstant.RECORD_NOT_FOUND);
			
			var responseData = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<List<AuctionViewModel>>(responseData);
			return result.Any() 
				? new Result<List<AuctionViewModel>>(true, ResultConstant.RECORD_FOUND, result.ToList()) 
				: new Result<List<AuctionViewModel>>(false, ResultConstant.RECORD_NOT_FOUND);
		}

		public async Task<Result<AuctionViewModel>> CreateAuction(AuctionViewModel model)
		{
			var dataAsString = JsonConvert.SerializeObject(model);
			var content = new StringContent(dataAsString);
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			
			var response = await _client.PostAsync("/Auction", content);
			if (!response.IsSuccessStatusCode) 
				return new Result<AuctionViewModel>(false, ResultConstant.RECORD_CREATE_NOT_SUCCESSFULLY);
			
			var responseData = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<AuctionViewModel>(responseData);
			return result != null 
				? new Result<AuctionViewModel>(true, ResultConstant.RECORD_CREATE_SUCCESSFULLY, result) 
				: new Result<AuctionViewModel>(false, ResultConstant.RECORD_CREATE_NOT_SUCCESSFULLY);
		}

		public async Task<Result<AuctionViewModel>> GetAuctionById(string id)
		{
			var response = await _client.GetAsync("/Auction/" + id);
			if (!response.IsSuccessStatusCode) 
				return new Result<AuctionViewModel>(false, ResultConstant.RECORD_NOT_FOUND);
			
			var responseData = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<AuctionViewModel>(responseData);
			return result != null 
				? new Result<AuctionViewModel>(true, ResultConstant.RECORD_FOUND, result) 
				: new Result<AuctionViewModel>(false, ResultConstant.RECORD_NOT_FOUND);
		}

		public async Task<Result<string>> CompleteBid(string id)
		{
			var dataAsString = JsonConvert.SerializeObject(id);
			var content = new StringContent(dataAsString);
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			
			var response = await _client.PostAsync("/Auction/CompleteAuction", content);
			if (!response.IsSuccessStatusCode) 
				return new Result<string>(false, ResultConstant.RECORD_CREATE_NOT_SUCCESSFULLY);
			
			var responseData = await response.Content.ReadAsStringAsync();
			return new Result<string>(true, ResultConstant.RECORD_CREATE_SUCCESSFULLY, responseData);
		}
	}
}
