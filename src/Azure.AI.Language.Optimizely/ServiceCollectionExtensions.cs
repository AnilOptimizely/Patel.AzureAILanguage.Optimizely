using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Patel.AzureAILanguage.Optimizely
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAzureAILanguageOptimizely(this IServiceCollection services)
        {
            return services.AddAzureAILanguageOptimizely(_ => { });
        }

        public static IServiceCollection AddAzureAILanguageOptimizely(this IServiceCollection services, Action<AzureAILanguageOptimizelyOptions> setupAction)
        {
            services.AddOptions<AzureAILanguageOptimizelyOptions>().Configure<IConfiguration>((options, configuration) =>
            {
                setupAction(options);
                configuration.GetSection("Patel:AzureAILanguageOptimizely").Bind(options);
            });

            return services;
        }
    }
}
