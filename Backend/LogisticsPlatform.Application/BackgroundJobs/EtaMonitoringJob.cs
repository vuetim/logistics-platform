using LogisticsPlatform.Application.Interfaces.Repositories.Loads;
using LogisticsPlatform.Application.Interfaces.Services.Loads;

namespace LogisticsPlatform.Application.Jobs;



    public class EtaMonitoringJob
    {
        private readonly ILoadStopRepository _stopRepo;
        private readonly IEtaPredictionService _eta;

        public EtaMonitoringJob(
            ILoadStopRepository stopRepo,
            IEtaPredictionService eta)
        {
            _stopRepo = stopRepo;
            _eta = eta;
        }

        public async Task ExecuteAsync()
        {
            var enRouteStops =
                await _stopRepo.GetEnRouteStopsWithLoadAsync();

            foreach (var stop in enRouteStops)
            {
                await _eta.EvaluateAsync(stop);
            }
        }
    
}
