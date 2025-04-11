using LiveChartPlay.Models;
using LiveChartPlay.Services;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Prism.Mvvm;
using Reactive.Bindings;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace LiveChartPlay.ViewModels
{
    public class WorkTimeChartViewModel : BindableBase
    {
        public ObservableCollection<ISeries> Series { get; } = new();
        public ReactiveProperty<Axis[]> XAxes { get; } = new();
        public ReactiveProperty<Axis[]> YAxes { get; } = new();

        private readonly ObservableCollection<WorkTime> _workHistory;

        public WorkTimeChartViewModel(IAppStateService appStateService)
        {
            _workHistory = appStateService.WorkHistory;

            UpdateChart(_workHistory);

            _workHistory.CollectionChanged += (_, e) =>
            {
                UpdateChart(_workHistory);
            };
        }

        private void UpdateChart(IEnumerable<WorkTime> history)
        {
            if (!history.Any()) return;

            var grouped = history
                .GroupBy(x => x.EndDatetime.Date)
                .OrderBy(g => g.Key)
                .ToList();

            var labels = grouped.Select(g => g.Key.ToString("MM/dd")).ToArray();
            var values = grouped.Select(g => g.Sum(x => x.WorkingMinutes)).ToArray();

            Series.Clear();
            Series.Add(new ColumnSeries<int> { Values = values });


            XAxes.Value = new[]
            {
                new Axis
                {
                    Name = "Date",  
                    Labels = labels,
                    LabelsRotation = 15,
                    Labeler = (value) => value.ToString("C")
                }
            };

            YAxes.Value = new[]
            {
                new Axis
                {
                    Name = "mins",
                    MinLimit = 0
                }
            };

            Log.Information("[Chart] Updated with {0} days", labels.Length);
        }
    }
}