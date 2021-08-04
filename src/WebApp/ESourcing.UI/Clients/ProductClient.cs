using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ESourcing.Core.Common;
using ESourcing.Core.ResultModels;
using ESourcing.UI.ViewModel;

namespace ESourcing.UI.Clients
{
	public class ProductClient
	{
		private readonly HttpClient _client;

		public ProductClient(HttpClient client)
		{
			_client = client;
			_client.BaseAddress = new Uri(CommonInfo.BaseAddress);
		}

		public async Task<Result<List<ProductViewModel>>> GetProducts()
		{
			var response = await _client.GetAsync("/Product");
			if (!response.IsSuccessStatusCode) return new Result<List<ProductViewModel>>(false, ResultConstant.RecordNotFound);
			var responseData = await response.Content.ReadAsStringAsync();
			var result = JsonSerializer.Deserialize<List<ProductViewModel>>(responseData);
			return result?.Any() ?? false
				? new Result<List<ProductViewModel>>(true, ResultConstant.RecordFound, result.ToList())
				: new Result<List<ProductViewModel>>(false, ResultConstant.RecordNotFound);
		}
	}
}
