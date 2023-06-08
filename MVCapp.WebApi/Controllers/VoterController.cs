using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCapp.DTOClasses;
using MVCapp.Persitence;
using MVCapp.Persitence.Services;
using System.Diagnostics;

namespace MVCapp.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[ApiConventionType(typeof(DefaultApiConventions))]
	public class VoterController : ControllerBase
	{
		private readonly IPollService service;

		public VoterController(IPollService service)
		{
			this.service = service;
		}

		[HttpGet("{id}")]
		[Authorize]
		public ActionResult<LoginDTO> GetVoter(string userid)
		{
			var user = service.GetApplicationUser(userid);
			return new LoginDTO
			{
				Email = user.UserName,
				Password = user.PasswordHash,
				Id = user.Id
			};
		}

		[HttpGet]
		[Authorize]
		public ActionResult<IEnumerable<LoginDTO>> GetAllVoters()
		{
			var users = service.GetAllApplicationUser();
			List<LoginDTO> result = new List<LoginDTO>();
			foreach (var user in users)
			{
				LoginDTO login = new LoginDTO
				{
					Email = user.UserName,
					Password = user.PasswordHash,
					Id = user.Id
				};
				result.Add(login);
			}
			return result;
		}

		[HttpPost("{pollid}")]
		//[Authorize]
		public ActionResult AddVoter(Int32 pollid, LoginDTO user) 
		{
			Poll poll = service.GetPoll(pollid);
			if (poll is null)
			{
				return NotFound();
			}

			if (service.AddVoter(pollid, user.Id))
			{
				return Ok();
			}

			return StatusCode(StatusCodes.Status500InternalServerError);
		}
	}
}
