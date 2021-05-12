using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public interface IPeriodRepository : IDisposable
    {
        Task<List<Period>> FindPeriodsAsync();
    }
}
