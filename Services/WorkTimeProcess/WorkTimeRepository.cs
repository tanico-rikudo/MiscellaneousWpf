using LiveChartPlay.Models;
using Microsoft.Extensions.Configuration;
using UserControl = System.Windows.Controls.UserControl;
using LiveChartPlay.Services.Core;

namespace LiveChartPlay.Services.WorkTimeProcess
{
    public interface IWorkTimeRepository
    {
        Task<List<WorkTime>> GetWorkTimesAsync();
    }

    public class WorkTimeRepository : DatabaseServiceBase, IWorkTimeRepository
    {

        public WorkTimeRepository(IConfiguration configuration, IMessengerService messenger)
            : base(configuration.GetConnectionString("Default"), messenger)
        {
        }

        public async Task<List<WorkTime>> GetWorkTimesAsync()
        {
            return await ExecuteReaderAsync(
                "SELECT start_time, end_time, comment FROM work_times",
                reader =>
                {
                    var start = reader.GetDateTime(0);
                    var end = reader.GetDateTime(1);
                    var comment = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);

                    return new WorkTime(start, end, comment)
                    {
                        WorkingMinutes = (int)(end - start).TotalMinutes
                    };
                });

        }
    }
}
