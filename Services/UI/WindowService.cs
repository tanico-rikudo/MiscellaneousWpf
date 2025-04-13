using System.Windows;

using LiveChartPlay.ViewModels.Auth;


namespace LiveChartPlay.Services.UI
{
    public interface IWindowService
    {
        void ShowWindow<T>() where T : Window;
        bool? ShowDialogWindow<T>() where T : Window;

        bool? ShowDialogWindow<TWindow, TViewModel>(TViewModel viewModel) where TWindow : Window;
    }

    public class WindowService : IWindowService
    {
        private readonly IContainerProvider _container;

        public WindowService(IContainerProvider container)
        {
            _container = container; 
        }

        public void ShowWindow<T>() where T : Window
        {
            var window = _container.Resolve<T>();
            window.Show();
        }

        public bool? ShowDialogWindow<T>() where T : Window
        {
            var window = _container.Resolve<T>();
            return window.ShowDialog();
        }

        public bool? ShowDialogWindow<TWindow, TViewModel>(TViewModel viewModel) where TWindow : Window
        {
            var window = _container.Resolve<TWindow>();
            window.DataContext = viewModel;

            // ViewModelが閉じたいときのためにイベント登録しておくとスマート
            if (viewModel is IClosable closable)
            {
                closable.CloseRequested += () => {
                    window.Dispatcher.BeginInvoke(() =>
                    {
                        try
                        {
                            if (window.IsVisible && window.IsLoaded)
                            {
                                window.DialogResult = true; // 安全に設定
                            }
                            else
                            {
                                // fallback
                                window.Close();
                            }
                        }
                        catch (InvalidOperationException ex)
                        {
                            Serilog.Log.Error(ex, "DialogResult set failed.");
                            window.Close(); // フォールバック
                        }
                    });
                };
            }
            return window.ShowDialog();
        }
    }
}
