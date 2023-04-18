using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCapp.Services;
using MVCapp.Models;
using Microsoft.AspNetCore.Identity;

namespace MVCapp.Controllers
{
	[Authorize]
	public class MyPollsController : Controller
	{
		private readonly IPollService pollService;

		public MyPollsController(IPollService pollService)
		{
			this.pollService = pollService;
		}

		[HttpGet]
		public IActionResult Index()
		{
			var user = (ApplicationUser)User.Identity!;
			var result = pollService.GetPollsByUser(user).Where(poll => poll.End > DateTime.Now && poll.Voters.Single(voter => voter.ApplicationUserId == user.Id).Voted == true).ToList();
			return View();
		}
	}
}
