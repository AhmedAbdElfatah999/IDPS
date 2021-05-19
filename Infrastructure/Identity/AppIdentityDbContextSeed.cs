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
        public static async Task SeedUsersAsync(UserManager<Person> userManager,RoleManager<IdentityRole> _roleManager,AppIdentityDbContext context)
        {
           
                if (!context.Specializations.Any())
                {
                    var specializationData =
                        File.ReadAllText("../Infrastructure/Data/SeedData/specialization.json");

                    var specializations = JsonSerializer.Deserialize<List<Specialization>>(specializationData);

                    foreach (var item in specializations)
                    {
                        context.Specializations.Add(item);
                    }

                    await context.SaveChangesAsync();
                }
            if(!userManager.Users.Any())
                {
                    //add Admin
                var admin = new Admin
                {
                    Name = "Bob",
                    Email = "bob@test.com",
                    UserName = "bob@test.com",
                    Address ="10 street ,Giza",
                    Password="Pa$$w0rd"
                };

                await userManager.CreateAsync(admin);
             if (await _roleManager.RoleExistsAsync(PersonRoles.Admin))  
                    {  
                        await userManager.AddToRoleAsync(admin, PersonRoles.Patient);  
                    } 
                var patient = new Patient
                {
                    Name = "Ali",
                    Email = "ali@test.com",
                    UserName = "ali@test.com",
                    Address ="10 street ,Giza",
                    Password="Pa$$w0rd"
                };

                await userManager.CreateAsync(patient);
             if (await _roleManager.RoleExistsAsync(PersonRoles.Patient))  
                    {  
                        await userManager.AddToRoleAsync(patient, PersonRoles.Patient);  
                    } 
                    
                 //add doctors   
                    var doctorData = 
                        File.ReadAllText("../Infrastructure/Data/SeedData/Doctors.json");
                    
                    var doctors = JsonSerializer.Deserialize<List<Doctor>>(doctorData);

                    foreach(var doctor in doctors)
                    {
                         await userManager.CreateAsync(doctor,"Pa$$w0rd");
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