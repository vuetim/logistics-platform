using LogisticsPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.Interfaces.Services
{
    public interface ILoadFinancialAutomationService
    {
        Task GenerateFinancialDocumentsAsync(Load load);
    }

}
