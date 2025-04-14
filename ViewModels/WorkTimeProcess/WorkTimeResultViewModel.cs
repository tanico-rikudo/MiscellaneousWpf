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

        public ReactiveCommand ExportXlsxCommand { get; }
        public ReactiveCommand ExportPdfCommand { get; }
        public ReactiveCommand ShowPreviewCommand { get; }

        public ReactiveProperty<string> ResultText { get; } = new();
        public ObservableCollection<WorkTime> WorkHistory { get; }
        public ISnackbarMessageQueue SnackbarMessageQueue { get; }

        public WorkTimeResultViewModel(IMessengerService messenger,
                                        IAppStateService appStateService,
                                        IExportService exportService)
        {
            WorkHistory = appStateService.WorkHistory;


            messenger.Subscribe<int>(result =>
            {
                ResultText.Value = result.ToString();
            });


            ExportXlsxCommand = new ReactiveCommand();
            ExportXlsxCommand.Subscribe(_ =>
            {
                var file = ShowSaveDialog("Excelファイル|*.xlsx");
                if (file != null)
                    exportService?.ExportTo(ExportType.XLSX, file);
            });

            ExportPdfCommand = new ReactiveCommand();
            ExportPdfCommand.Subscribe(_ =>
            {
                var file = ShowSaveDialog("PDFファイル|*.pdf");
                if (file != null)
                    exportService?.ExportTo(ExportType.PDF, file);
            });

            ShowPreviewCommand = new ReactiveCommand();
            ShowPreviewCommand.Subscribe(_ => exportService?.ShowPreview());
        }


        private string? ShowSaveDialog(string filter)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = filter,
                RestoreDirectory = true
            };
            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }





    }
}
