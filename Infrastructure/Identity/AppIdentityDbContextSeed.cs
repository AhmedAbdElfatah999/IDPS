using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Identity;


namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<Person> userManager,RoleManager<IdentityRole> _roleManager)
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
             if (await _roleManager.RoleExistsAsync(PersonRoles.Admin))  
                    {  
                        await userManager.AddToRoleAsync(user, PersonRoles.Admin);  
                    } 
            }
            if(!userManager.Users.Any())
                {
                    var doctorData = 
                        File.ReadAllText("../Infrastructure/Data/SeedData/Doctors.json");
                    
                    var doctors = JsonSerializer.Deserialize<List<Doctor>>(doctorData);

                    foreach(var doctor in doctors)
                    {
                         await userManager.CreateAsync(doctor, "Pa$$w0rd");
                        //Add Role
                        if (await _roleManager.RoleExistsAsync(PersonRoles.Doctor))  
                        {  
                            await userManager.AddToRoleAsync(doctor, PersonRoles.Doctor);  
                        } 
                    }
                   
                }
        }
    }
}