using Azure.AI.TextAnalytics;
using Azure;
using AzureAILanguage.Interfaces;
using Microsoft.Extensions.Options;
using Azure.AI.Language.Optimizely;
using EPiServer.ServiceLocation;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;

namespace AzureAILanguage.Services
{
    public class AzureAITextAnalyticsService : IAzureAITextAnalyticsService
    {
        private static IOptions<AzureAILanguageOptimizelyOptions> _configuration;
        private static IOptions<AzureAILanguageOptimizelyOptions> Configuration => _configuration ??= ServiceLocator.Current.GetInstance<IOptions<AzureAILanguageOptimizelyOptions>>();

        private static readonly string TextAnalyticsSubscriptionKey = Configuration.Value.TextAnalyticsSubscriptionKey;
        private static readonly string TextAnalyticsEndpoint = Configuration.Value.TextAnalyticsEndpoint;

        public TextAnalyticsClient GetTextAnalyticsClient()
        {
            var key = TextAnalyticsSubscriptionKey;
            Uri endpointUrl = new(TextAnalyticsEndpoint);
            AzureKeyCredential credential = new(TextAnalyticsSubscriptionKey);
            TextAnalyticsClient client = new(endpointUrl, credential);
            return client;
        }
        public DetectedLanguage LanguageDetectionExample(string languageDetectionText)
        {
            if (!string.IsNullOrWhiteSpace(languageDetectionText))
            {
                DetectedLanguage detectedLanguage = GetTextAnalyticsClient().DetectLanguage(languageDetectionText);
                Console.WriteLine($"Language Detection operation has completed");
                Console.WriteLine("Language:");
                Console.WriteLine($"\t{detectedLanguage.Name},\tISO-6391: {detectedLanguage.Iso6391Name}\n");
                return detectedLanguage;

            }
            return new DetectedLanguage();
        }

        public List<string> ProcessExtractiveSummarisation(List<string> batchedDocuments)
        {
            var analyseExtractiveSummarisation = ExtractiveSummarisation(batchedDocuments).Result;
            return analyseExtractiveSummarisation;
        }

        private async Task<List<string>> ExtractiveSummarisation(List<string> batchedDocuments)
        {
            var listSentences = new List<string>();
            // Perform the text analysis operation.
            ExtractiveSummarizeOperation operation = GetTextAnalyticsClient().ExtractiveSummarize(WaitUntil.Completed, batchedDocuments);
            Console.WriteLine($"Extractive Summarisation operation has completed");

            Console.WriteLine($"Created On   : {operation.CreatedOn}");
            Console.WriteLine($"Expires On   : {operation.ExpiresOn}");
            Console.WriteLine($"Status       : {operation.Status}");

            await foreach (ExtractiveSummarizeResultCollection documentsInPage in operation.Value)
            {
                Console.WriteLine($"Extractive Summarisation, version: \"{documentsInPage.ModelVersion}\"");
                Console.WriteLine();

                foreach (ExtractiveSummarizeResult documentResult in documentsInPage)
                {
                    if (documentResult.HasError)
                    {
                        continue;
                    }
                    Console.WriteLine($"  Extracted the following {documentResult.Sentences.Count} sentence(s):");
                    foreach (ExtractiveSummarySentence sentence in documentResult.Sentences)
                    {
                        Console.WriteLine($"  Sentence: {sentence.Text}");
                        listSentences.Add(sentence.Text);
                    }
                }
            }
            return listSentences;
        }
        public async Task<List<string>> AbstractiveSummarisation(List<string> batchedDocuments)
        {
            var listAbstractSummaries = new List<string>();
            // Perform the text analysis operation.
            AbstractiveSummarizeOperation operation = GetTextAnalyticsClient().AbstractiveSummarize(WaitUntil.Completed, batchedDocuments);
            Console.WriteLine($"Abstractive Summarisation operation has completed");
            Console.WriteLine($"Status       : {operation.Status}");
            await foreach (AbstractiveSummarizeResultCollection documentsInPage in operation.Value)
            {
                foreach (AbstractiveSummarizeResult documentResult in documentsInPage)
                {
                    if (documentResult.HasError)
                    {
                        continue;
                    }
                    if (documentResult.Summaries.Any())
                    {
                        Console.WriteLine($"  Extracted the following {documentResult.Summaries.Count} Summaries:");
                        foreach (AbstractiveSummary summary in documentResult.Summaries)
                        {
                            Console.WriteLine($"  Sentence: {summary.Text}");
                            listAbstractSummaries.Add(summary.Text);
                        }
                    }
                }
            }
            return listAbstractSummaries;
        }
        public TextSentiment AnalyseSentimentTextField(string sentimentText)
        {
            if (!string.IsNullOrWhiteSpace(sentimentText))
            {
                var response = GetTextAnalyticsClient().AnalyzeSentiment(sentimentText);

                Console.WriteLine($"Analyse Sentiment operation of text field has completed");
                Console.WriteLine($"Text used for Sentiment operation: {sentimentText}");
                Console.WriteLine($"Document sentiment: {response.Value.Sentiment}\n");
                Console.WriteLine($"\tPositive score: {response.Value.ConfidenceScores.Positive:0.00}");
                Console.WriteLine($"\tNegative score: {response.Value.ConfidenceScores.Negative:0.00}");
                Console.WriteLine($"\tNeutral score: {response.Value.ConfidenceScores.Neutral:0.00}\n");

                return response.Value.Sentiment;
            }
            return TextSentiment.Neutral;
        }

        public List<string> ExtractKeyPhrasesFromText(List<string> batchedDocuments)
        {
            var listPhrases = new List<string>();
            var processRequest = GetTextAnalyticsClient().ExtractKeyPhrasesBatch(batchedDocuments);
            Console.WriteLine($"Key Phrase Extraction operation of text field has completed");
            var keyPhrases = processRequest.Value;
            
            Console.WriteLine($"Number of Key Phrases detected is: {keyPhrases.Count}");
            foreach (var keyPhrase in keyPhrases)
            {
                foreach (var keyPhraseText in keyPhrase.KeyPhrases)
                {
                    Console.WriteLine($"  Key Phrase: {keyPhrase}");
                    listPhrases.Add(keyPhraseText);
                }
            }
            return listPhrases;
        }

        public List<string> RecogniseLinkedEntitiesFromText(List<string> batchedDocuments)
        {
            var listLinkedEntities = new List<string>();
            var response = GetTextAnalyticsClient().RecognizeEntitiesBatch(batchedDocuments);
            var linkedEntities = response.Value;

            Console.WriteLine($"Recognise Linked Entities operation of text field has completed");
            Console.WriteLine($"Recognised {linkedEntities.Count} entities:");

            foreach (var linkedEntity in linkedEntities)
            {
                foreach (var match in linkedEntity.Entities)
                {
                    Console.WriteLine($"  Name: {match.Text}");
                    listLinkedEntities.Add(match.Text);
                }
            }
            return listLinkedEntities;
        }

        public List<string> ProcessHealthcareContentFromText(string healthcareText)
        {
            var getHealthcareContentList = AnalyseHealthcareContentFromText(healthcareText).Result;
            return getHealthcareContentList;
        }

        private async Task<List<string>> AnalyseHealthcareContentFromText(string healthcareText)
        {
            var healthcareTextList = new List<string>();
            var batchedTextList = new List<string>
            {
                healthcareText
            };
            
            AnalyzeHealthcareEntitiesOperation operation = GetTextAnalyticsClient().AnalyzeHealthcareEntities(WaitUntil.Completed, batchedTextList);

            await foreach (AnalyzeHealthcareEntitiesResultCollection documentsInPage in operation.Value)
            {
                Console.WriteLine($"Analysis of Text Analytics for Health on a text field has completed");
                Console.WriteLine($"Analyse Healthcare Entities, model version: \"{documentsInPage.ModelVersion}\"");

                foreach (AnalyzeHealthcareEntitiesResult documentResult in documentsInPage)
                {
                    if (documentResult.HasError)
                    {
                        Console.WriteLine($"  Error!");
                        Console.WriteLine($"  Document error code: {documentResult.Error.ErrorCode}");
                        Console.WriteLine($"  Message: {documentResult.Error.Message}");
                        continue;
                    }

                    Console.WriteLine($"  Recognized the following {documentResult.Entities.Count} healthcare entities:");

                    // View the healthcare entities that were recognized.
                    foreach (HealthcareEntity entity in documentResult.Entities)
                    {
                        if (!string.IsNullOrWhiteSpace(entity.Category.ToString()))
                        {
                            if (entity.Category == HealthcareEntityCategory.MedicationName)
                            {
                                Console.WriteLine($"  Entity: {entity.Text}");
                                Console.WriteLine($"  Category: {entity.Category}");
                                Console.WriteLine($"  Offset: {entity.Offset}");
                                Console.WriteLine($"  Length: {entity.Length}");
                                healthcareTextList.Add(entity.Text);
                            }
                        }
                       
                    }
                }
            }
            return healthcareTextList;
        }
    }
}
