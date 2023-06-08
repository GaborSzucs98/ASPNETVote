using AutoMapper;
using MVCapp.Persitence;
using MVCapp.DTOClasses;

namespace MVCapp.WebApi.MappingConfig
{
	public class PollProfile : Profile
	{
		public PollProfile()
		{
			CreateMap<Poll, PollDTO>();
		}
	}

	public class PollDtoProfile : Profile
	{
		public PollDtoProfile()
		{
			CreateMap<PollDTO, Poll>();
		}
	}

	public class PollUserProfile : Profile
	{
		public PollUserProfile()
		{
			CreateMap<PollUser, PollUserDTO>();
		}
	}

	public class PollUserDtoProfile : Profile
	{
		public PollUserDtoProfile()
		{
			CreateMap<PollUserDTO, PollUser>();
		}
	}

	public class OptionProfile : Profile
	{
		public OptionProfile()
		{
			CreateMap<Option, OptionDTO>();
		}
	}

	public class OptionDtoProfile : Profile
	{
		public OptionDtoProfile()
		{
			CreateMap<OptionDTO, Option>();
		}
	}
}
