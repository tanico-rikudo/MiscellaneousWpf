using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveChartPlay.Interfaces
{
    public interface IBusyNotifier
    {
        ReactiveProperty<bool> IsBusy { get; }
    }
}
