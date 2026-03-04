using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UrlHealthMonitor.Data;
using UrlHealthMonitor.Models;
using UrlHealthMonitor.Services;
using Xunit;

namespace UrlHealthMonitor.Tests
{
    // Fake email service used only for testing
    public class FakeEmailService : IEmailService
    {
        public Task SendEmailAsync(
            string toEmails,
            string subject,
            string body,
            byte[]? attachmentBytes = null,
            string? attachmentName = null)
        {
            return Task.CompletedTask;
        }

        public Task SendDownAlertAsync(MonitoredUrl url)
        {
            return Task.CompletedTask;
        }
    }

    public class UrlHealthProcessorTests
    {
        [Fact]
        public async Task ProcessAsync_ShouldRunWithoutErrors()
        {
            var services = new ServiceCollection();

            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("TestDb"));

            services.AddLogging();

            services.AddHttpClient();

            // Use fake email service instead of real one
            services.AddScoped<IEmailService, FakeEmailService>();

            services.AddScoped<IUrlHealthProcessor, UrlHealthProcessor>();

            var provider = services.BuildServiceProvider();

            using var scope = provider.CreateScope();
            var processor = scope.ServiceProvider.GetRequiredService<IUrlHealthProcessor>();

            var exception = await Record.ExceptionAsync(() =>
                processor.ProcessAsync(CancellationToken.None));

            Assert.Null(exception);
        }
    }
}