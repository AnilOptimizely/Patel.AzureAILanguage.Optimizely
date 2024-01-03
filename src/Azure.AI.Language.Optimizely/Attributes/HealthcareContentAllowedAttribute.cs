namespace Patel.AzureAILanguage.Optimizely.Attributes
{
    public class HealthcareContentAllowedAttribute : TextAnalyticsBaseContentAttribute
    {
        public override bool AnalyzeCMSContent => true;
    }
}
