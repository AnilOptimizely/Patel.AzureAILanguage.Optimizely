using EPiServer.ServiceLocation;

namespace Patel.AzureAILanguage.Optimizely
{
    [ServiceConfiguration]
    public class AzureAILanguageOptimizelyOptions
    {
        public string TextAnalyticsSubscriptionKey { get; set; }
        public string TextAnalyticsEndpoint { get; set; }
    }
}
