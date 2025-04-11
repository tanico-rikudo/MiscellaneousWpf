using System.Windows;
using LiveChartPlay.Services;

using LiveChartPlay.ViewModels;
using MaterialDesignThemes.Wpf;
namespace LiveChartPlay.Views
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SnackbarMessageQueue _snackbarQueue = new SnackbarMessageQueue();

        public MainWindow(MainViewModel viewModel, IMessengerService messenger, IContainerProvider container, IAppStateService appStateService)
        {
            InitializeComponent();
            DataContext = viewModel;

            MySnackbar.MessageQueue = _snackbarQueue;

            // ViewModel や Service からのメッセージ通知を受け取り、Snackbar に流す
            // viewmodelを経由しない実装にしているので、ここに書いている
            messenger.Subscribe<string>(msg => _snackbarQueue.Enqueue(msg));

            // Prism のコンテナに手動登録
            //var documentService = new DocumentService(containerProvider, DockManager);
            //var registry = Prism.Ioc.ContainerLocator.Container.Resolve<IContainerRegistry>();
            //registry.RegisterInstance<IDocumentService>(documentService);

            var documentService = new DocumentService(container, DockManager);
            documentService.RegisterDocument<WorkTimeSummaryView , WorkTimeSummaryViewModel>("summary", "Summary View");
            documentService.RegisterDocument<WorkTimeChartView,    WorkTimeChartViewModel>("chart", "chart View");


            // ViewModel に渡す（例：AppStateService経由でも可）
            appStateService.DocumentService = documentService;

        }
    }
}
