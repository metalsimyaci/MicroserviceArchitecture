using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ESourcing.Core.Common;
using ESourcing.Core.ResultModels;
using ESourcing.UI.ViewModel;
using Newtonsoft.Json;

namespace ESourcing.UI.Clients
{
	public class BidClient
	{
		private readonly HttpClient _client;

		public BidClient(HttpClient client)
		{
			_client = client;
			_client.BaseAddress = new Uri(CommonInfo.BaseAddress);
		}

		public async Task<Result<List<BidViewModel>>> GelAllBidsByAuctionId(string id)
		{
			var response = await _client.GetAsync("/Bid/GetAllBidsByAuctionId?id=" + id);
			if (!response.IsSuccessStatusCode) 
				return new Result<List<BidViewModel>>(false, ResultConstant.RECORD_NOT_FOUND);
			
			var responseData = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<List<BidViewModel>>(responseData);
			return result.Any() 
				? new Result<List<BidViewModel>>(true, ResultConstant.RECORD_FOUND, result.ToList()) 
				: new Result<List<BidViewModel>>(false, ResultConstant.RECORD_NOT_FOUND);
		}

		public async Task<Result<string>> SendBid(BidViewModel model)
		{
			var dataAsString = JsonConvert.SerializeObject(model);
			var content = new StringContent(dataAsString);
			content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
			
			var response = await _client.PostAsync("/Bid", content);
			if (!response.IsSuccessStatusCode) 
				return new Result<string>(false, ResultConstant.RECORD_CREATE_NOT_SUCCESSFULLY);
			
			var responseData = await response.Content.ReadAsStringAsync();
			return new Result<string>(true, ResultConstant.RECORD_CREATE_SUCCESSFULLY, responseData);
		}
	}
}
