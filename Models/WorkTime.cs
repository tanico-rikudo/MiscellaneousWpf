using Prism.Mvvm;
using Serilog;

namespace LiveChartPlay.Models
{
    public class WorkTime: BindableBase
    {
        private DateTime _startDatetime;

        /// <summary>
        /// 作業を実施した日付を設定、または取得します。
        /// </summary>
        public DateTime StartDatetime
        {
            get => _startDatetime;
            // OnPropertyChanged : already implemnted
            set => SetProperty(ref _startDatetime, value);
        }

        private DateTime _endDatetime;

        /// <summary>
        /// 作業を実施した日付を設定、または取得します。
        /// </summary>
        public DateTime EndDatetime
        {
            get => _endDatetime;
            // OnPropertyChanged : already implemnted
            set => SetProperty(ref _endDatetime, value);
        }

        private int _workingMinutes;

        /// <summary>
        /// 実作業時間を設定、または取得します。
        /// 単位は"秒"です。
        /// </summary>
        public int WorkingMinutes
        {
            get => _workingMinutes;
            set => SetProperty(ref _workingMinutes, value);
        }

        private string? _comment;

        /// <summary>
        /// 実施した作業内容を設定、または取得します。
        /// </summary>
        public string? Comment
        {
            get => _comment;
            set => SetProperty(ref _comment, value);
        }

        public WorkTime(DateTime startDatetime, DateTime endDatetime, String comment)
        {
            StartDatetime = startDatetime;
            EndDatetime = endDatetime;
            Comment = comment;
            WorkingMinutes = 0;
        }

    }
}
