using ESourcing.Core.ResultModels.Abstract;

namespace ESourcing.Core.ResultModels
{
	public class Result<T>:IResult
	{
		public bool IsSuccess { get; set; }
		public string Message { get; set; }
		public T Data { get; set; }
		public int TotalCount { get; set; }

		public Result(bool isSuccess, string message, T data=default(T), int totalCount = 0)
		{
			IsSuccess = isSuccess;
			Message = message;
			Data = data;
			TotalCount = totalCount;
		}
	}
}
