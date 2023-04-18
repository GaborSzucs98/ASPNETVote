﻿using Microsoft.AspNetCore.Identity;

namespace MVCapp.Models
{
    public class ApplicationUser : IdentityUser
    {
		public virtual ICollection<PollUser> Polls { get; set; }

		public ApplicationUser() : base() {

            Polls = new HashSet<PollUser>();
        }
    }
}
