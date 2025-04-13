using LiveChartPlay.Models;
using LiveChartPlay.Services;
using MahApps.Metro.Controls;
using Serilog;
using System.Windows;

namespace LiveChartPlay.Views
{
    /// <summary>
    /// LoginWIndow.xaml の相互作用ロジック
    /// </summary>
    public partial class LoginWindow : MetroWindow
    {

        public LoginWindow(IAppStateService appState)
        {
            InitializeComponent();
        }


    }
}
