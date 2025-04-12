using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveChartPlay.Models
{
    [Flags]
    public enum UserRole
    {
        None = 0,
        Viewer = 1 << 0,
        Editor = 1 << 1,
        Admin = 1 << 2,
        Super = 1 << 3
    }
}
