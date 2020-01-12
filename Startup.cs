using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using PackageCopycat.DbContexts;
using PackageCopycat.Services;
using PackageCopycat.Repositories;
using PackageCopycat.Interfaces;
using PackageCopycat.Helpers;
using PackageCopycat.Configs;
using PackageCopycat.Hubs;
using PackageCopycat.Controllers;
using PackageCopycat.Factories;
using PackageCopycat.Models;
using Microsoft.AspNetCore.SignalR;
using Serilog;
using PackageCopycat.Abstracts;

namespace PackageCopycat
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

            services.AddSingleton<IClientsContainer, ClientsConnectionContainer>();

            services.AddScoped<IFactory, QueueMessagesFactory>();
            services.AddScoped<IHandler, QueueMassagesHandler>();
            services.AddScoped<IClientManager, ClientManager>();

            services.AddRabbitMQ(Configuration);
            services.AddSingleton<IPublisher, HubPublisher>();
            
            services.AddSignalR();

            services.AddCors(options => options.AddPolicy("CorsPolicy",
            builder =>
            {
                builder.AllowAnyMethod()
                       .WithOrigins("http://localhost:3000")
                       .AllowCredentials()
                       .AllowAnyHeader();
                       //.SetIsOriginAllowed((origin) => true);
            }));

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
            app.UseCors("CorsPolicy");
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.AddSignalR();
            });
            

        }
    }

}