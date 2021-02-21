using FoodPal.Orders.BackgroundServices.Handlers;
using FoodPal.Orders.BackgroundServices.Handlers.Contracts;
using FoodPal.Orders.BackgroundServices.Settings;
using FoodPal.Orders.BackgroundWorkers.Workers;
using FoodPal.Orders.Data;
using FoodPal.Orders.Mappers;
using FoodPal.Orders.MessageBroker.Contracts;
using FoodPal.Orders.MessageBroker.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FoodPal.Orders.BackgroundWorkers
{
    internal static class HostServices
    {
        public static void Configure(HostBuilderContext hostContext, IServiceCollection services)
        {
            // AutoMapper profiles
            services.AddAutoMapper(typeof(AbstractProfile).Assembly);

            services.Configure<HttpProviderEndpoints>(x => hostContext.Configuration.Bind("HttpProviderEndpoints", x));

            // Message broker configuration
            services.Configure<MessageBrokerConnectionSettings>(x => hostContext.Configuration.Bind("MessageBrokerSettings", x));
            services.AddTransient<IMessageBroker, ServiceBusMessageBroker>();

            services.AddTransient<IMessageHandlerFactory, MessageHandlerFactory>();
            services.AddTransient<NewOrderMessageHandler>();
            services.AddTransient<ProviderProcessedOrderItemsHandler>();

            // Data layer configuration
            string dbConnectionString = hostContext.Configuration.GetConnectionString("OrdersConnectionString");
            services.AddTransient<IOrdersContextFactory, OrdersContextFactory>();
            services.AddTransient<IOrdersUnitOfWork, OrdersUnitOfWork>(x => new OrdersUnitOfWork(x.GetService<IOrdersContextFactory>().CreateDbContext(dbConnectionString)));

            services.AddHostedService<NewOrderWorker>();
            services.AddHostedService<KfcProviderOrderItemsWorker>();
            services.AddHostedService<XyzProviderOrderItemsWorker>();
            services.AddHostedService<PrestoPizzaProviderOrderItemsWorker>();
        }
    }
}
