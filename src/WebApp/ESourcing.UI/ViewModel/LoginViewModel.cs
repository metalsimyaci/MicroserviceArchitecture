using System.ComponentModel.DataAnnotations;

namespace ESourcing.UI.ViewModel
{
	public class LoginViewModel
	{
		[EmailAddress]
		[Display(Name = "E-Posta")]
		[Required(ErrorMessage = "E-Posta bilgisi zorunludur")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Parola alanı gereklidir")]
		[DataType(DataType.Password)]
		[MinLength(4,ErrorMessage = "Parola alanı minimum {0} karakter olmalıdır")]
		public string Password { get; set; }
	}
}
