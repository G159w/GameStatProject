using System;
using System.IO;
using System.Text;
using AutoMapper;
using DotnetProjectBack.BusinessManagement;
using DotnetProjectBack.BusinessManagement.GamesStats;
using DotnetProjectBack.DataAccess;
using DotnetProjectBack.DataAccess.ApiRequests;
using DotnetProjectBack.DataAccess.GamesStats;
using DotnetProjectBack.DatabaseModels;
using DotnetProjectBack.Tools;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace DotnetProjectBack
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
            // Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(opts =>
              {
                  opts.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidateLifetime = true,
                      ValidateIssuerSigningKey = true,
                      ValidIssuer = Configuration["Jwt:Issuer"],
                      ValidAudience = Configuration["Jwt:Issuer"],
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                      ClockSkew = TimeSpan.FromMinutes(1)
                  };
              });

            // Database
            var connectionstring = Configuration.GetConnectionString("Default");
            if (string.IsNullOrEmpty(connectionstring))
            {
                throw new Exception("Empty connection string");
            }
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connectionstring));

            // Config and automapper
            services.AddSingleton(Configuration);
            services.AddAutoMapper();

            // API
            services.AddSingleton<IRequestFactory, RequestFactory>();

            // User
            services.AddTransient<IUserDataAccess, UserDataAccess>();
            services.AddTransient<IUserBusiness, UserBusiness>();

            // Game and user game
            services.AddTransient<IUserGameDataAccess, UserGameDataAccess>();
            services.AddTransient<IGameDataAccess, GameDataAccess>();
            services.AddTransient<IGameBusiness, GameBusiness>();

            // Elite
            services.AddTransient<IEliteDataAccess, EliteDataAccess>();
            services.AddTransient<IEliteBusiness, EliteBusiness>();

            // R6
            services.AddTransient<IR6DataAccess, R6DataAccess>();
            services.AddTransient<IR6Business, R6Business>();

            // Fortnite
            services.AddTransient<IFortniteDataAccess, FortniteDataAccess>();
            services.AddTransient<IFortniteBusiness, FortniteBusiness>();

            // Lol
            services.AddTransient<ILolDataAccess, LolDataAccess>();
            services.AddTransient<ILolBusiness, LolBusiness>();

            // GW2
            services.AddTransient<IGw2DataAccess, Gw2DataAccess>();
            services.AddTransient<IGw2Business, Gw2Business>();

            // Tools
            services.AddTransient<CacheDeleteHelper>();

            // Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Dotnet project API", Version = "v1" });
                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "DotnetProjectBack.xml");
                c.IncludeXmlComments(xmlPath);
            });

            services.AddCors();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            app.UseAuthentication();

            // Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dotnet project API");
            });

            app.UseMvc();
        }
    }
}