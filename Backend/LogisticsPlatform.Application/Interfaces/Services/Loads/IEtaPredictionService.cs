using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.Interfaces.Services.Loads
{
    using LogisticsPlatform.Domain.Entities;

    public interface IEtaPredictionService
    {
        Task<EtaEvaluationResult?> EvaluateAsync(LoadStop stop);
    }

}
