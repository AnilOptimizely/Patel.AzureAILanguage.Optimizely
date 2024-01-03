namespace Patel.AzureAILanguage.Optimizely.Attributes
{
    public class TextAnalyticsAllowedAttribute : TextAnalyticsBaseContentAttribute
    {
        public override bool AnalyzeCMSContent => true;
    }
}
