using MVCapp.Client.ViewModels;
using MVCapp.DTOClasses;
using MVCapp.Persitence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml.Linq;

namespace MVCapp.Client.ViewModel
{
	public class PollViewModel : ViewModelBase, IEditableObject
	{
		private int id;
		private string question;
		private DateTime start;
		private DateTime end;
		private ObservableCollection<OptionViewModel> options;
		private ObservableCollection<UserViewModel> voters;

		private Boolean _isDirty = false;
		private PollViewModel _backup;

		public Boolean IsDirty { get { return _isDirty; } private set { _isDirty = value; } }

		public int Id { get { return id; } set { id = value; OnPropertyChanged(); } }

		public String Question { get { return question; } set { question = value; OnPropertyChanged(); } }

		public DateTime Start { get { return start; } set { start = value; OnPropertyChanged(); } }

		public DateTime End { get { return end; } set { end = value; OnPropertyChanged(); } }

		public ObservableCollection<OptionViewModel> Options { get { return options; } set { options = value; OnPropertyChanged(); } }

		public ObservableCollection<UserViewModel> Voters { get { return voters; } set { voters = value; OnPropertyChanged(); } }

		public PollViewModel()
		{
			Options = new ObservableCollection<OptionViewModel>();
			Voters = new ObservableCollection<UserViewModel>();
			Start = DateTime.Today;
			End = DateTime.Today;
		}

		public static explicit operator PollViewModel(PollDTO dto) => new PollViewModel
		{
			Id = dto.Id,
			Question = dto.Question,
			Start = dto.Start,
			End = dto.End,
		};

		public static explicit operator PollDTO(PollViewModel vm) => new PollDTO
		{
			Id = vm.Id,
			Question = vm.Question,
			Start = vm.Start,
			End = vm.End,
		};

		public void UpdateIds()
		{
			foreach (OptionViewModel option in Options)
			{
				option.PollId = Id;
			}
			foreach (UserViewModel user in Voters)
			{
				user.PollId = Id;
			}
		}

		public event EventHandler EditEnded;

		public void BeginEdit()
		{
			if (!_isDirty)
			{
				_backup = (PollViewModel)this.MemberwiseClone();
				_isDirty = true;
			}
		}

		public void CancelEdit()
		{
			if (_isDirty)
			{
				Id = _backup.Id;
				_isDirty = false;
				_backup = null;
			}
		}

		public void EndEdit()
		{
			if (_isDirty)
			{
				EditEnded?.Invoke(this, EventArgs.Empty);
				_isDirty = false;
				_backup = null;
			}
		}

		public bool validate()
		{
			if (String.IsNullOrEmpty(Question))
			{
				return false;	
			}
			else if (Start < DateTime.Now)
			{
				return  false;
			}
			else if (End < Start.AddMinutes(15))
			{
				return  false;
			}
			else if (Options.Count < 2)
			{
				return  false;
			}
			else if (Voters.Count < 2)
			{
				return  false;
			}
			else
			{
				return true;
			}
		}
	}

	public class PollValidationRule : ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			PollViewModel poll = (value as BindingGroup).Items[0] as PollViewModel;

			if (String.IsNullOrEmpty(poll.Question))
			{
				return new ValidationResult(false,
					"Question cannot be empty.");
			}
			else if (poll.Start < DateTime.Now)
			{
				return new ValidationResult(false,
					"Set start date!");
			}
			else if (poll.End < poll.Start.AddMinutes(15))
			{
				return new ValidationResult(false,
					"Set end date!.");
			}
			else if (poll.Options.Count < 2)
			{
				return new ValidationResult(false,
					"Must have at least 2 options!.");
			}
			else if (poll.Voters.Count < 2)
			{
				return new ValidationResult(false,
					"Must have at least 2 voters assigned!.");
			}
			else
			{
				return ValidationResult.ValidResult;
			}
		}
	}
}
