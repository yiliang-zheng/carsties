using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Domain.Events;

namespace WebApi.Consumer;

public class AuctionCreatedFaultConsumer : IConsumer<Fault<AuctionCreated>>
{
    private readonly ILogger<AuctionCreatedFaultConsumer> _logger;

    public AuctionCreatedFaultConsumer(ILogger<AuctionCreatedFaultConsumer> logger)
    {
        _logger = logger;
    }
    public Task Consume(ConsumeContext<Fault<AuctionCreated>> context)
    {
        this._logger.LogInformation("--> Consuming faulty auction created message");


        var exception = context.Message.Exceptions.FirstOrDefault();
        this._logger.LogError(exception?.Message, context.Message.Message);
        return Task.CompletedTask;
        //if (exception is not null && exception.ExceptionType.Equals(typeof(ArgumentException).FullName))
        //{
        //    context.Message.Message.Model = "FooBar";
        //    await context.Publish(context.Message.Message);
        //}
        //else
        //{
        //    Console.WriteLine("Not an argument exception - update error dashboard somewhere");
        //}


    }
}