using MassTransit;
using Shared.Domain.Messages;
using Shared.Domain.Response;

namespace SagaOrchestration
{
    public class Worker(ILogger<Worker> logger, IServiceProvider serviceProvider) : BackgroundService
    {
        /// <summary>
        /// Running every 5 seconds to check finish auction
        /// publish <see cref="MarkAuctionFinishMessage"/> event to trigger FinishAuctionStateMachine Saga
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var scope = serviceProvider.CreateScope();
            var requestClient = scope.ServiceProvider.GetRequiredService<IRequestClient<CheckAuctionFinish>>();
            var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

            stoppingToken.Register(() => logger.LogInformation("==> Auction check is stopping"));

            while (!stoppingToken.IsCancellationRequested)
            {
                var response = (await requestClient.GetResponse<CheckAuctionFinishResponse>(new CheckAuctionFinish(), stoppingToken)).Message;
                foreach (var auctionId in response.Auctions)
                {

                    await publishEndpoint.Publish(new MarkAuctionFinishMessage(auctionId), stoppingToken);
                }

                await Task.Delay(1000 * 60, stoppingToken);
            }
        }
    }
}
