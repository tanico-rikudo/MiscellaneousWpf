using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveChartPlay.Services;
using Prism.Mvvm;
using Reactive.Bindings;

namespace LiveChartPlay.ViewModels
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
