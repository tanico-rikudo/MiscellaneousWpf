using Reactive.Bindings;

using LiveChartPlay.Models;
using LiveChartPlay.Services;

using Serilog;
using System.Reactive.Linq;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;

using MaterialDesignThemes.Wpf;
using LiveChartPlay.Views;

namespace LiveChartPlay.ViewModels
{
    public class WorkTimeCommandPanelViewModel: BindableBase
    {
        private WorkTime _workTime;

        public ReactiveCommand AddRandomRecordCommand { get; }
        public ReactiveCommand StartAutoGenerateCommand { get; }
        public ReactiveCommand StopAutoGenerateCommand { get; }

        public ReactiveCommand OpenSummaryWindowCommand { get; } 

        public ReactiveCommand OpenChartWindowCommand { get; }

        public ReactiveCommand LoadFromDatabaseCommand { get; }


        //複数の IDisposable（＝購読オブジェクト）をまとめて管理する入れ物
        //必要になったら.Dispose() で一括停止できる
        //ここでは「タイマー処理を停止したいときのために保持」している
        private CompositeDisposable _disposables = new();

        private readonly AutoGeneratorService _autoGenService;

        private readonly IWorkTimeCalculator _calculator;
        private readonly IMessengerService _messengerService;
        private readonly IViewHostService _viewHostService;
        private readonly IAppStateService _appStateService;
        private readonly IWorkTimeRepository _workTimeRepository;



        public WorkTimeCommandPanelViewModel(IWorkTimeCalculator calculator,
                             IMessengerService messengerService,
                             IAppStateService appStateService,
                             IViewHostService viewHostService,
                             IWorkTimeRepository workTimeRepository)
        {
            _workTime = new WorkTime(DateTime.Now, DateTime.Now, String.Empty);
            _messengerService = messengerService;
            _calculator = calculator;
            _viewHostService = viewHostService;
            _appStateService = appStateService;
            _workTimeRepository = workTimeRepository;

            //create random
            AddRandomRecordCommand = new ReactiveCommand();
            AddRandomRecordCommand.Subscribe(_ =>
            {
                var randomRecord = WorkTimeFactory.CreateRandom();
                _appStateService.WorkHistory.Add(randomRecord);
            });
            Log.Information("Loaded AddRandomRecordCommand");


            //auto gen
            _autoGenService = new AutoGeneratorService(_appStateService.WorkHistory, messengerService, calculator);
            StartAutoGenerateCommand = new ReactiveCommand();
            StartAutoGenerateCommand.Subscribe(_ => _autoGenService.Start());
            Log.Information("Loaded StartAutoGenerateCommand");


            // stop gen
            StopAutoGenerateCommand = new ReactiveCommand();
            StopAutoGenerateCommand.Subscribe(_ => _autoGenService.Stop());
            Log.Information("Loaded StopAutoGenerateCommand");


            // show summary view
            OpenSummaryWindowCommand = new ReactiveCommand();
            OpenSummaryWindowCommand.Subscribe(_ => {
                Log.Information("OpenSummaryWindowCommand executed!");
                var summaryViewModel = new WorkTimeSummaryViewModel(_messengerService);
                viewHostService.ShowInWindow<WorkTimeSummaryView>(summaryViewModel);
                appStateService.DocumentService?.ShowDocument("summary");
            });
            Log.Information("Loaded OpenSummaryWindowCommand");


            // show summary view
            OpenChartWindowCommand = new ReactiveCommand();
            OpenChartWindowCommand.Subscribe(_ => {
                Log.Information("OpenChartWindowCommand executed!");
                var chartViewModel = new WorkTimeChartViewModel(_appStateService);
                //viewHostService.ShowInWindow<WorkTimeChartView>(chartViewModel);
                appStateService.DocumentService?.ShowDocument("chart");
            }
            );
            Log.Information("Loaded OpenChartWindowCommand");

            LoadFromDatabaseCommand = new ReactiveCommand();
            LoadFromDatabaseCommand.Subscribe(async _ =>
            {
                try
                {
                    (await _workTimeRepository.GetWorkTimesAsync())
                     .ForEach(record => _appStateService.WorkHistory.Add(record));
                    //WorkHistory.Clear();
                    //foreach (var record in records)
                    //{
                    //    WorkHistory.Add(record);
                    //}
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "[MainViewModel] Failed to load data from database.");
                }
            });
            Log.Information("Loaded LoadFromDatabaseCommand");


        }

    }
}
