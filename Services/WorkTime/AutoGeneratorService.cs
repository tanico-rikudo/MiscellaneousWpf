using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using LiveChartPlay.Models;
using MaterialDesignThemes.Wpf;
using Reactive.Bindings.Extensions;
using Serilog;

namespace LiveChartPlay.Services
{
    public class AutoGeneratorService
    {
        private readonly ObservableCollection<WorkTime> _history;
        private readonly IMessengerService _messengerService;
        private readonly IWorkTimeCalculator _calculator;
        private CompositeDisposable _disposables = new();

        public AutoGeneratorService(ObservableCollection<WorkTime> history, IMessengerService messengerService, IWorkTimeCalculator calculator)
        {
            _history = history;
            _messengerService = messengerService;
            _calculator = calculator;
        }

        public void Start()
        {
            _messengerService.Publish("Start Auto adding...");

            var subscription = Observable.Interval(TimeSpan.FromSeconds(1))
                .ObserveOnUIDispatcher()
                .Subscribe(_ =>
                {
                    Log.Information("[DEBUG] add 3 cases");
                    for (int i = 0; i < 3; i++)
                    {
                        WorkTime randomWorkTime = WorkTimeFactory.CreateRandom();
                        int result = _calculator.CalculateMinutes(randomWorkTime);
                        randomWorkTime.WorkingMinutes = result;
                        _messengerService.Publish(result);
                        Log.Information($"[DEBUG] Publish {randomWorkTime}");
                        _history.Add(randomWorkTime);
                    }
                },
                ex => Log.Error($"[ERROR] Auto generation failed: {ex}"));

            _disposables.Add(subscription);
        }

        public void Stop()
        {
            _disposables.Dispose();
            _disposables = new CompositeDisposable();
        }
    }
}
