using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<Person> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new Person
                {
                    Name = "Bob",
                    Email = "bob@test.com",
                    UserName = "bob@test.com",
                    Address ="10 street ,Giza"
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}