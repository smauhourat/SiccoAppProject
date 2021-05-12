using SiccoApp.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SiccoApp.Services
{
    public interface IPresentationServices : IDisposable
    {

        Task CreateAsync(Presentation presentationToAdd);
        Task<List<String>> GetMailsAuditors(Presentation presentation);
        Task<List<String>> GetMailsContractors(int contractorID);
    }
}