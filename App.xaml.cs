using Serilog;
using Prism.Unity;
using LiveChartPlay.Views;
using LiveChartPlay.Services;
using System.Windows;
using LiveChartPlay.ViewModels;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;
using Prism.Ioc;
using LiveChartPlay.Helpers;
using System.Windows.Documents;
using LiveChartPlay.Models;


namespace LiveChartPlay
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        public bool _isLoginSuccess = false;
        public App()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.Debug()
                .WriteTo.File("logs\\log.txt",
                rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information("Launched");

        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Application.Current.MainWindow != null)
            {
                Log.Information("MainWindow is assigned");
            }
            else
            {
                Log.Error("MainWindow is null after initialization.");
                Application.Current.Shutdown();
            }

        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += (s, ev) =>
            {
                var ex = (Exception)ev.ExceptionObject;
                Log.Fatal(ex, "unhandled exception");
                MessageBox.Show($"unhandled : {ex.Message}");

            };

            try
            {
                Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

                Log.Information("on Startup...");
                base.OnStartup(e);
                Log.Information("end on Startup");

                var userRepo = Container.Resolve<IUserRepository>();
                await InitializeUsersAsync(userRepo);

                Application.Current.Exit += (s, ev) =>
                {
                    Log.Warning("Application.Exit event fired");
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Startup Exception");
                MessageBox.Show($"Fatal error: {ex.Message}");
            }

        }

        protected override void OnExit(ExitEventArgs e)
        {
            Log.Information("Exited");
            Log.CloseAndFlush();
            base.OnExit(e);
        }

        protected override Window CreateShell()
        {
            Log.Information("CreateShell() called");

            var windowService = Container.Resolve<IWindowService>();
            var loginService = Container.Resolve<ILoginService>();
            var appState = Container.Resolve<IAppStateService>();

            var loginViewModel = new LoginViewModel(loginService, appState);

            var result = windowService.ShowDialogWindow<LoginWindow, LoginViewModel>(loginViewModel);

            if (result != true  || !appState.IsAuthenticated)
            {
                Log.Warning("Login failed or canceled. Returning null");
                return null!;
            }

            Log.Information("Login succeeded. Resolving MainWindow");
            var mainWindow = Container.Resolve<MainWindow>();
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            mainWindow.Closed += (s, e) =>
            {
                Log.Warning("MainWindow.Closed fired");
            };

            return mainWindow;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var config = ConfigurationBuilderHelper.BuildConfiguration();

            containerRegistry.RegisterInstance<IConfiguration>(config);

            // サービス登録
            containerRegistry.Register<IWorkTimeCalculator, WorkTimeCalculator>();


            // Singleton
            containerRegistry.Register<IMessengerService, MessengerService>();
            containerRegistry.RegisterSingleton<IEventAggregator, EventAggregator>();
            containerRegistry.RegisterSingleton<IAppStateService, AppStateService>();
            containerRegistry.RegisterSingleton<ILoginService, LoginService>();
            // でもその時点では、まだ MainWindow のインスタンスは生成されていないためDockManager にアクセスできないのです。
            // Viewで手動入れ込み
            containerRegistry.RegisterSingleton<IDocumentService, DocumentService>();

            //  ViewModel
            containerRegistry.Register<WorkTimeSummaryViewModel>();
            containerRegistry.Register<WorkTimeInputViewModel>();
            containerRegistry.Register<WorkTimeResultViewModel>();
            containerRegistry.Register<WorkTimeCommandPanelViewModel>();
            containerRegistry.Register<WorkTimeChartViewModel>();

            // View 
            containerRegistry.Register<IWindowService, WindowService>();
            containerRegistry.Register<IViewHostService, ViewHostService>();
            containerRegistry.Register<WorkTimeSummaryView>();
            containerRegistry.Register<WorkTimeChartView>();


            //var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=postgres";
            containerRegistry.Register<IWorkTimeRepository, WorkTimeRepository>();
            containerRegistry.Register<IUserRepository, UserRepository>();
        }

        public async Task InitializeUsersAsync(IUserRepository userRepository)
        {
            if (!await userRepository.AnyUsersAsync())
            {
                var testUser = new UserInfo(
                    username: "admin",
                    password: PasswordHasher.HashPassword("password"), 
                    email: "admin@example.com",
                    role: UserRole.Admin | UserRole.Editor | UserRole.Viewer //all 
                );

                await userRepository.InsertUserAsync(testUser);

                Serilog.Log.Information("Admin user created: admin / password");
            }
            else
            {
                Log.Information("There is users ");

            }
        }

    }

}
