using LogisticsPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.Interfaces.Services.Loads
{
    public interface ILoadAlertService
    {
        Task HandleEtaDelayAsync(LoadStop stop);

    }

}
