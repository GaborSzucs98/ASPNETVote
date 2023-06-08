using ELTE.TodoList.Desktop.Model;
using MVCapp.Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MVCapp.Client.ViewModels
{
    /// <summary>
    /// A bejelentkezés nézetmodell típusa.
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        private readonly PollAPIService _model;
        private Boolean _isLoading;

        /// <summary>
        /// Bejelentkezés parancs lekérdezése.
        /// </summary>
        public DelegateCommand LoginCommand { get; private set; }

        /// <summary>
        /// Felhasználónév lekérdezése, vagy beállítása.
        /// </summary>
        public String UserName { get; set; }

        public Boolean IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Sikeres bejelentkezés eseménye.
        /// </summary>
        public event EventHandler LoginSucceeded;

        /// <summary>
        /// Sikertelen bejelentkezés eseménye.
        /// </summary>
        public event EventHandler LoginFailed;

        /// <summary>
        /// Nézetmodell példányosítása.
        /// </summary>
        /// <param name="model">A modell.</param>
        public LoginViewModel(PollAPIService model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            _model = model;
            UserName = String.Empty;
            IsLoading = false;

            LoginCommand = new DelegateCommand(_ => !IsLoading, param => LoginAsync(param as PasswordBox));
        }

        /// <summary>
        /// Bejelentkezés
        /// </summary>
        /// <param name="passwordBox">Jelszótároló vezérlő.</param>
        private async void LoginAsync(PasswordBox passwordBox)
        {
            if (passwordBox == null)
                return;

            try
            {
                IsLoading = true;
                bool result = await _model.LoginAsync(UserName, passwordBox.Password);
                IsLoading = false;

                if (result)
                    OnLoginSuccess();
                else
                    OnLoginFailed();
            }
            catch (HttpRequestException ex)
            {
                OnMessageApplication($"Server error occurred: ({ex.Message})");
            }
            catch (NetworkException ex)
            {
                OnMessageApplication($"Unexpected error occurred: ({ex.Message})");
            }
        }

        /// <summary>
        /// Sikeres bejelentkezés eseménykiváltása.
        /// </summary>
        private void OnLoginSuccess()
        {
            LoginSucceeded?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Sikertelen bejelentkezés eseménykiváltása.
        /// </summary>
        private void OnLoginFailed()
        {
            LoginFailed?.Invoke(this, EventArgs.Empty);
        }
    }
}
