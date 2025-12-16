using Hangfire;
using LogisticsPlatform.Application.Jobs;
using Microsoft.Extensions.Hosting;


namespace LogisticsPlatform.Application.BackgroundJobs
{
    public class HangfireStartupJob : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            RecurringJob.AddOrUpdate<EtaMonitoringJob>(
                "eta-monitoring",
                j => j.ExecuteAsync(),
                Cron.MinuteInterval(5));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}
