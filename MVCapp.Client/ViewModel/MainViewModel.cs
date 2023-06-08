using ELTE.TodoList.Desktop.Model;
using MVCapp.Client.Model;
using MVCapp.Client.ViewModels;
using MVCapp.DTOClasses;
using MVCapp.Persitence;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace MVCapp.Client.ViewModel
{
	public class SelectedPollConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is PollViewModel)
				return value;
			return null!;
		}
	}

	public class MainViewModel : ViewModelBase
	{
		private readonly PollAPIService service;
		private ObservableCollection<PollViewModel> polls;
		private ObservableCollection<OptionViewModel> options;
		private ObservableCollection<UserViewModel> users;
		private PollViewModel selectedPoll;
		private DateTime selectedstart;
		private DateTime selectedend;
		private string newquestion;
		ObservableCollection<LoginDTO> allusers;
		LoginDTO selecteduser;

		public LoginDTO SelectedUser
		{
			get { return selecteduser; }
			set { selecteduser = value; OnPropertyChanged(); }
		}

		public ObservableCollection<LoginDTO> AllUsers 
		{
			get { return allusers; }
			set { allusers = value; OnPropertyChanged(); }
		}

		public string NewQuestion
		{
			get { return newquestion; }
			set { newquestion = value; OnPropertyChanged(); }
		}

		public DateTime SelectedStart
		{
			get { return selectedstart; }
			set { selectedstart = value; OnPropertyChanged(); }
		}

		public DateTime SelectedEnd
		{
			get { return selectedend; }
			set { selectedend = value; OnPropertyChanged(); }
		}

		public ObservableCollection<PollViewModel> Polls
		{
			get { return polls; }
			set { polls = value; OnPropertyChanged(); }
		}

		public PollViewModel SelectedPoll
		{
			get { return selectedPoll; }
			set { selectedPoll = value; OnPropertyChanged(); }
		}

		public ObservableCollection<OptionViewModel> Options
		{
			get { return options; }
			set { options = value; OnPropertyChanged(); }
		}

		public ObservableCollection<UserViewModel> Users
		{
			get { return users; }
			set
			{ users = value; OnPropertyChanged(); }
		}

		public DelegateCommand SelectPollCommand { get; private set; }

		public DelegateCommand RefreshPollsCommand { get; private set; }

		public DelegateCommand LogoutCommand { get; private set; }

		public DelegateCommand AddingNewPollCommand { get; private set; }

		public DelegateCommand AddNewPollCommand { get; private set; }

		public DelegateCommand AddNewOptionCommand { get; private set; }

		public DelegateCommand CancelCommand { get; private set; }

		public DelegateCommand AddUsersCommand { get; private set; }

		public DelegateCommand SavePollCommand { get; private set; }

		public event EventHandler LogoutSucceeded;

		public event EventHandler StartingPollEdit;

		public event EventHandler FinishingPollEdit;

		public MainViewModel(PollAPIService service)
		{
			this.service = service;

			LogoutCommand = new DelegateCommand(_ => LogoutAsync());
			RefreshPollsCommand = new DelegateCommand(_ => LoadPollsAsync());
			SelectPollCommand = new DelegateCommand(_ =>
			{
				if (selectedPoll is null) { return; }
				LoadOptionsAsync(SelectedPoll);
				LoadUsersAsync(SelectedPoll);
				SelectedStart = SelectedPoll.Start;
				SelectedEnd = SelectedPoll.End;
			});
			CancelCommand = new DelegateCommand(_ => CancelNewPollEdit());
			AddNewOptionCommand = new DelegateCommand(_ => AddNewOption());
			AddingNewPollCommand = new DelegateCommand(param => AddNewPoll(param as AddingNewItemEventArgs));
			AddNewPollCommand = new DelegateCommand(_ => AddPoll());
			SavePollCommand = new DelegateCommand(_ => SelectedPoll.validate(),_ => SavePoll());
			AddUsersCommand = new DelegateCommand(_ => AddUsers());

		}

		private void AddUsers()
		{
			var user = SelectedUser;
			if (SelectedPoll.Voters.Any(v => v.Id == user.Id))
			{
				return;
			}
			else
			{
				PollUserDTO pollUser = new PollUserDTO()
				{
					PollId = selectedPoll.Id,
					Voted = false
				};
				UserViewModel newvoter = new UserViewModel(pollUser, user);
				AllUsers.Remove(user);
				SelectedPoll.Voters.Add(newvoter);
			}
		}

		private async void LogoutAsync()
		{
			try
			{
				await service.LogoutAsync();
				OnLogoutSuccess();
			}
			catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
			{
				OnMessageApplication($"Unexpected error occurred! ({ex.Message})");
			}
		}

		private void OnLogoutSuccess()
		{
			LogoutSucceeded?.Invoke(this, EventArgs.Empty);
		}

		private void AddNewPoll(AddingNewItemEventArgs e)
		{
			var pollVm = new PollViewModel();
			pollVm.EditEnded += PollViewModel_EditEnded;
			e.NewItem = pollVm;
		}

		private async Task LoadPollsAsync()
		{
			try
			{
				Polls = new ObservableCollection<PollViewModel>((await service.LoadPollsAsync()).Select(poll =>
				{
					var pollVm = (PollViewModel)poll;
					pollVm.EditEnded += PollViewModel_EditEnded;
					return pollVm;
				}));
			}
			catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
			{
				OnMessageApplication($"Unexpected error occurred! ({ex.Message})");
			}
		}

		private async void PollViewModel_EditEnded(object sender, EventArgs e)
		{
			try
			{
				var pollvm = sender as PollViewModel;
				if(pollvm.Id ==0)
				{
					var polldto = (PollDTO)pollvm;
					await service.CreatePollAsync(polldto);
					pollvm.Id = polldto.Id;
					pollvm.UpdateIds();

					foreach (OptionViewModel option in pollvm.Options)
					{
						await service.CreateOptionAsync((OptionDTO)option);
					}
					foreach (UserViewModel pollUser in pollvm.Voters)
					{
						var user = await service.LoadUser(pollUser.Id);
						await service.AddVoterAsync(pollvm.Id, user);
					}
				}
				else
				{
					pollvm.CancelEdit();
				}
			}
			catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
			{
				OnMessageApplication($"Unexpected error occurred! ({ex.Message})");
			}
		}

		private async void LoadOptionsAsync(PollViewModel pollvm)
		{
			if(pollvm is null || pollvm.Id == 0)
			{
				Options = null;
				return;
			}

			try
			{
				Options = new ObservableCollection<OptionViewModel>();
				var LoadOptions = new ObservableCollection<OptionDTO>(await service.LoadOptionsAsync(pollvm.Id));
				foreach (var option in LoadOptions)
				{
					Options.Add((OptionViewModel)option);
				}
			}
			catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
			{
				OnMessageApplication($"Unexpected error occurred! ({ex.Message})");
			}
		}

		private async void LoadUsersAsync(PollViewModel pollvm)
		{
			if (pollvm is null || pollvm.Id == 0)
			{
				Users = null;
				return;
			}

			try
			{
				Users = new ObservableCollection<UserViewModel>();
				var users = await service.LoadUsersAsync();
				var polluserlist = await service.LoadPollUserAsync(pollvm.Id);
				foreach (var polluser in polluserlist)
				{
					var user = users.Single(u => u.Id == polluser.ApplicationUserId);
					UserViewModel uservm = new UserViewModel(polluser, user);
					Users.Add(uservm);
				}
			}
			catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
			{
				OnMessageApplication($"Unexpected error occurred! ({ex.Message})");
			}
		}

		private void AddNewOption()
		{
			OptionViewModel newopt = new OptionViewModel();
			newopt.Ans = NewQuestion;
			SelectedPoll.Options.Add(newopt);
			NewQuestion = string.Empty;
		}

		private async void AddPoll()
		{
			await LoadPollsAsync();
			var newPoll = new PollViewModel
			{
				Id = 0,			
			};
			Polls.Add(newPoll);
			SelectedPoll = newPoll;
			var users = await service.LoadUsersAsync();
			AllUsers = new ObservableCollection<LoginDTO>(users);
			StartingPollEdit?.Invoke(this, EventArgs.Empty);
		}

		private void CancelNewPollEdit()
		{
			if (SelectedPoll is null || !SelectedPoll.IsDirty)
				return;

			if (SelectedPoll.Id == 0)
			{
				Polls.Remove(SelectedPoll);
				SelectedPoll = null;
			}
			else
			{
				SelectedPoll.CancelEdit();
			}
			FinishingPollEdit?.Invoke(this, EventArgs.Empty);
		}

		private async void SavePoll()
		{
			try
			{
				if (SelectedPoll.Id == 0)
				{
					var polldto = (PollDTO)SelectedPoll;
					await service.CreatePollAsync(polldto);
					SelectedPoll.Id = polldto.Id;
					SelectedPoll.UpdateIds();
					foreach(var option in SelectedPoll.Options)
					{
						var optiondto = (OptionDTO)option;
						await service.CreateOptionAsync(optiondto);
						option.Id = optiondto.Id;
					}
					foreach(var voter in selectedPoll.Voters)
					{
						LoginDTO loginDTO = new LoginDTO
						{
							Email = voter.Email,
							Id = voter.Id,
							Password = ""
						};
						await service.AddVoterAsync(SelectedPoll.Id, loginDTO);
					}
					SelectedPoll.EndEdit();
				}
				else
				{
					throw new Exception("Editing is forbidden!");
				}
			}
			catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
			{
				OnMessageApplication($"Unexpected error occurred! ({ex.Message})");
			}
			FinishingPollEdit?.Invoke(this, EventArgs.Empty);
		}
	}
}
