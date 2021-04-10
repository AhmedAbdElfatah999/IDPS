using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class IDPSContextSeed
    {
        public static async Task SeedAsync(IDPSContext context, ILoggerFactory loggerFactory)
        {
            try
            {
             
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

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

                if (!context.Diseases.Any())
                {
                    var diseaseData =
                        File.ReadAllText("../Infrastructure/Data/SeedData/Disease.json");

                    var disease = JsonSerializer.Deserialize<List<Disease>>(diseaseData);

                    foreach (var item in disease)
                    {
                        context.Diseases.Add(item);
                    }

                    await context.SaveChangesAsync();
                }

                if(!context.Hospitals.Any())
                {
                    var hospitalData = 
                        File.ReadAllText("../Infrastructure/Data/SeedData/Hospitals.json");
                    
                    var hospital = JsonSerializer.Deserialize<List<Hospital>>(hospitalData);

                    foreach(var item in hospital)
                    {
                        context.Hospitals.Add(item);
                    }
                    await context.SaveChangesAsync();
                }

                if(!context.Doctors.Any())
                {
                    var doctorData = 
                        File.ReadAllText("../Infrastructure/Data/SeedData/Doctors.json");
                    
                    var doctor = JsonSerializer.Deserialize<List<Doctor>>(doctorData);

                    foreach(var item in doctor)
                    {
                        context.Doctors.Add(item);
                    }
                    await context.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<IDPSContextSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}