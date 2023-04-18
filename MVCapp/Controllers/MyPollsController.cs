using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MVCapp.Services;
using MVCapp.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace MVCapp.Controllers
{
	[Authorize]
	public class MyPollsController : Controller
	{
		private readonly IPollService pollService;

        private readonly UserManager<ApplicationUser> userManager;

        public MyPollsController(IPollService pollService, UserManager<ApplicationUser> userManager)
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
                var result = pollService.GetPollsByUser(user).Where(poll => poll.End > DateTime.Now && poll.Voters.Single(voter => voter.ApplicationUserId == user).Voted == true).OrderBy(poll => poll.End).ToList();
				return View(result);
			}
			catch(Exception ex)
			{
                Console.WriteLine(ex.Message);
                return NotFound();	
			}

		}
	}
}
