using SkiaSharp;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

using LiveChartPlay.Models;
using LiveChartPlay.Services;
using LiveChartPlay.Attributes;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore.SkiaSharpView.Extensions;

using Serilog;
using System.Reactive.Linq;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;

using MaterialDesignThemes.Wpf;
using LiveChartPlay.Views;
using System.Xml.Linq;



namespace LiveChartPlay.ViewModels
{
    public class MainViewModel
    {
        public WorkTimeInputViewModel InputViewModel { get; }
        public WorkTimeCommandPanelViewModel CommandPanelViewModel { get; }
        public WorkTimeResultViewModel ResultViewModel { get; }

        public IDocumentService DocumentService { get; set; }

        public MainViewModel(
            IWorkTimeCalculator calculator,
            IMessengerService messengerService,
            IAppStateService appStateService,
            IViewHostService viewHostService,
            IDocumentService documentService,
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

        }
    }
}