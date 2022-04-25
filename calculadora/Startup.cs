using Calculadora.Repository;
using Calculadora.Repository.Implementations;
using Estudo.Service;
using Estudo.Service.Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Collections.Generic;

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

            services.AddControllers();

            var connection = Configuration.GetConnectionString("XUXA");
            //  services.AddDbContext<MySQLContext>(options => options.UseMySql(connection));

            if (Environment.IsDevelopment())
            {
                MigrateDatabase(connection);
            }

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Calculadora", Version = "v1" });
            });
            services.AddScoped<IPersonService, PersonServiceImplementation>();
            services.AddScoped<IPersonRepository, PersonRepositoryImplementation>();

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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void MigrateDatabase(string connection)
        {
            try
            {
                //var evolveConnection = new MySql.Data.MySqlClient.MySQLConnection(connection);
                //var evolve = new Evolve.Evolve(evolveConnection, msg => Log.Information(msg))
                //{
                //    Locations = new List<string> { "db/migrations", "db/dataset" },
                //    IsEraseDisabled = true
                //};
            }
            catch (Exception ex)
            {
                Log.Error("Database migration failed", ex);
                throw;
            }
        }
    }
}