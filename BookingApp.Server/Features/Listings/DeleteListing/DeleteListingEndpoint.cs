using BookingApp.Server.BuildingBlocks.Extensions;
using BookingApp.Server.Infrastructure.Web;
using System.Security.Claims;

namespace BookingApp.Server.Features.Listings.DeleteListing
{
    public class DeleteListingEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/api/listings/{id:guid}", async (Guid id, ClaimsPrincipal user, ISender sender) =>
            {
                var command = new DeleteListingCommand(id, user.GetUserId());

                var result = await sender.Send(command);

                return result.ToNoContentResult();
            })
            .WithName("DeleteListing")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Deletes a listing")
            .WithDescription("Deletes a listing by its unique identifier.")
            .WithTags("Listings")
            //.RequireRateLimiting("listings")
            .RequireAuthorization();
        }
    }
}
