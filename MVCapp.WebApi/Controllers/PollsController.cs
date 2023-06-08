using Microsoft.AspNetCore.Mvc;
using MVCapp.Persitence.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using MVCapp.DTOClasses;
using MVCapp.Persitence;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MVCapp.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[ApiConventionType(typeof(DefaultApiConventions))]
	public class PollsController : ControllerBase
	{
		private readonly IPollService service;
		private readonly IMapper mapper;

		public PollsController(IPollService service, IMapper mapper)
		{
			this.service = service;
			this.mapper = mapper;
		}

		// GET: api/<ValuesController>
		[HttpGet]
		[Authorize]
		public ActionResult<IEnumerable<PollDTO>> GetPolls()
		{
			return service.GetPolls().Select(poll => mapper.Map<PollDTO>(poll)).ToList();
		}

		// GET api/<ValuesController>/5
		[HttpGet("{id}")]
		[Authorize]
		public ActionResult<PollDTO> GetPoll(int id)
		{
			try
			{
				return mapper.Map<PollDTO>(service.GetPoll(id));
			}
			catch (InvalidOperationException)
			{
				return NotFound();
			}
		}

		// POST api/<ValuesController>
		[Authorize]
		[HttpPost]
		public ActionResult<PollDTO> PostPoll(PollDTO pollDTO)
		{
			var poll = service.CreatePoll(mapper.Map<Poll>(pollDTO));
			if(poll is null)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
			else
			{
				return CreatedAtAction(nameof(GetPoll), new { id = poll.Id },
					mapper.Map<PollDTO>(poll));
			}
		}
	}
}
