using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCapp.Models;
using MVCapp.Persitence.Services;
using MVCapp.Persitence;

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
                string user = userManager.GetUserId(HttpContext.User); 
				var result = pollService.GetPollsByUser(user).Where(poll => poll.Start < DateTime.Now && poll.End > DateTime.Now && poll.Voters.Single(voter => voter.ApplicationUserId == user).Voted == false).OrderBy(poll => poll.End).ToList();
				return View(result);
			}
			catch(Exception ex)
			{
                Console.WriteLine(ex.Message);
                return NotFound();
			}
			
		}

		[HttpGet]
		public IActionResult VotePage(int id)
		{
			VoteViewModel vm = new VoteViewModel();
			vm.pollid = id;
			vm.poll = pollService.GetPoll(id);
            return View(vm);
		}

		[HttpPost]
		public IActionResult VotePage(VoteViewModel vm)
		{
			if (vm.optionid > 0)
			{
				vm.poll = pollService.GetPoll(vm.pollid);
				string user = userManager.GetUserId(HttpContext.User);
				pollService.Vote(vm.poll,user,vm.optionid);
				return RedirectToAction(nameof(Index));
			}
			else
			{
                vm.poll = pollService.GetPoll(vm.pollid);
                return View(vm);
			}

        }
    }
}
