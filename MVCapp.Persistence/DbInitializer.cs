using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MVCapp.Persitence
{
    public static class DbInitializer
    {

		private static VotingDbContext context = null!;
		private static UserManager<ApplicationUser> userManager = null!;
		public static void Initialize(IServiceProvider serviceProvider)
        {
			context = serviceProvider.GetRequiredService<VotingDbContext>();
			userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

			context.Database.Migrate();

            if (context.Polls.Any())
            {
                return;
            }

            var users = SeedUsers();

			List<Poll> defaultPolls = new List<Poll>
            {
                new Poll
                {
                    Question = "Miért kék az ég?",
                    Start = DateTime.Now,
                    End = DateTime.Now.AddDays(30),
                    Options = new List<Option>()
                    {
                        new Option()
                        {
                            Ans = "Az óceánok miatt",
                            Votes= 0
                        },
                        new Option()
                        {
                            Ans = "Rayleigh-féle fényszórás miatt",
                            Votes= 0
                        }
                    }
                    
                },

                new Poll
                {
                    Question = "Mennyi?",
                    Start = DateTime.Now.AddDays(-10),
                    End = DateTime.Now.AddDays(-1),
                    Options = new List<Option>()
                    {
                        new Option()
                        {
                            Ans = "Ennyi",
                            Votes= 0
                        },
                        new Option()
                        {
                            Ans = "Annyi",
                            Votes= 0
                        },
                        new Option()
                        {
                            Ans = "Megannyi",
                            Votes= 0
                        },
                        new Option()
                        {
                            Ans = "Valamennyi",
                            Votes= 0
                        }
                    }
                    
                },

                new Poll
                {
                    Question = "Melyik a legjobb JDM autó?",
                    Start = DateTime.Now.AddDays(-10),
                    End = DateTime.Now.AddDays(30),
                    Options = new List<Option>()
                    {
                        new Option()
                        {
                            Ans = "RX7",
                            Votes= 0
                        },
                        new Option()
                        {
                            Ans = "AE86",
                            Votes= 0
                        },
                        new Option()
                        {
                            Ans = "Skyline GT-R",
                            Votes= 0
                        },
                        new Option()
                        {
                            Ans = "Silvia",
                            Votes= 0
                        },
                        new Option()
                        {
                            Ans = "180SX",
                            Votes= 0
                        },
                        new Option()
                        {
                            Ans = "NSX",
                            Votes= 0
                        },
                        new Option()
                        {
                            Ans = "Civic Type-R",
                            Votes= 0
                        },
                        new Option()
                        {
                            Ans = "350Z",
                            Votes= 0
                        },
                        new Option()
                        {
                            Ans = "370Z",
                            Votes= 0
                        },
                        new Option()
                        {
                            Ans = "Lancer EVO",
                            Votes= 0
                        },
                        new Option()
                        {
                            Ans = "Impreza",
                            Votes= 0
                        },
                        new Option()
                        {
                            Ans = "S2000",
                            Votes= 0
                        },
                        new Option()
                        {
                            Ans = "Supra",
                            Votes= 0
                        },
                        new Option()
                        {
                            Ans = "Integra",
                            Votes= 0
                        },
                        new Option()
                        {
                            Ans = "Miata",
                            Votes= 0
                        },
                        new Option()
                        {
                            Ans = "Celica GT-Four",
                            Votes= 0
                        },
                        new Option()
                        {
                            Ans = "MR-2",
                            Votes= 0
                        }
                    }
                }
            };

            defaultPolls[0].AddVoter(users[0]);
			defaultPolls[0].AddVoter(users[1]);

			defaultPolls[1].AddVoter(users[0]);
			defaultPolls[1].AddVoter(users[1]);

			defaultPolls[2].AddVoter(users[0]);

            context.AddRange(defaultPolls);
            context.SaveChanges();
            
        }
		private static List<ApplicationUser> SeedUsers()
		{
			var adminUser = new ApplicationUser
			{
				UserName = "admin@admin.hu",
				Email = "admin@admin.hu",
			};
			var adminPassword = "admin";

			var result1 = userManager.CreateAsync(adminUser, adminPassword).Result;

			var adminUser2 = new ApplicationUser
			{
				UserName = "test@test.hu",
				Email = "test@test.hu",
			};
			var adminPassword2 = "test";

			var result2 = userManager.CreateAsync(adminUser2, adminPassword2).Result;

            return new List<ApplicationUser> { adminUser, adminUser2 };
		}
	}
}
