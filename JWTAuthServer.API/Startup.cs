using JWTAuthServer.Core.Configuration;
using JWTAuthServer.Core.Entity;
using JWTAuthServer.Core.Repositories.Abstract;
using JWTAuthServer.Core.Services;
using JWTAuthServer.Core.UnitOfWork;
using JWTAuthServer.Data;
using JWTAuthServer.Data.Repositories;
using JWTAuthServer.Service.Services;
using JWTAuthServer.SharedLibrary.Configurations;
using JWTAuthServer.SharedLibrary.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;

namespace JWTAuthServer.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Dependency Injection
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
            services.AddScoped(typeof(IServiceGeneric<,>), typeof(ServiceGeneric<,>));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SqlServer"), sqlOptions => {
                    sqlOptions.MigrationsAssembly("JWTAuthServer.Data");
                });
            });

            services.AddIdentity<UserApp,IdentityRole>(Opt =>
            {
                Opt.User.RequireUniqueEmail = true;
                Opt.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();


            //TokenOption i�lemleri
            services.Configure<CustomTokenOption>(Configuration.GetSection("TokenOption"));
            services.Configure<List<Client>>(Configuration.GetSection("Clients"));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
            {
                var tokenOptions = Configuration.GetSection("TokenOption").Get<CustomTokenOption>();
                opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidIssuer = tokenOptions.Issuer,
                    //benim projemin kendisi
                    ValidAudience = tokenOptions.Audience[0],
                    IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),

                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    //10:45:00
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "JWTAuthServer.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "JWTAuthServer.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
