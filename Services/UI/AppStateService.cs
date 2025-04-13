using LiveChartPlay.Models;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveChartPlay.Services
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
