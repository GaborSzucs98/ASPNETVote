using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCapp.Persitence;
using MVCapp.Persitence.Services;
using MVCapp.DTOClasses;
using Microsoft.AspNetCore.Authorization;

namespace MVCapp.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[ApiConventionType(typeof(DefaultApiConventions))]
	public class OptionsController : ControllerBase
	{
		private readonly IMapper mapper;
		private readonly IPollService service;

		public OptionsController(IMapper mapper, IPollService service) 
		{
			this.mapper = mapper;
			this.service = service;
		}

		[HttpGet("{id}")]
		[Authorize]
		public ActionResult<OptionDTO> GetOption(int id)
		{
			return mapper.Map<OptionDTO>(service.GetOption(id));
		}

		[HttpGet]
		[Authorize]
		public ActionResult<IEnumerable<OptionDTO>> GetOptions()
		{
			try
			{
				return service.GetAllOptions().Select(option => mapper.Map<OptionDTO>(option)).ToList();
			}
			catch(Exception)
			{
				return NotFound();
			}
		}

		[HttpPost]
		[Authorize]
		public ActionResult<OptionDTO> CreateOption(OptionDTO optionDto)
		{
			var option = mapper.Map<Option>(optionDto);
			var newoption = service.CreateOption(option);

			if(newoption is null)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
			else
			{
				return CreatedAtAction(nameof(GetOption), new { id = option.Id }, mapper.Map<OptionDTO>(option));
			}
		}
	}
}
