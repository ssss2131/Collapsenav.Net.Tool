using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace Collapsenav.Net.Tool.Ext;

public static class QuartzServiceExt
{
    public static IServiceCollection AddDIJobFactory(this IServiceCollection services)
    {
        if (!services.Any(item => item.Lifetime == ServiceLifetime.Singleton && item.ServiceType == typeof(DIJobFactory)))
            services.AddSingleton<IJobFactory, DIJobFactory>();
        return services;
    }

    public static IServiceCollection AddJob<Job>(this IServiceCollection services, int len) where Job : class, IJob
    {
        QuartzNode.Builder ??= new();
        QuartzNode.Builder.AddJob<Job>(len);
        return services.AddTransient<Job>().AddDIJobFactory();
    }
    public static IServiceCollection AddJob<Job>(this IServiceCollection services, string cron) where Job : class, IJob
    {
        QuartzNode.Builder ??= new();
        QuartzNode.Builder.AddJob<Job>(cron);
        return services.AddTransient<Job>().AddDIJobFactory();
    }

    public static IServiceCollection AddDefaultQuartzService(this IServiceCollection services)
    {
        return services.AddDIJobFactory().AddHostedService<EasyJobService>();
    }
    public static IServiceCollection AddDefaultQuartzService(this IServiceCollection services, Action<QuartzJobBuilder> action)
    {
        action?.Invoke(QuartzNode.Builder ??= new());
        services
        .AddDIJobFactory()
        .AddHostedService<EasyJobService>()
        ;
        return services;
    }
}