using LiveChartPlay.Models;
using LiveChartPlay.Services;
using MahApps.Metro.Controls;
using Serilog;
using System.Windows;
using LiveChartPlay.Views.Auth;
using LiveChartPlay.Views.Controls;
using LiveChartPlay.Views.Main;
using LiveChartPlay.Views.WorkTimeProcess;

using LiveChartPlay.Services.Auth;
using LiveChartPlay.Services.UI;
using LiveChartPlay.Services.Core;
using LiveChartPlay.Services.Document;
using LiveChartPlay.Services.User;
using LiveChartPlay.Services.WorkTimeProcess;

using LiveChartPlay.ViewModels.Auth;
using LiveChartPlay.ViewModels.Main;
using LiveChartPlay.ViewModels.WorkTimeProcess;


namespace LiveChartPlay.Views.Auth
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
