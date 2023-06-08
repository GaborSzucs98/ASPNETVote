using MVCapp.Client.ViewModels;
using MVCapp.DTOClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace MVCapp.Client.ViewModel
{
	public class OptionViewModel : ViewModelBase, IEditableObject
	{
		private Int32 id;
		private string ans;
		private int votes;
		private int pollid;

		public Int32 Id { get { return id; } set { id = value; OnPropertyChanged(); } }
		public string Ans { get { return ans; } set { ans = value; OnPropertyChanged(); } }
		public int Votes { get { return votes; } set { votes = value; OnPropertyChanged(); } }
		public int PollId { get { return pollid; } set { pollid = value; OnPropertyChanged(); } }

		private Boolean _isDirty = false;
		private OptionViewModel _backup;

		public event EventHandler EditEnded;

		public OptionViewModel()
		{
			PollId = 0;
			Id = 0;
			votes = 0;
		}

		public static explicit operator OptionViewModel(OptionDTO dto) => new OptionViewModel
		{
			Id = dto.Id,
			Votes = dto.Votes,
			PollId = dto.PollId,
			Ans= dto.Ans,
		};

		public static explicit operator OptionDTO(OptionViewModel vm) => new OptionDTO
		{
			Id = vm.Id,
			Votes = vm.Votes,
			PollId = vm.PollId,
			Ans = vm.Ans,
		};

		public void BeginEdit()
		{
			if (!_isDirty)
			{
				_backup = (OptionViewModel)this.MemberwiseClone();
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
	}

	public class OptionValidationRule : ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			OptionViewModel opt = (value as BindingGroup).Items[0] as OptionViewModel;

			if (String.IsNullOrEmpty(opt.Ans))
			{
				return new ValidationResult(false,
					"Answer cannot be empty.");
			}
			else if (opt.Votes != 0)
			{
				return new ValidationResult(false,
					"Starting votes must be 0!");
			}
			else
			{
				return ValidationResult.ValidResult;
			}
		}
	}
}
