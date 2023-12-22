using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Azure.AI.Language.Optimizely
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAzureAILanguageOptimizely(this IServiceCollection services)
        {
            return AddAzureAILanguageOptimizely(services, _ => { });
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
