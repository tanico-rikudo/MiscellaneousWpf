using LiveChartPlay.Models;
using LiveChartPlay.Services;
using Reactive.Bindings;
using Serilog;
using System.Windows;


using LiveChartPlay.Services.Auth;
using LiveChartPlay.Services.UI;
using MessageBox = System.Windows.MessageBox;


namespace LiveChartPlay.ViewModels.Auth
{



    public interface IClosable
    {
        event Action? CloseRequested;
    }

    public class LoginViewModel : IClosable
    {
        public ReactiveProperty<string> Username { get; } = new("");
        public ReactiveProperty<string> Password { get; } = new("");
        public ReactiveCommand LoginCommand { get; }
        public ReactiveCommand CancelCommand { get; }


        private readonly ILoginService _loginService;
        private readonly IAppStateService _appState;
        public event Action? CloseRequested;

        public LoginViewModel(ILoginService loginService, IAppStateService appState)
        {
            _loginService = loginService;
            _appState = appState;

            LoginCommand = new ReactiveCommand();
            LoginCommand.Subscribe(async _ =>
            {
                var userInfo = await _loginService.AuthenticateAsync(Username.Value, Password.Value);
                if (userInfo != null)
                {
                    Log.Information("Login successful.");
                    appState.CurrentUser = userInfo;
                    CloseRequested?.Invoke();
                }
                else
                {
                    Log.Warning("Login failed.");
                    MessageBox.Show("ログイン失敗しました", "Authentication Failure", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            });

            CancelCommand = new ReactiveCommand();
            CancelCommand.Subscribe(_ =>
            {
                Log.Information("Login canceled.");
                CloseRequested?.Invoke();
            });
        }
    }
}
