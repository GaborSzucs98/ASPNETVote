using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCapp.Models;
using MVCapp.Services;

namespace MVCapp.Controllers
{
	[Authorize]
	public class MainPageController : Controller
	{
		private readonly IPollService pollService;

		private readonly UserManager<ApplicationUser> userManager;

		public MainPageController(IPollService pollService, UserManager<ApplicationUser> userManager)
		{
			this.pollService = pollService;
			this.userManager = userManager;
		}

		[HttpGet]
		public IActionResult Index()
		{
			try
			{
				var user = userManager.GetUserId(HttpContext.User); 
				var result = pollService.GetPollsByUser(user).Where(poll => poll.Start > DateTime.Now && poll.End < DateTime.Now && poll.Voters.Single(voter => voter.ApplicationUserId == user).Voted == false).OrderBy(poll => poll.End).ToList();
				return View(result);
			}
			catch(Exception ex)
			{
                Console.WriteLine(ex.Message);
                return NotFound();
			}
			
		}

		[HttpPost]
		public IActionResult VotePage(int id)
		{
			return View(pollService.GetPoll(id));
		}
    }
}
