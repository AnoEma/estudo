using Calculadora.Configurations;
using Calculadora.ConfigurationService;
using Calculadora.ConfigurationService.Implementations;
using Calculadora.DTO;
using Calculadora.Hypermedia.Enricher;
using Calculadora.Hypermedia.Filers;
using Calculadora.Repository;
using Calculadora.Repository.Implementations;
using Calculadora.Service;
using Calculadora.Service.Implementations;
using Estudo.Service;
using Estudo.Service.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Estudo
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }
        public Startup
        (
            IConfiguration configuration,
            IWebHostEnvironment env
        )
        {
            Configuration = configuration;
            Environment = env;

            Log.Logger = new LoggerConfiguration()
                .WriteTo
                .Console()
                .CreateLogger();
        }


        public void ConfigureServices(IServiceCollection services)
        {
            var tokenConfig = new TokenConfiguration();

            new ConfigureFromConfigurationOptions<TokenConfiguration>
                (
                Configuration.GetSection("TokenConfiguration")
                ).Configure(tokenConfig);

            services.AddSingleton(tokenConfig);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = tokenConfig.Isssuer,
                    ValidAudience = tokenConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfig.Secret))
                };
            });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });


            services.AddCors(options => options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
            }));
            services.AddControllers();

            var connection = Configuration.GetConnectionString("Connection");

            services.AddDbContext<DatabaseContext>(options => options.UseMySql(connection));

            //if (Environment.IsDevelopment())
            //{
            //    MigrateDatabase(connection);
            //}

            services.AddMvc(options =>
            {
                options.RespectBrowserAcceptHeader = true;

                // options.FormatterMappings.SetMediaTypeMappingForFormat("xml", MediaTypeHeaderValue.Parse("application/xml"));
                options.FormatterMappings.SetMediaTypeMappingForFormat("json", MediaTypeHeaderValue.Parse("application/json"));

            }).AddXmlSerializerFormatters();

            var filterOptions = new HyperMediaFilterOptions();
            filterOptions.ContentResponseEnricherList.Add(new PersonEnricher());

            services.AddSingleton(filterOptions);


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Calculadora", Version = "v1" });
            });

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IPersonService, PersonServiceImplementation>();
            services.AddScoped<IPersonRepository, PersonRepositoryImplementation>();
            services.AddScoped<ILoginService, LoginServiceImpletation>();
            services.AddScoped<IUserRepository, UserRepositoryImplementation>();
            services.AddScoped<IFileService, FileService>();

            //  services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

            services.AddTransient<ITokenService, TokenService>();


            services.AddApiVersioning();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Calculadora v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute("DefaultApi", "{controller=values}/{id?}");
            });
        }

        private void MigrateDatabase(string connection)
        {
            try
            {
                var evolveConnection = new MySql.Data.MySqlClient.MySqlConnection(connection);
                var evolve = new Evolve.Evolve(evolveConnection, msg => Log.Information(msg))
                {
                    Locations = new List<string> { "db/migrations", "db/dataset" },
                    IsEraseDisabled = true
                };
            }
            catch (Exception ex)
            {
                Log.Error("Database migration failed", ex);
                throw;
            }
        }
    }
}