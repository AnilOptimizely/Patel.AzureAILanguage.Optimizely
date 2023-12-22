using Azure.AI.TextAnalytics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureAILanguage.Interfaces
{
    public interface IAzureAITextAnalyticsService
    {
        Task<List<string>> ExtractiveSummarisation(string textToTransliterate);
        Task<List<string>> AbstractiveSummarisation(List<string> batchedDocuments);
        DetectedLanguage LanguageDetectionExample(string languageDetectionText);
        TextSentiment AnalyseSentimentTextField(string sentimentText);
        Task<List<string>> RecogniseLinkedEntitiesFromText(string linkedEntitiesText);
        Task<List<string>> ExtractKeyPhrasesFromText(string keyPhraseText);
        List<string> ProcessHealthcareContentFromText(string healthcareText);
    }
}
