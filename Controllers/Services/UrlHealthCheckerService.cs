using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace UrlHealthMonitor.Services
{
    public class UrlHealthCheckerService : BackgroundService
    {
        private readonly ILogger<UrlHealthCheckerService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public UrlHealthCheckerService(
            ILogger<UrlHealthCheckerService> logger,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("UrlHealthCheckerService started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var processor = scope.ServiceProvider.GetRequiredService<IUrlHealthProcessor>();
                        await processor.ProcessAsync(stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while processing URL health checks.");
                }

                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }

            _logger.LogInformation("UrlHealthCheckerService stopping.");
        }
    }
}