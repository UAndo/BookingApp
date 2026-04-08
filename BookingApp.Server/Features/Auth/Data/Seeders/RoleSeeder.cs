// using BookingApp.Server.Features.Auth.Common;
//
// namespace BookingApp.Server.Infrastructure.Persistence.Seeders;
//
// public class RoleSeeder
// {
//     public static async Task SeedAsync(IServiceProvider serviceProvider)
//     {
//         using var scope = serviceProvider.CreateScope();
//         var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//
//         if (!dbContext.Roles.Any())
//         {   
//             var roles = new List<Role>
//             {
//                 new() { Id = RoleId.CreateUnique(), RoleName = "Admin" },
//                 new() { Id = RoleId.CreateUnique(), RoleName = "Partner" },
//                 new() { Id = RoleId.CreateUnique(), RoleName = "Guest" }
//             };
//
//             dbContext.Roles.AddRange(roles);
//             await dbContext.SaveChangesAsync();
//         }
//     }
// }