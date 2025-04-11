using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LiveChartPlay.Services
{
    public interface  IViewHostService
    {
        void ShowInWindow<TView>(object viewModel) where TView : UserControl;
        void ShowinDock<TView>(object viewModel) where TView : UserControl;
    }

    public class ViewHostService : IViewHostService
    {
        public void ShowInWindow<TView>(object viewModel) where TView : UserControl
        {
            // ジェネリックで TView しか型がわからない場合（TView : UserControl）は、名前が分からないので new TView() が使えません。
            // ふつうはこう　var view = new SummaryView();
            // 型だけからインスタンスを作って画面に出すことができるん
            // です。
            // もし DI（依存注入）でコンストラクターに引数が必要な場合は、Activator.CreateInstance ではなく DI コンテナで解決する必要があります。
            var view = Activator.CreateInstance(typeof(TView)) as UserControl;
            if (view == null) return;

            view.DataContext = viewModel;

            var window = new Window
            {
                Title = typeof(TView).Name,
                Content = view,
                Width = 400,
                Height = 300
            };

            window.Show();
        }

        public void ShowinDock<TView>(object viewModel) where TView : UserControl
        {
            throw new NotImplementedException();
        }
    }

}
// 