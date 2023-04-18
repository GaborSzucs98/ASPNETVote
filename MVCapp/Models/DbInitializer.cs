using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace MVCapp.Models
{
    public static class DbInitializer
    {
        public static void Initialize(VotingDbContext context, IPasswordHasher<IdentityUser> passwordHasher)
        {
            context.Database.Migrate();

			if (context.Polls.Any())
            {
                return;
            }

            List<ApplicationUser> defaultusers = new List<ApplicationUser>()
            {
				new ApplicationUser
                {
                    UserName = "admin",
                },
                new ApplicationUser
                {
                    UserName = "gaborszucs98@gmail.com"
                }
            };

            defaultusers[0].PasswordHash = passwordHasher.HashPassword(defaultusers[0], "admin");
			defaultusers[1].PasswordHash = passwordHasher.HashPassword(defaultusers[1], "Sonicx");

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

            foreach (Poll poll in defaultPolls)
            {
                poll.AddVoter(defaultusers[0]);
                Console.WriteLine(defaultusers[0].UserName);
			}

            defaultPolls[2].AddVoter(defaultusers[1]);
			Console.WriteLine(defaultusers[1].UserName);

			context.AddRange(defaultusers);
			context.AddRange(defaultPolls);
            context.SaveChanges();
        }
    }
}
