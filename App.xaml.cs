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


namespace LiveChartPlay
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
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

        protected override void OnStartup(StartupEventArgs e)
        {


            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Log.Information("Exited");
            Log.CloseAndFlush();
            base.OnExit(e);
        }

        protected override Window CreateShell()
        {
            // // ← ここで MainWindow の依存をDIで解決！
            return Container.Resolve<MainWindow>();
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



        }
    }

}
