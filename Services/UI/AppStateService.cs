using LiveChartPlay.Models;
using System.Collections.ObjectModel;
using LiveChartPlay.Services.Document;


namespace LiveChartPlay.Services.UI
{

    public interface IAppStateService 
    { 
        ObservableCollection<WorkTime> WorkHistory { get; }
        IDocumentService? DocumentService { get; set; }
        //snack (pop up raised from bottom of screen )

        public UserInfo? CurrentUser { get; set; }

        public bool IsAuthenticated { get;  }

    }

    public class AppStateService: IAppStateService
    {
        public ObservableCollection<WorkTime> WorkHistory { get; } = new();
        public IDocumentService? DocumentService { get; set; } 

        public UserInfo? CurrentUser { get; set; }

        public bool IsAuthenticated => CurrentUser != null;

    }
}
