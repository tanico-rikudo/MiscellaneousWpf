using Serilog;
using LiveChartPlay.Services.UI;
using LiveChartPlay.Services.Core;
using LiveChartPlay.Services.WorkTimeProcess;

using LiveChartPlay.ViewModels.WorkTimeProcess;


namespace LiveChartPlay.ViewModels.Main
{
    public class MainViewModel
    {
        public WorkTimeInputViewModel InputViewModel { get; }
        public WorkTimeCommandPanelViewModel CommandPanelViewModel { get; }
        public WorkTimeResultViewModel ResultViewModel { get; }


        public MainViewModel(
            IWorkTimeCalculator calculator,
            IMessengerService messengerService,
            IAppStateService appStateService,
            IViewHostService viewHostService,
            IWorkTimeRepository workTimeRepository
            )
        {
            ResultViewModel = new WorkTimeResultViewModel(messengerService,
                appStateService);
            InputViewModel = new WorkTimeInputViewModel(calculator,
                messengerService,
                appStateService);
            CommandPanelViewModel = new WorkTimeCommandPanelViewModel(calculator,
                messengerService,
                appStateService,
                viewHostService,
                workTimeRepository
                );
            Log.Information("Generated main view model chain");

        }
    }
}