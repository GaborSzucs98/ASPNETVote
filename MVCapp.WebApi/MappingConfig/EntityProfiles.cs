using AutoMapper;
using MVCapp.Persitence;
using MVCapp.DTOClasses;

namespace MVCapp.WebApi.MappingConfig
{
	public class UserProfile : Profile
	{
		public UserProfile() 
		{
			CreateMap<ApplicationUser, UserDTO>();
		}
	}

	public class PollProfile : Profile
	{
		public PollProfile()
		{
			CreateMap<Poll, PollDTO>();
		}
	}

	public class PollUserProfile : Profile
	{
		public PollUserProfile()
		{
			CreateMap<PollUser, PollUserDTO>();
		}
	}

	public class OptionProfile : Profile
	{
		public OptionProfile()
		{
			CreateMap<Option, OptionDTO>();
		}
	}
}
