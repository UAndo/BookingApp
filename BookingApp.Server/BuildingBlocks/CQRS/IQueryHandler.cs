namespace BookingApp.Server.BuildingBlocks.CQRS;

public interface IQueryHandler<in TQuery, TResponse>
: IRequestHandler<TQuery, ErrorOr<TResponse>>
where TQuery : IQuery<TResponse>
where TResponse : notnull
{
}
