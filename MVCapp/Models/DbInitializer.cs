using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MVCapp.Models
{
    public static class DbInitializer
    {
        public static void Initialize(VotingDbContext context)
        {
            context.Database.Migrate();

            if (context.Polls.Any())
            {
                return;
            }
            // Első indításnál regisztrálni kell felhasználókat a böngészőben
            // és második inditáskor ha van legalább 1 felhasználó
            // hozzáadja a szavazásokat random felhasználókhoz
            else if (!context.Users.Any())
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

            Random R = new Random((int)DateTime.Now.Ticks);
            int someRandomNumber;
            foreach (Poll poll in defaultPolls)
            {
                // admin/teszt felhasználó
                poll.AddVoter(context.Users.Single(user => user.UserName == "admin@admin.com"));   
                for(int i = 0; i < 2; i++)
                {
                    someRandomNumber = R.Next(0, context.Users.Count());
                    poll.AddVoter(context.Users.ToList()[someRandomNumber]);
                }
            }

            context.AddRange(defaultPolls);
            context.SaveChanges();
            
        }
    }
}
