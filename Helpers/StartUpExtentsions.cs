using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PackageCopycat.Configs;
using PackageCopycat.Hubs;
using PackageCopycat.Interfaces;
using PackageCopycat.Services;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PackageCopycat.Helpers
{
    public static class StartupExtentsions
    {
        public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitConfigs>(configuration.GetSection("RabbitConfigs"));
            var config = configuration.GetSection("RabbitConfigs").Get<RabbitConfigs>();

            services.AddSingleton<IListenerService>(serviceProvider =>
            {
                var factory = new ConnectionFactory()
                {
                    HostName = config.HostName,
                    UserName = config.UserNAme,
                    Password = config.Password
                };

                services.Configure<RabbitConfigs>(configuration.GetSection("RabbitApiUri"));
                var bindingApiConfigs = configuration.GetSection("RabbitApiUri").Get<RabbitApiBindingConfigs>();
                return new ListenerService(factory, bindingApiConfigs);
            });
            return services;
        }
        

    public static IEndpointRouteBuilder AddSignalR(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapHub<PackagesHub>("/hub/packages");

            return endpoints;
        }
    }
}
