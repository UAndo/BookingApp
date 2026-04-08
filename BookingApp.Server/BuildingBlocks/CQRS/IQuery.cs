namespace BookingApp.Server.BuildingBlocks.CQRS;

public interface IQuery<TResponse> : IRequest<ErrorOr<TResponse>>
    where TResponse : notnull
{
}
