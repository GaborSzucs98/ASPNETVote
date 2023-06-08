using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCapp.Persitence;

namespace WebApi.Test
{
	public static class TestDbInitializer
	{
		public static void Initialize(VotingDbContext context)
		{
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

			context.AddRange(defaultPolls);
			context.SaveChanges();
		}
	}
}
