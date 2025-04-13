using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveChartPlay.Models;
using LiveChartPlay.Services;
using MaterialDesignThemes.Wpf;
using Prism.Mvvm;
using Reactive.Bindings;


namespace LiveChartPlay.ViewModels
{
    public class WorkTimeResultViewModel : BindableBase
    {
        public ReactiveProperty<string> ResultText { get; } = new();
        public ObservableCollection<WorkTime> WorkHistory { get; }
        public ISnackbarMessageQueue SnackbarMessageQueue { get; }

        public WorkTimeResultViewModel(IMessengerService messenger, IAppStateService appStateService)
        {
            WorkHistory = appStateService.WorkHistory;


            messenger.Subscribe<int>(result =>
            {
                ResultText.Value = result.ToString();
            });
        }
    }
}
