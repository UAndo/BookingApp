namespace BookingApp.Server.BuildingBlocks.CQRS;

public interface ICommandHandler<in TCommand>
: IRequestHandler<TCommand, ErrorOr<Unit>>
where TCommand : ICommand
{
}

public interface ICommandHandler<in TCommand, TResponse>
    : IRequestHandler<TCommand, ErrorOr<TResponse>>
    where TCommand : ICommand<TResponse>
    where TResponse : notnull
{
}
