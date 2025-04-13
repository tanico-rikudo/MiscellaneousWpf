using System.Windows.Controls;

namespace LiveChartPlay.Views
{
    /// <summary>
    /// WorkTimeCommandPanelView.xaml の相互作用ロジック
    /// </summary>
    public partial class WorkTimeCommandPanelView : UserControl
    {
        public WorkTimeCommandPanelView()
        {
            InitializeComponent();
            // ViewModel を直接バインドする場合は、コードビハインドではなく Prism の DI によって DataContext を自動でセットすることが推奨されます。
            //            つまり：ここで DataContext = new WorkTimeInputViewModel(); のようなことはやらなくてよいです！
            // Mainviewmodelで解決済み
            // DataContext = viewModel; 
        }
    }
}
