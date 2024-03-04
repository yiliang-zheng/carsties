using MassTransit;
using SagaOrchestration.StateInstances;
using Shared.Domain.Events;
using Shared.Domain.Messages;

namespace SagaOrchestration.StateMachines;

public class FinishAuctionStateMachine : MassTransitStateMachine<FinishAuctionStateInstance>
{
    private readonly Serilog.ILogger _logger;

    //Commands
    private Event<MarkAuctionFinishMessage> MarkAuctionFinishMessage { get; set; }

    //Events
    public Event<AuctionFinished> AuctionFinishedEvent { get; set; }

    public Event<AuctionFinishFailed> AuctionFinishFailedEvent { get; set; }

    public Event<SearchMarkFinished> SearchMarkFinishedEvent { get; set; }

    public Event<SearchMarkFinishedFailed> SearchMarkFinishedFailedEvent { get; set; }

    //States
    public State AuctionFinishRequestSent { get; set; }

    public State AuctionFinishMarked { get; set; }

    public State AuctionFinishMarkedFailed { get; set; }

    public State SearchMarkFinished { get; set; }

    public State SearchMarkFinishedFailed { get; set; }


    public FinishAuctionStateMachine()
    {
        _logger = Serilog.Log.Logger;
        InstanceState(p => p.CurrentState);
        Event(
            () => MarkAuctionFinishMessage,
            p => p.CorrelateBy<Guid>(
                    x => x.AuctionId,
                    z => z.Message.AuctionId)
                .SelectId(context => NewId.NextGuid())
        );

        Event(
            () => AuctionFinishedEvent,
            p => p.CorrelateById(context => context.Message.CorrelationId));

        Event(
            () => AuctionFinishFailedEvent,
            p => p.CorrelateById(context => context.Message.CorrelationId)
        );

        Event(
            () => SearchMarkFinishedEvent,
            p => p.CorrelateById(context => context.Message.CorrelationId)
        );

        Event(
            () => SearchMarkFinishedFailedEvent,
            p => p.CorrelateById(context => context.Message.CorrelationId)
        );

        Initially(
            When(MarkAuctionFinishMessage)
                .Then(
                    context =>
                    {
                        _logger.ForContext("CorrelationId", context.Saga.CorrelationId)
                            .Information(
                            "MarkAuctionFinishMessage received in FinishAuctionStateMachine: {ContextSaga} ",
                            context.Saga);
                    })
                .Then(
                    context =>
                    {
                        context.Saga.AuctionId = context.Message.AuctionId;
                        context.Saga.CreatedDate = DateTimeOffset.UtcNow;
                    })
                .Send(new Uri("queue:mark-auction-finish"), context => new FinishAuctionMessage
                {
                    CorrelationId = context.Saga.CorrelationId,
                    AuctionId = context.Saga.AuctionId,
                    Amount = context.Message.Amount,
                    ItemSold = context.Message.ItemSold,
                    Seller = context.Message.Seller,
                    Winner = context.Message.Winner
                })
                .Then(context =>
                {
                    this._logger
                        .ForContext("CorrelationId", context.Saga.CorrelationId)
                        .Information(
                        "FinishAuction message sent in FinishAuctionStateMachine: {ContextSaga}",
                        context.Saga);
                })
                .TransitionTo(AuctionFinishRequestSent)
        );

        During(AuctionFinishRequestSent,
            When(AuctionFinishedEvent)
                .Then(context =>
                {
                    _logger
                        .ForContext("CorrelationId", context.Saga.CorrelationId)
                        .Information(
                        "AuctionFinishedEvent received in FinishAuctionStateMachine: {ContextSaga} ",
                        context.Saga);
                })
                .TransitionTo(AuctionFinishMarked)
                .Then(context =>
                {
                    this._logger.ForContext("CorrelationId", context.Saga.CorrelationId)
                        .Information(
                        "Transit state into {SagaState}",
                        context.Saga.CurrentState
                    );
                })
                .Send(
                    new Uri("queue:mark-search-auction-finish"),
                    context => new MarkSearchFinishMessage
                    {
                        CorrelationId = context.Saga.CorrelationId,
                        AuctionId = context.Message.AuctionId,
                        Winner = context.Message.Winner,
                        Status = context.Message.Status,
                        SoldAmount = context.Message.SoldAmount
                    }),
            When(AuctionFinishFailedEvent)
                .Then(context =>
                {
                    this._logger
                        .ForContext("CorrelationId", context.Saga.CorrelationId)
                        .Information(
                            "{Event} received in FinishAuctionStateMachine: {ContextSaga} ",
                            nameof(AuctionFinishFailedEvent),
                            context.Saga);
                })
                .TransitionTo(AuctionFinishMarkedFailed)
        );

        During(AuctionFinishMarked,
            When(SearchMarkFinishedEvent)
                .Then(context =>
                {
                    this._logger
                        .ForContext("CorrelationId", context.Saga.CorrelationId)
                        .Information(
                        "{Event} received in FinishAuctionStateMachine: {ContextSaga} ",
                        nameof(SearchMarkFinishedEvent),
                        context.Saga);
                })
                .TransitionTo(SearchMarkFinished)
                .Then(context =>
                {
                    this._logger.ForContext("CorrelationId", context.Saga.CorrelationId)
                        .Information(
                            "Transit state into {SagaState}",
                            context.Saga.CurrentState);
                })
                .Finalize(),
            When(SearchMarkFinishedFailedEvent)
                .Then(context =>
                {
                    this._logger.ForContext("CorrelationId", context.Saga.CorrelationId)
                        .Information(
                            "{Event} received in FinishAuctionStateMachine: {ContextSaga} ",
                            nameof(SearchMarkFinishedFailedEvent),
                            context.Saga);
                })
                .TransitionTo(SearchMarkFinishedFailed)
        );

    }
}