using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCapp.DTOClasses;
using MVCapp.Persitence.Services;

namespace MVCapp.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PollVoterController : ControllerBase
	{
		private readonly IPollService service;
		private readonly IMapper mapper;

		public PollVoterController(IMapper mapper, IPollService service)
		{
			this.service = service;
			this.mapper = mapper;
		}

		[HttpGet("{id}")]
		[Authorize]
		public ActionResult<IEnumerable<PollUserDTO>> GetPollUser(int id)
		{
			return service.GetPoll(id).Voters.Select(x => mapper.Map<PollUserDTO>(x)).ToList();
		}
	}
}
