
using LiveChartPlay.Models;
namespace LiveChartPlay.Services.WorkTimeProcess
{
    public static  class WorkTimeFactory
    {
        private static readonly string[] _comments = { "A1", "B1", "C1", "D1", "E1", "F1" };
        public static WorkTime CreateRandom() {
            var rnd = new Random();
            var today = DateTime.Today;
            var start = today.AddHours(rnd.Next(6, 12)).AddMinutes(rnd.Next(0, 60));
            var end = start.AddHours(rnd.Next(6, 10)).AddMinutes(rnd.Next(0, 1000000));
            var comment = _comments[rnd.Next(_comments.Length)];

            return new WorkTime(start, end, comment);
        } 
    }   
}





