using LiveChartPlay.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;
using LiveChartPlay.Services;
using Microsoft.Extensions.Configuration;

namespace LiveChartPlay.Services
{
    public interface IWorkTimeRepository
    {
        Task<List<WorkTime>> GetWorkTimesAsync();
    }

    public class WorkTimeRepository : DatabaseServiceBase, IWorkTimeRepository
    {
        private readonly IMessengerService _messenger;

        public WorkTimeRepository(IConfiguration configuration, IMessengerService messenger)
            : base(configuration.GetConnectionString("Default"), messenger)
        {
            _messenger = messenger;
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
