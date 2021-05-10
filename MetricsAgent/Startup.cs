using MetricsAgent.DAL;
using MetricsAgent.DAL.Interfaces;
using MetricsAgent.DAL.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent
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
            services.AddControllers();
            services.AddSingleton<SQLiteConnectionFactory>();
            ConfigureSqlLiteConnection(services);
            services.AddSingleton<ICpuMetricsRepository, CpuMetricsRepository>();
            services.AddSingleton<IDotNetMetricsRepository, DotNetMetricsRepository>();
            services.AddSingleton<IHddMetricsRepository, HddMetricsRepository>();
            services.AddSingleton<INetworkMetricsRepository, NetworkMetricsRepository>();
            services.AddSingleton<IRamMetricsRepository, RamMetricsRepository>();
        }
        private void ConfigureSqlLiteConnection(IServiceCollection services)
        {
            var connect = new SQLiteConnectionFactory();
            var connection = new SQLiteConnection(connect.Connect());
            connection.Open();
            PrepareSchema(connection);
        }

        private void PrepareSchema(SQLiteConnection connection)
        {
            using var command = new SQLiteCommand(connection);

            command.CommandText = "DROP TABLE IF EXISTS cpumetrics";
            command.ExecuteNonQuery();
            command.CommandText = @"CREATE TABLE cpumetrics(id INTEGER PRIMARY KEY, value INT, time INT64)";
            command.ExecuteNonQuery();
            command.CommandText = "INSERT INTO cpumetrics(value, time) VALUES(40, 100000)";
            command.ExecuteNonQuery();
            command.CommandText = "INSERT INTO cpumetrics(value, time) VALUES(10, 200000)";
            command.ExecuteNonQuery();
            command.CommandText = "INSERT INTO cpumetrics(value, time) VALUES(14, 300000)";
            command.ExecuteNonQuery();

            command.CommandText = "DROP TABLE IF EXISTS dotnetmetrics";
            command.ExecuteNonQuery();
            command.CommandText = @"CREATE TABLE dotnetmetrics(id INTEGER PRIMARY KEY, value INT, time INT64)";
            command.ExecuteNonQuery();
            command.CommandText = "INSERT INTO dotnetmetrics(value, time) VALUES(340, 100000)";
            command.ExecuteNonQuery();
            command.CommandText = "INSERT INTO dotnetmetrics(value, time) VALUES(110, 200000)";
            command.ExecuteNonQuery();
            command.CommandText = "INSERT INTO dotnetmetrics(value, time) VALUES(814, 300000)";
            command.ExecuteNonQuery();

            command.CommandText = "DROP TABLE IF EXISTS hddmetrics";
            command.ExecuteNonQuery();
            command.CommandText = @"CREATE TABLE hddmetrics(id INTEGER PRIMARY KEY, value INT, time INT64)";
            command.ExecuteNonQuery();
            command.CommandText = "INSERT INTO hddmetrics(value, time) VALUES(490, 100000)";
            command.ExecuteNonQuery();
            command.CommandText = "INSERT INTO hddmetrics(value, time) VALUES(170, 200000)";
            command.ExecuteNonQuery();
            command.CommandText = "INSERT INTO hddmetrics(value, time) VALUES(184, 300000)";
            command.ExecuteNonQuery();

            command.CommandText = "DROP TABLE IF EXISTS networkmetrics";
            command.ExecuteNonQuery();
            command.CommandText = @"CREATE TABLE networkmetrics(id INTEGER PRIMARY KEY, value INT, time INT64)";
            command.ExecuteNonQuery();
            command.CommandText = "INSERT INTO networkmetrics(value, time) VALUES(406, 100000)";
            command.ExecuteNonQuery();
            command.CommandText = "INSERT INTO networkmetrics(value, time) VALUES(105, 200000)";
            command.ExecuteNonQuery();
            command.CommandText = "INSERT INTO networkmetrics(value, time) VALUES(144, 300000)";
            command.ExecuteNonQuery();

            command.CommandText = "DROP TABLE IF EXISTS rammetrics";
            command.ExecuteNonQuery();
            command.CommandText = @"CREATE TABLE rammetrics(id INTEGER PRIMARY KEY, value INT, time INT64)";
            command.ExecuteNonQuery();
            command.CommandText = "INSERT INTO rammetrics(value, time) VALUES(407, 100000)";
            command.ExecuteNonQuery();
            command.CommandText = "INSERT INTO rammetrics(value, time) VALUES(109, 200000)";
            command.ExecuteNonQuery();
            command.CommandText = "INSERT INTO rammetrics(value, time) VALUES(148, 300000)";
            command.ExecuteNonQuery();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
