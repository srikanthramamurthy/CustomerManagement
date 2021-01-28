using System.Collections.Generic;
using CustomerManagement.Api.Middleware;
using CustomerManagement.Api.Repository;
using CustomerManagement.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;

namespace CustomerManagement.Api
{
    public class Startup
    {
        private static readonly ILoggerFactory DebugSqlLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddConsole(options => { })
                .AddFilter((category, level) =>
                    category == DbLoggerCategory.Database.Command.Name
                    && level == LogLevel.Information);
        });


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddControllers(options => options.EnableEndpointRouting = false);

            var corsDNSList = new List<string>();

            var corsConfiguration = Configuration.GetSection("CORSConfiguration");
            var localhostDNS = corsConfiguration.GetValue<string>("LocalHostDNS");
            corsDNSList.Add(localhostDNS);
            services.AddCors(options =>
            {
                options.AddPolicy("EnableCORS", builder =>
                {
                    builder.WithOrigins(corsDNSList.ToArray())
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            ConfigureServicesInternal(services);
            ConfigureDatabase(services);

            services.AddMvcCore();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Version = "v1", Title = "Customer Management API", Description = "API to manage customers"
                    });
            });
        }

        private void ConfigureDatabase(IServiceCollection services)
        {
            var connString = Configuration.GetConnectionString("CustomerManagementDB");
            services
                .AddDbContext<CustomerManagementDbContext>(options => options.UseSqlServer(connString));
        }

// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseCors("EnableCORS");

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer Management V1"); });

            app.UseMvc();
        }

        private void ConfigureServicesInternal(IServiceCollection services)
        {
            services.AddTransient<ICustomerRepository, CustomerRepository>();
        }
    }
}