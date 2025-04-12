using System.Windows;
using LiveChartPlay.Services;
using LiveChartPlay.ViewModels;
using MaterialDesignThemes.Wpf;
using MahApps.Metro.Controls;
using Serilog;

namespace LiveChartPlay.Views
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private readonly SnackbarMessageQueue _snackbarQueue = new SnackbarMessageQueue();


        ~MainWindow()
        {
            System.Diagnostics.Debugger.Break(); // Dispose時にブレーク
        }

        public MainWindow(MainViewModel viewModel, IMessengerService messenger, IContainerProvider container, IAppStateService appStateService)
        {
            try
            {
                InitializeComponent();
                DataContext = viewModel;

                MySnackbar.MessageQueue = _snackbarQueue;

                // ViewModel や Service からのメッセージ通知を受け取り、Snackbar に流す
                // viewmodelを経由しない実装にしているので、ここに書いている
                messenger.Subscribe<string>(msg => _snackbarQueue.Enqueue(msg));
                Log.Information("Mainwinfow loadeding");

                // Prism のコンテナに手動登録
                //var documentService = new DocumentService(containerProvider, DockManager);
                //var registry = Prism.Ioc.ContainerLocator.Container.Reslve<IContainerRegistry>();
                //registry.RegisterInstance<IDocumentService>(documentService);

                this.Closed += (s,v) =>
                {
                    Log.Warning("MainWindow Closed");
                };


                this.Loaded += (_, _) =>
                {
                    Log.Information("MainWindow Loaded event fired");
                    this.WindowState = WindowState.Normal;
                    this.Activate();
                    this.Topmost = true;    // 一時的に最前面に出す
                    this.Topmost = false;
                    if (DockManager == null)
                    {
                        Log.Warning("DockManager is null in Loaded event");
                    }
                    var documentService = new DocumentService(container, DockManager);
                    documentService.RegisterDocument<WorkTimeSummaryView, WorkTimeSummaryViewModel>("summary", "Summary View");
                    Log.Information("Mainwinfow loadeding2");
                    documentService.RegisterDocument<WorkTimeChartView, WorkTimeChartViewModel>("chart", "chart View");
                    Log.Information("Mainwinfow loadeding3");

                    // ViewModel に渡す（例：AppStateService経由でも可）
                    appStateService.DocumentService = documentService;
                    Log.Information("Mainwinfow loaded");
                };
                Log.Information("Mainwinfow loadeding4");


            }
            catch (Exception ex) {
                MessageBox.Show($"[MainWindow] XAML Init Failed: {ex.Message}", "Fatal", MessageBoxButton.OK, MessageBoxImage.Error);
                Log.Error(ex, "[MainWindow] XAML Init Failed");
                throw;
            }
        }
    }
}
