using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiccoApp.Persistence
{
    public interface ILocalizationRepository : IDisposable
    {
        List<Country> Countries();
        Task<List<Country>> CountriesAsync();

        List<State> States();
        List<State> States(int? countryId);
        Task<List<State>> StatesAsync();
    }
}
