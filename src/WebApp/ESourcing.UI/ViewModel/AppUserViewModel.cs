using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ESourcing.UI.ViewModel
{
	public class AppUserViewModel
	{
		[Required(ErrorMessage = "UserName is Required")]
		[Display(Name = "User Name")]
		public string UserName { get; set; }
		[Required(ErrorMessage = "FirstName is Required")]
		[Display(Name = "First Name")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "LastName is Required")]
		[Display(Name = "Last Name")]
		public string LastName { get; set; }

		[Display(Name = "Phone Number")]
		public string PhoneNumber { get; set; }

		[Required(ErrorMessage = "Email is Required")]
		[Display(Name = "Email")]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password is Required")]
		[Display(Name = "Password")]
		[DataType(DataType.Password)]
		public string Password { get; set; }


		[Required(ErrorMessage = "IsBuyer is Required")]
		[Display(Name = "IsBuyer")]
		public bool IsBuyer { get; set; }


		[Required(ErrorMessage = "IsSeller is Required")]
		[Display(Name = "IsSeller")]
		public bool IsSeller { get; set; }

		public int UserSelectTypeId { get; set; }
	}
}
