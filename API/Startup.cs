using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Infrastructure;
using Core.Interfaces;
using API.Helpers;
using AutoMapper;
using API.Middleware;
using API.Extensions;
using Infrastructure.Identity;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
      
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDiseaseRepository,DiseaseRepository>();
            services.AddScoped(typeof(IGenericRepository<>),(typeof(GenericRepository<>)));
            services.AddControllers();
            services.AddAutoMapper(typeof(MappingProfiles));
            services.AddDbContext<IDPSContext>(x =>
            x.UseSqlServer("server=(localdb)\\mssqllocaldb;database=IDPS.db;trusted_connection=true;MultipleActiveResultSets=true",x => x.MigrationsAssembly("API")));
            services.AddDbContext<AppIdentityDbContext>(x => 
            {
                x.UseSqlServer(_configuration.GetConnectionString("IdentityConnection"));
            });
            services.AddCors(opt => 
            {
                opt.AddPolicy("CorsPolicy", policy => 
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                });
            });
           services.AddApplicationServices();
           services.AddSwaggerDocumentation();   
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware <ExceptionMiddleware>();
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseStaticFiles();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseSwaggerDocumentation();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
