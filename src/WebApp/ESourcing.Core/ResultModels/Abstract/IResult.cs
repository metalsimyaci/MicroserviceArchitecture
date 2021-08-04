namespace ESourcing.Core.ResultModels.Abstract
{
	public interface IResult
	{
		public bool IsSuccess { get; set; }
		public string Message { get; set; }
	}
}