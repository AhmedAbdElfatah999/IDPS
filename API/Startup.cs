using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Infrastructure;
using Core.Interfaces;
using Core.Entities;
using API.Helpers;
using AutoMapper;
using API.Middleware;
using API.Extensions;
using Infrastructure.Identity;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using System;

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
            services.AddScoped(typeof(IPersonGenericRepository<>),(typeof(PersonGenericRepository<>)));
            services.AddScoped<ITokenService,TokenService>();
            services.AddControllers();
            services.AddAutoMapper(typeof(MappingProfiles));
            services.AddDbContext<IDPSContext>(x =>
            x.UseSqlServer("server=(localdb)\\mssqllocaldb;database=IDPS.db;trusted_connection=true;MultipleActiveResultSets=true",x => x.MigrationsAssembly("API")));
            services.AddDbContext<AppIdentityDbContext>(x => 
            {
                x.UseSqlServer(_configuration.GetConnectionString("IdentityConnection"));
            });
           services.AddApplicationServices();
           services.AddIdentityServices(_configuration);

            // For Identity  
            services.AddIdentity<Person, IdentityRole>()  
                .AddEntityFrameworkStores<AppIdentityDbContext>()  
                .AddDefaultTokenProviders();  
           //google logins
        services.AddAuthentication()
        .AddGoogle(options =>
        {
            IConfigurationSection googleAuthNSection =
                Configuration.GetSection("Authentication:Google");

            options.ClientId = googleAuthNSection["ClientId"];
            options.ClientSecret = googleAuthNSection["ClientSecret"];
        });
        //facebook logins
        services.AddAuthentication().AddFacebook(facebookOptions =>
        {
            facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
            facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
        });
        //add emailsender service
        services.Configure<EmailSender>(Configuration);
        //token life span,the token is valid for 2 h
        services.Configure<DataProtectionTokenProviderOptions>(opt =>
                  opt.TokenLifespan = TimeSpan.FromHours(2));

        services.Configure<AuthMessageSenderOptions>(Configuration);

           services.AddSwaggerDocumentation();             
            services.AddCors(opt => 
            {
                opt.AddPolicy("CorsPolicy", policy => 
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                });
            });
           
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
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwaggerDocumentation();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
