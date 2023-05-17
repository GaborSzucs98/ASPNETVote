using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCapp.DTOClasses;
using MVCapp.Persitence;

namespace MVCapp.WebApi.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	[ApiConventionType(typeof(DefaultApiConventions))]
	public class AccountController : ControllerBase
	{
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(SignInManager<ApplicationUser> signInManager)
		{
			_signInManager = signInManager;
		}

		// api/Account/Login
		[HttpPost]
		public async Task<IActionResult> Login([FromBody] UserDTO user)
		{
			if (_signInManager.IsSignedIn(User))
				await _signInManager.SignOutAsync();

			var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password,
				isPersistent: false, lockoutOnFailure: false);

			if (result.Succeeded)
			{
				return Ok();
			}

			return Unauthorized("A bejelentkezés sikertelen!");
		}

		// api/Account/Logout
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();

			return Ok();
		}
	}
}
