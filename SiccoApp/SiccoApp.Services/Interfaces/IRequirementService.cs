using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiccoApp.Services
{
    public interface IRequirementService
    {
        Task GenerateContractorAllRequirements();
    }
}
