using Microsoft.AspNetCore.Mvc;
using MVCapp.Persitence.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using MVCapp.DTOClasses;
using MVCapp.Persitence;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MVCapp.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
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
		[Authorize]
		[HttpGet]
		public ActionResult<IEnumerable<PollDTO>> GetPolls()
		{
			return service.GetPolls().Select(poll => mapper.Map<PollDTO>(poll)).ToList();
		}

		// GET api/<ValuesController>/5
		[Authorize]
		[HttpGet("{id}")]
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
		public ActionResult<PollDTO> Post(PollDTO pollDTO)
		{
			var Poll = service.CreatePoll(mapper.Map<Poll>(pollDTO));
		}

		// PUT api/<ValuesController>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE api/<ValuesController>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
