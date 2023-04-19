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
                string user = userManager.GetUserId(HttpContext.User);
				var result = pollService.GetPollsByUser(user).Where(poll => poll.End < DateTime.Now || poll.Voters.All(voter => voter.Voted == true)).OrderBy(poll => poll.End).ToList();
				return View(result);
			}
			catch(Exception ex)
			{
                Console.WriteLine(ex.Message);
                return NotFound();	
			}
		}
		[HttpPost]

		public IActionResult Index(string search, DateTime start, DateTime end) 
		{
            try
            {
                string user = userManager.GetUserId(HttpContext.User);
                var result = pollService.GetPollsByUser(user).Where(poll => poll.End < DateTime.Now || poll.Voters.All(voter => voter.Voted == true)).OrderBy(poll => poll.End).ToList();
				if(search is not null)
				{
					var newresult = result.Where(poll => poll.Question.ToLower().Contains(search.ToLower()) && poll.Start > start && poll.End < end);
                    return View(newresult);
                }
				else
				{
					var newresult = result.Where(poll => poll.Start > start && poll.End < end);
                    return View(newresult);
				}
				
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

		[HttpGet]
		public IActionResult Details(int id)
		{
			return View(pollService.GetPoll(id));
		}
	}
}
