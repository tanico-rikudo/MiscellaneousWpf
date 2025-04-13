using Reactive.Bindings;
using LiveChartPlay.Services.Core;

namespace LiveChartPlay.ViewModels.WorkTimeProcess
{
    public class WorkTimeSummaryViewModel: BindableBase
    {
        public ReactiveProperty<string> SummaryText { get; }

        public WorkTimeSummaryViewModel(IMessengerService messenger)
        {
            SummaryText = new ReactiveProperty<string>("Nothing coming...");

            messenger.Subscribe<int>(minutes =>
            {
                SummaryText.Value = $"Total {minutes} minutes s of now";
            });
        }
    }
}
