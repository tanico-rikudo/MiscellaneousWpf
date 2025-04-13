using System.Collections.ObjectModel;
using LiveChartPlay.Models;
using MaterialDesignThemes.Wpf;
using Reactive.Bindings;
using LiveChartPlay.Services.UI;
using LiveChartPlay.Services.Core;

namespace LiveChartPlay.ViewModels.WorkTimeProcess
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
