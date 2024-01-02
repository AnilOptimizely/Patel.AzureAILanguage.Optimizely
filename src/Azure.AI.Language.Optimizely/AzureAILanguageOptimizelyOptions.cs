using EPiServer.ServiceLocation;

namespace Azure.AI.Language.Optimizely
{
    [ServiceConfiguration]
    public class AzureAILanguageOptimizelyOptions
    {
        public string TextAnalyticsSubscriptionKey { get; set; }
        public string TextAnalyticsEndpoint { get; set; }
    }
}
