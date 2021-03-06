using RabbitMQ.Client;
using TingTuffer.Examples.WorkerStatic.Service;

namespace TingTuffer.Examples.WorkerStatic
{
    public class WorkerWithoutRingBuffer : BackgroundService
    {
        private readonly ILogger<WorkerWithoutRingBuffer> _logger;

        public WorkerWithoutRingBuffer(ILogger<WorkerWithoutRingBuffer> logger)
            => _logger = logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var task = Task.Run(() =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    using var model = new ConnectionFactory()
                      .CreateConnection()
                      .CreateModel();

                    _logger.LogInformation(ServicePublisher.Publish(model, EnumQueue.WithoutRingBuffer));
                }
            }, stoppingToken);

            await task;
        }
    }
}