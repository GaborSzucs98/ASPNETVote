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

            SeedUser("admin");
			SeedUser("test1");
			SeedUser("test2");

			context.AddRange(defaultPolls);
			context.SaveChanges();

            var polls = context.Polls.ToList();
			var users = userManager.Users.ToList();

            polls[0].AddVoter(users[0]);
			polls[0].AddVoter(users[1]);

			polls[1].AddVoter(users[0]);
			polls[1].AddVoter(users[1]);

			polls[2].AddVoter(users[0]);

            context.SaveChanges();            
        }
		private static void SeedUser(string input)
		{
			var adminUser = new ApplicationUser
			{
				UserName = $"{input}@admin.hu",
				Email = $"{input}@admin.hu",
			};
            var adminPassword = input;

			var result1 = userManager.CreateAsync(adminUser, adminPassword).Result;
		}
	}
}
