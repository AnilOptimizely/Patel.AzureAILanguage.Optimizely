using Azure.AI.TextAnalytics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureAILanguage.Interfaces
{
    public interface IAzureAITextAnalyticsService
    {
        List<string> ProcessExtractiveSummarisation(List<string> batchedDocuments);
        Task<List<string>> AbstractiveSummarisation(List<string> batchedDocuments);
        DetectedLanguage LanguageDetectionExample(string languageDetectionText);
        TextSentiment AnalyseSentimentTextField(string sentimentText);
        List<string> RecogniseLinkedEntitiesFromText(List<string> batchedDocuments);
        List<string> ProcessHealthcareContentFromText(string healthcareText);
        List<string> ExtractKeyPhrasesFromText(List<string> batchedDocuments);
    }
}
