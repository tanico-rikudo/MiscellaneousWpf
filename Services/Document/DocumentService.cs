using AvalonDock;
using AvalonDock.Layout;
using Serilog;
using System.Diagnostics;
using System.Windows.Controls;

namespace LiveChartPlay.Services
{
    public interface IDocumentService
    {
        void RegisterDocument<TView, TViewModel>(string key, string title)
           where TView : UserControl
            where TViewModel : class; 

        void ShowDocument(string key);

    }
    public class DocumentService: IDocumentService
    {
        private readonly IContainerProvider _container;
        // この DockingManager は MainWindow.xaml の UI にある名前付きコントロール DockManager です：
        // つまり、XAML 側で定義されている DockManager のインスタンスが必要なんですね。
        private readonly DockingManager _dockingManager;
        private readonly Dictionary<string, (Type ViewType, Type ViewModelType, string Title)> _documents = new();

        public DocumentService(IContainerProvider container, DockingManager dockingManager)
        { 
            _container = container;
            _dockingManager = dockingManager;
        }

        public void RegisterDocument<TView, TViewModel>(string key, string title)
            where TView : UserControl
            where TViewModel : class
        {
            _documents[key] = (typeof(TView),typeof(TViewModel),  title);
        }

        public void ShowDocument(string key)
        {

            //check in document service
            if (!_documents.TryGetValue(key, out var info))
            {
                Log.Warning($"[DocumentService] '{key}' not registered.");
                return;
            }
            var view = (UserControl)_container.Resolve(info.ViewType);

            // ViewModel を解決して DataContext にセット（あれば）
            if (info.ViewModelType != null)
            {
                var viewModel = _container.Resolve(info.ViewModelType);
                view.DataContext = viewModel;
            }

            var doc = new LayoutDocument
            {
                Title = info.Title,
                Content = view,
                IsSelected = true
            };

            var pane = _dockingManager.Layout.Descendents()
                .OfType<LayoutDocumentPane>()
                .FirstOrDefault();

            if (pane == null)
            {
                Log.Warning("[DocumentService] No LayoutDocumentPane found.");
                return;
            }

            Log.Information($"[DocumentService] add new doc: '{key}'");
            pane.Children.Add(doc);
            doc.IsSelected = true;
        }
    }

}
