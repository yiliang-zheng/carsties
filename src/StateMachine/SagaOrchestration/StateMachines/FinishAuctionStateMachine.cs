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
    public Event<AuctionMarkFinished> AuctionMarkFinishedEvent { get; set; }

    public Event<AuctionMarkFinishFailed> AuctionMarkFinishFailedEvent { get; set; }

    public Event<SearchMarkFinished> SearchMarkFinishedEvent { get; set; }

    public Event<SearchMarkFinishedFailed> SearchMarkFinishedFailedEvent { get; set; }

    public Event<BidMarkFinished> BidMarkFinishedEvent { get; set; }

    public Event<BidMarkFinishedFailed> BidMarkFinishedFailedEvent { get; set; }

    //States
    public State AuctionFinishRequestSent { get; set; }

    public State AuctionFinishMarked { get; set; }

    public State AuctionFinishMarkedFailed { get; set; }

    public State SearchMarkFinished { get; set; }

    public State SearchMarkFinishedFailed { get; set; }

    public State BidMarkFinished { get; set; }

    public State BidMarkFinishedFailed { get; set; }


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
            () => AuctionMarkFinishedEvent,
            p => p.CorrelateById(context => context.Message.CorrelationId));

        Event(
            () => AuctionMarkFinishFailedEvent,
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

        Event(() => BidMarkFinishedEvent,
            p => p.CorrelateById(context => context.Message.CorrelationId)
        );

        Event(() => BidMarkFinishedFailedEvent,
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
                .TransitionTo(AuctionFinishRequestSent)
                .Publish(context => new MarkBidFinishMessage(context.Saga.CorrelationId, context.Message.AuctionId))
                .Then(context =>
                {
                    this._logger
                        .ForContext("CorrelationId", context.Saga.CorrelationId)
                        .Information(
                            "MarkBidFinishMessage sent in FinishAuctionStateMachine: {ContextSaga}",
                            context.Saga);
                })
        );

        //bid svc done
        During(AuctionFinishRequestSent,
            Ignore(MarkAuctionFinishMessage),
            When(BidMarkFinishedEvent)
                .Then(context =>
                {
                    _logger
                        .ForContext("CorrelationId", context.Saga.CorrelationId)
                        .Information(
                            "BidMarkFinished received in FinishAuctionStateMachine: {ContextSaga} ",
                            context.Saga);
                })
                .TransitionTo(BidMarkFinished)
                .Then(context =>
                {
                    this._logger.ForContext("CorrelationId", context.Saga.CorrelationId)
                        .Information(
                            "Transit state into {SagaState}",
                            context.Saga.CurrentState
                        );
                })
                .Send(
                    new Uri("queue:mark-auction-finish"),
                    context => new FinishAuctionMessage
                    {
                        CorrelationId = context.Saga.CorrelationId,
                        AuctionId = context.Message.AuctionId,
                        Amount = context.Message.SoldAmount,
                        ItemSold = context.Message.ItemSold,
                        Winner = context.Message.Winner,
                        Seller = context.Message.Seller
                    }),
            When(BidMarkFinishedFailedEvent)
                .Then(context =>
                {
                    this._logger
                        .ForContext("CorrelationId", context.Saga.CorrelationId)
                        .Information(
                            "{Event} received in FinishAuctionStateMachine: {ContextSaga} ",
                            nameof(BidMarkFinishedFailedEvent),
                            context.Saga);
                })
                .TransitionTo(BidMarkFinishedFailed));

        //auction svc done
        During(BidMarkFinished,
            Ignore(MarkAuctionFinishMessage),
            When(AuctionMarkFinishedEvent)
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
                        SoldAmount = context.Message.SoldAmount,
                        Seller = context.Message.Seller,
                        ItemSold = context.Message.ItemSold
                    })
        );

        //search svc done
        During(AuctionFinishMarked,
            Ignore(MarkAuctionFinishMessage),
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
                .Publish(context=> new AuctionFinished
                {
                    AuctionId = context.Message.AuctionId,
                    Winner = context.Message.Winner,
                    Amount = context.Message.SoldAmount,
                    ItemSold = context.Message.ItemSold,
                    Seller = context.Message.Seller
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

        //raise markAuctionFailed event when any step failed
        DuringAny(
            When(BidMarkFinishedFailedEvent)
                .Publish(context => new MarkAuctionFinishFailed(context.Message.AuctionId, context.Saga.CorrelationId)),
            When(AuctionMarkFinishFailedEvent)
                .Publish(context =>
                new MarkAuctionFinishFailed(context.Message.AuctionId, context.Saga.CorrelationId)),
            When(SearchMarkFinishedFailedEvent)
                .Publish(context =>
                new MarkAuctionFinishFailed(context.Message.AuctionId, context.Saga.CorrelationId)));

        SetCompletedWhenFinalized();
    }
}