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

            RecurringJob.AddOrUpdate<NotificationCleanupJob>(
                "notification-cleanup",
                j => j.ExecuteAsync(),
                Cron.Hourly);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}
