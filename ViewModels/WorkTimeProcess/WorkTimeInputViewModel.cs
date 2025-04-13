

using SkiaSharp;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

using LiveChartPlay.Models;
using LiveChartPlay.Attributes;
using Serilog;
using System.Reactive.Linq;
using System.Reactive.Disposables;

using MaterialDesignThemes.Wpf;

using LiveChartPlay.Services.UI;
using LiveChartPlay.Services.Core;
using LiveChartPlay.Services.WorkTimeProcess;

namespace LiveChartPlay.ViewModels.WorkTimeProcess
{
    public class WorkTimeInputViewModel: BindableBase
    {
        private WorkTime _workTime;

        // ReactiveProperty  implement setter

        [DatetimeValidation("yyyy/MM/dd HH:mm:ss")]
        public ReactiveProperty<String> StartDateTime { get; }

        [DatetimeValidation("yyyy/MM/dd HH:mm:ss")]
        public ReactiveProperty<String> EndDateTime { get; }

        public ReactiveProperty<String?> Comment { get; }

        public ReactiveProperty<string> ResultText { get; } = new ReactiveProperty<string>();

        // Reactive commands
        public ReactiveCommand SendCalculateWorkTime { get; }

        //複数の IDisposable（＝購読オブジェクト）をまとめて管理する入れ物
        //必要になったら.Dispose() で一括停止できる
        //ここでは「タイマー処理を停止したいときのために保持」している
        private CompositeDisposable _disposables = new();

        private readonly IWorkTimeCalculator _calculator;
        private readonly IMessengerService _messenger;
        private readonly IAppStateService _appStateService;


        //snack (pop up raised from bottom of screen )
        public ISnackbarMessageQueue SnackbarMessageQueue { get; } = new SnackbarMessageQueue();


        public WorkTimeInputViewModel(IWorkTimeCalculator calculator,
                             IMessengerService messenger,
                             IAppStateService appStateService)
        {
            Random random = new Random();

            _workTime = new WorkTime(DateTime.Now, DateTime.Now.AddMinutes(random.Next(1,2000)), String.Empty);
            _messenger = messenger;
            _calculator = calculator;
            _appStateService = appStateService;


            StartDateTime = _workTime.ToReactivePropertyAsSynchronized(
                m => m.StartDatetime,
                x => x.ToString(),
                s => DateTime.Parse(s),
                ReactivePropertyMode.DistinctUntilChanged | ReactivePropertyMode.RaiseLatestValueOnSubscribe,
                true).SetValidateAttribute(() => StartDateTime);
            EndDateTime = _workTime.ToReactivePropertyAsSynchronized(
                m => m.EndDatetime,
                x => x.ToString(),
                s => DateTime.Parse(s),
                ReactivePropertyMode.DistinctUntilChanged | ReactivePropertyMode.RaiseLatestValueOnSubscribe,
                true).SetValidateAttribute(() => EndDateTime);
            Comment = _workTime.ToReactivePropertyAsSynchronized(
                m => m.Comment,
                //x => x,
                //s => s,
                ReactivePropertyMode.DistinctUntilChanged | ReactivePropertyMode.RaiseLatestValueOnSubscribe,
                true).SetValidateAttribute(() => Comment);


            // エラーログを監視（パターン 1）
            StartDateTime.ObserveHasErrors.Subscribe(hasError =>
            {
                Log.Information($"[DEBUG] StartDateTime Has Error: {hasError}");
            });
            EndDateTime.ObserveHasErrors.Subscribe(hasError =>
            {
                Log.Information($"[DEBUG] EndDateTime Has Error: {hasError}");
            });



            // Check avilable: ReactiveCommand 
            SendCalculateWorkTime = StartDateTime.ObserveHasErrors.CombineLatest(
                    EndDateTime.ObserveHasErrors, (x, y) => (!x && !y))
                    .ToReactiveCommand();
            Log.Information(string.Format("Can execute {0}", SendCalculateWorkTime.CanExecute()));

            // Generate command 
            SendCalculateWorkTime.Subscribe(
                _ =>
                {

                    int result = _calculator.CalculateMinutes(_workTime);

                    Log.Information($"[DEBUG] Calculated Work Time: {result}");

                    // Messenger を通じて発信
                    _messenger.Publish(result);
                    Log.Information($"[DEBUG] Publish Time: {result}");

                    // 初期化子
                    string fixed_comment = String.IsNullOrWhiteSpace(_workTime.Comment) ? _calculator.GetRandomComment() : _workTime.Comment;
                    WorkTime wt = new WorkTime(_workTime.StartDatetime,
                                               _workTime.EndDatetime,
                                               fixed_comment)
                    {
                        WorkingMinutes = result
                    };

                    // add hist
                    _appStateService.WorkHistory.Add(wt);

                });

            // Messenger から結果を受け取って ResultText に設定
            //_messenger.Subscribe<int>(result =>
            //{
            //    Log.Information($"[DEBUG] ResultText Updated: {ResultText.Value}");
            //    ResultText.Value = result.ToString();
            //});
        }

    }
}