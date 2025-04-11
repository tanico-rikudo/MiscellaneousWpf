using LiveChartPlay.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace LiveChartPlay.Services
{
    public interface IWindowService
    {
        void ShowWindow<T>() where T : Window;
    }

    public class WindowService : IWindowService
    {
        private readonly IContainerProvider _container;

        public WindowService(IContainerProvider container)
        {
            _container = container; 
        }

        public void ShowWindow<T>() where T : Window
        {
            var window = _container.Resolve<T>();
            window.Show();
        }
    }
}
