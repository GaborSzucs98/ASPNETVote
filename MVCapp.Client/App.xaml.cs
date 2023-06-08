using Microsoft.Win32;
using MVCapp.Client.Model;
using MVCapp.Client.View;
using MVCapp.Client.ViewModel;
using MVCapp.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MVCapp.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
		private PollAPIService service;

		private LoginViewModel loginViewModel;
		private LoginWindow loginView;

		private MainViewModel mainViewModel;
		private MainWindow mainView;

		private NewPollWindow pollWindow;

        public App()
        {
			Startup += App_Startup;
        }

		private void App_Startup(object sender, StartupEventArgs e)
		{
			service = new PollAPIService(ConfigurationManager.AppSettings["baseAddress"]!);

			loginViewModel = new LoginViewModel(service);

			loginViewModel.LoginSucceeded += ViewModel_LoginSucceeded;
			loginViewModel.LoginFailed += ViewModel_LoginFailed;
			loginViewModel.MessageApplication += ViewModel_MessageApplication;

			loginView = new LoginWindow
			{
				DataContext = loginViewModel
			};

			mainViewModel = new MainViewModel(service);
			mainViewModel.LogoutSucceeded += ViewModel_LogoutSucceeded;
			mainViewModel.MessageApplication += MainViewModel_MessageApplication;
			mainViewModel.StartingPollEdit += ViewModel_StartingPollEdit;
			mainViewModel.FinishingPollEdit += ViewModel_FinishingPollEdit;

			mainView = new MainWindow
			{
				DataContext = mainViewModel
			};

			MainWindow = mainView;
			ShutdownMode = ShutdownMode.OnMainWindowClose;
			loginView.Closed += LoginView_Closed;

			loginView.Show();
		}

		private void ViewModel_FinishingPollEdit(object? sender, EventArgs e)
		{
			if (pollWindow.IsActive)
			{
				pollWindow.Close();
			}
		}

		private void ViewModel_StartingPollEdit(object? sender, EventArgs e)
		{
			pollWindow = new NewPollWindow
			{
				DataContext = mainViewModel
			};
			pollWindow.ShowDialog();
			
		}

		private void MainViewModel_MessageApplication(object? sender, MessageEventArgs e)
		{
			MessageBox.Show(e.Message, "VotingApp", MessageBoxButton.OK, MessageBoxImage.Asterisk);
		}

		private void LoginView_Closed(object? sender, EventArgs e)
		{
			Shutdown();
		}

		private void ViewModel_LoginSucceeded(object? sender, EventArgs e)
		{
			loginView.Hide();
			mainView.Show();
		}

		private void ViewModel_LoginFailed(object? sender, EventArgs e)
		{
			MessageBox.Show("Login unsuccessful!", "VotingApp", MessageBoxButton.OK, MessageBoxImage.Asterisk);
		}

		private void ViewModel_LogoutSucceeded(object? sender, EventArgs e)
		{
			mainView.Hide();
			loginView = new LoginWindow
			{
				DataContext = loginViewModel
			};
			loginView.Show();
		}

		private void ViewModel_MessageApplication(object? sender, MessageEventArgs e)
		{
			MessageBox.Show(e.Message, "TodoList", MessageBoxButton.OK, MessageBoxImage.Asterisk);
		}
	}
}
