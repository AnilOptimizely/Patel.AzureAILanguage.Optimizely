namespace Patel.AzureAILanguage.Optimizely.Attributes
{
    public class DetectLanguageAttribute : TextAnalyticsBaseContentAttribute
    {
        public override bool AnalyzeCMSContent => true;
    }
}
