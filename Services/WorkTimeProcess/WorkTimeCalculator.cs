using LiveChartPlay.Models;

namespace LiveChartPlay.Services.WorkTimeProcess
{
    public interface IWorkTimeCalculator
    {
        int CalculateMinutes(WorkTime workTime);
        string GetRandomComment();
    }
    public class WorkTimeCalculator: IWorkTimeCalculator
    {

        private static List<string> RandomWords = new()
        {
            "apple", "banana", "coffee", "dream", "engine", "forest", "galaxy", "hero", "ice", "jungle",
            "keyboard", "lemon", "magic", "night", "orange", "pixel", "quest", "river", "star", "tiger"
        };
        private static readonly Random Random = new();

        public int CalculateMinutes(WorkTime workTime) { 
            if (workTime == null) throw new ArgumentNullException(nameof(workTime));
            var minutes = (int)(workTime.EndDatetime - workTime.StartDatetime).TotalMinutes;
            return minutes < 0 ? 0 : minutes;
        }

        public string GetRandomComment()
        {
            // generate randomw strings
            return RandomWords[Random.Next(RandomWords.Count)];

        }
    }
}
