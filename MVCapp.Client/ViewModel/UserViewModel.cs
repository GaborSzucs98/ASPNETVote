using MVCapp.DTOClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCapp.Client.ViewModel
{
	public class UserViewModel
	{
		public string Id { get; set; }

		public string Email { get; set; }

		public bool Voted { get; set; }

		public int PollId { get; set; }

		public UserViewModel(PollUserDTO pollUser, LoginDTO user)
		{
			Id = user.Id;
			Email = user.Email;
			Voted = pollUser.Voted;
			PollId = pollUser.PollId;
		}
	}
}
