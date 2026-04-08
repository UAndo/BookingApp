namespace BookingApp.Server.BuildingBlocks.CQRS;

public interface ICommand<TResponse> : IRequest<ErrorOr<TResponse>>
where TResponse : notnull
{
}

public interface ICommand : ICommand<Unit>
{
}
