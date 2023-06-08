using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ELTE.TodoList.Desktop.Model;
using MVCapp.DTOClasses;

namespace MVCapp.Client.Model
{
	public class PollAPIService
	{
		private readonly HttpClient client;

		public PollAPIService(string baseAddress)
		{
			client = new HttpClient();
			{
				client.BaseAddress = new Uri(baseAddress);
			}
		}

		public async Task<bool> LoginAsync(string email, string password)
		{
			LoginDTO user = new LoginDTO
			{
				Id = "",
				Email = email,
				Password = password
			};

			HttpResponseMessage response = await client.PostAsJsonAsync("api/Account/Login", user);

			if (response.IsSuccessStatusCode)
			{
				return true;
			}

			if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
			{
				return false;
			}

			throw new NetworkException("Service returned response: " + response.StatusCode);
		}

		public async Task LogoutAsync()
		{
			HttpResponseMessage response = await client.PostAsync("api/Account/Logout", null);

			if (response.IsSuccessStatusCode)
			{
				return;
			}

			throw new NetworkException("Service returned response: " + response);
		}

		public async Task<IEnumerable<PollDTO>> LoadPollsAsync()
		{
			HttpResponseMessage resp = await client.GetAsync("api/Polls/");

			if (resp.IsSuccessStatusCode)
			{
				return await resp.Content.ReadAsAsync<IEnumerable<PollDTO>>();
			}

			throw new NetworkException("Service returned response: " + resp.StatusCode);
		}

		public async Task<PollDTO> LoadPollAsync(int id)
		{
			HttpResponseMessage resp = await client.GetAsync($"api/Polls/{id}");

			if (resp.IsSuccessStatusCode)
			{
				return await resp.Content.ReadAsAsync<PollDTO>();
			}

			throw new NetworkException("Service returned response: " + resp.StatusCode);
		}

		public async Task CreatePollAsync(PollDTO pollDTO)
		{
			HttpResponseMessage resp = await client.PostAsJsonAsync("api/Polls/", pollDTO);
			pollDTO.Id = (await resp.Content.ReadAsAsync<PollDTO>()).Id;
			
			if (!resp.IsSuccessStatusCode)
			{
				throw new NetworkException("Service returned response: " + resp.StatusCode);
			}
		}

		public async Task<IEnumerable<OptionDTO>> LoadOptionsAsync(int pollid)
		{
			HttpResponseMessage resp = await client.GetAsync($"api/Options");

			if (resp.IsSuccessStatusCode)
			{
				var loaded = await resp.Content.ReadAsAsync<IEnumerable<OptionDTO>>();
				return loaded.Where(x => x.PollId == pollid);
			}

			throw new NetworkException("Service returned response: " + resp.StatusCode);
		}

		public async Task CreateOptionAsync(OptionDTO optionDTO)
		{
			HttpResponseMessage resp = await client.PostAsJsonAsync("api/Options/", optionDTO);
			optionDTO.Id = (await resp.Content.ReadAsAsync<OptionDTO>()).Id;

			if (!resp.IsSuccessStatusCode)
			{
				throw new NetworkException("Service returned response: " + resp.StatusCode);
			}
		}

		public async Task<IEnumerable<LoginDTO>> LoadUsersAsync()
		{
			HttpResponseMessage resp = await client.GetAsync($"api/Voter/");

			if (resp.IsSuccessStatusCode)
			{
				return await resp.Content.ReadAsAsync<IEnumerable<LoginDTO>>();
			}

			throw new NetworkException("Service returned response: " + resp.StatusCode);
		}

		public async Task<LoginDTO> LoadUser(string userid)
		{
			HttpResponseMessage resp = await client.GetAsync($"api/Voter/{userid}");

			if (resp.IsSuccessStatusCode)
			{
				return await resp.Content.ReadAsAsync<LoginDTO>();
			}

			throw new NetworkException("Service returned response: " + resp.StatusCode);
		}

		public async Task AddVoterAsync(int pollid, LoginDTO user)
		{
			Trace.WriteLine($"api/Voter/{pollid}");
			HttpResponseMessage resp = await client.PostAsJsonAsync($"api/Voter/{pollid}", user);

			if (!resp.IsSuccessStatusCode)
			{
				throw new NetworkException("Service returned response: " + resp.StatusCode);
			}
		}

		public async Task<IEnumerable<PollUserDTO>> LoadPollUserAsync(int pollid)
		{
			HttpResponseMessage resp = await client.GetAsync($"api/PollVoter/{pollid}");

			if (resp.IsSuccessStatusCode)
			{
				return await resp.Content.ReadAsAsync<IEnumerable<PollUserDTO>>();
			}

			throw new NetworkException("Service returned response: " + resp.StatusCode);
		}
	}
}
