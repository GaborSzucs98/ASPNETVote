using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCapp.DTOClasses;
using MVCapp.Persitence;
using MVCapp.Persitence.Services;
using MVCapp.WebApi.Controllers;
using MVCapp.WebApi.MappingConfig;

namespace WebApi.Test
{
	public class ControllerTests : IDisposable
	{
		private readonly VotingDbContext _context;
		private readonly PollService _service;
		private readonly PollsController _pollcontroller;
		private readonly OptionsController _optionscontroller;
		private readonly IMapper _mapper;

		public ControllerTests()
		{
			var options = new DbContextOptionsBuilder<VotingDbContext>()
				.UseInMemoryDatabase("TestDb")
				.Options;

			_context = new VotingDbContext(options);
			TestDbInitializer.Initialize(_context);

			/* 
             * Ezzel az utasítással elengedjük az adatbázis objektumainak követését (tracking).
             * Ez a listák átnevezésének teszteléséhez szükséges, mivel egyébként a
             * PutList megpróbálna új objektumot létrehozni az adatbázisban.
             */
			_context.ChangeTracker.Clear();
			_service = new PollService(_context);

			var config = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile(new PollDtoProfile());
				cfg.AddProfile(new PollProfile());
				cfg.AddProfile(new OptionProfile());
				cfg.AddProfile(new OptionDtoProfile());
			});
			_mapper = new Mapper(config);
			_pollcontroller = new PollsController(_service, _mapper);
			_optionscontroller = new OptionsController(_mapper, _service);
		}

		public void Dispose()
		{
			_context.Database.EnsureDeleted();
			_context.Dispose();
		}

		[Fact]
		public void GetPollsTest()
		{
			var result = _pollcontroller.GetPolls();

			var content = Assert.IsAssignableFrom<IEnumerable<PollDTO>>(result.Value);
			Assert.Equal(3, content.Count());
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		public void GetPollTest(Int32 id)
		{

			var result = _pollcontroller.GetPoll(id);

			var requestResult = Assert.IsAssignableFrom<PollDTO>(result.Value);
		}

		[Fact]
		public void PostPollTest()
		{
			// Arrange
			var newpoll = new PollDTO { Question = "New test question" };
			var count = _context.Polls.Count();

			// Act
			var result = _pollcontroller.PostPoll(newpoll);

			// Assert
			var objectResult = Assert.IsAssignableFrom<CreatedAtActionResult>(result.Result);
			var content = Assert.IsAssignableFrom<PollDTO>(objectResult.Value);
			Assert.Equal(count + 1, _context.Polls.Count());
		}




		[Fact]
		public void GetOptionsTest()
		{
			var result = _optionscontroller.GetOptions();

			var content = Assert.IsAssignableFrom<IEnumerable<OptionDTO>>(result.Value);
			Assert.Equal(23, content.Count());
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		public void GetOptionTest(Int32 id)
		{

			var result = _optionscontroller.GetOption(id);

			var requestResult = Assert.IsAssignableFrom<OptionDTO>(result.Value);
		}

		[Fact]
		public void PostOptionTest()
		{
			// Arrange
			var newoption = new OptionDTO {
				Ans = "New test option",
			};
			var count = _context.Option.Count();

			// Act
			var result = _optionscontroller.CreateOption(newoption);

			// Assert
			var objectResult = Assert.IsAssignableFrom<CreatedAtActionResult>(result.Result);
			var content = Assert.IsAssignableFrom<OptionDTO>(objectResult.Value);
			Assert.Equal(count + 1, _context.Option.Count());
		}
	}
}