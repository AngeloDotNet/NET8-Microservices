using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.Shared.Services;

public static class DependencyInjection
{
    private static readonly string dbhostname = "YOUR-HOST";
    private static readonly string dbdatabase = "YOUR-DATABASE";
    private static readonly string dbusername = "YOUR-USER";
    private static readonly string dbpassword = "YOUR-PASSWORD";

    public static readonly string database = $"Data Source={dbhostname};Initial Catalog={dbdatabase};User ID={dbusername};Password={dbpassword};Encrypt=False";

    private static readonly string hostname = "rabbitmq://localhost";
    private static readonly string username = "guest";
    private static readonly string password = "guest";
    private static readonly string queueReceiver = "event-listener";

    public static IServiceCollection AddProducerRabbitMQ(this IServiceCollection services)
    {
        services.AddMassTransit(options =>
        {
            options.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(hostname), h =>
                {
                    h.Username(username);
                    h.Password(password);
                });

            });
        });

        return services;
    }

    public static IServiceCollection AddConsumerRabbitMQ<Consumer>(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            //x.AddConsumer<ProductCreatedConsumer>();
            x.AddConsumers(typeof(Consumer).Assembly);
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(hostname), h =>
                {
                    h.Username(username);
                    h.Password(password);
                });
                cfg.ReceiveEndpoint(queueReceiver, e =>
                {
                    //e.ConfigureConsumer<ProductCreatedConsumer>(context);
                    e.ConfigureConsumers(context);
                });
            });
        });

        return services;
    }
}