using Azure.AI.Language.Optimizely.Attributes;
using Azure.AI.Language.Optimizely.Helpers;
using Azure.AI.TextAnalytics;
using AzureAILanguage.Interfaces;
using AzureAILanguage.Services;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Logging;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Microsoft.Extensions.DependencyInjection;
using ILogger = EPiServer.Logging.ILogger;

namespace OptimizelyResearch.Business.Initialization
{
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class ChangeEventInitialization : IConfigurableModule
    {
        private ILogger _log = LogManager.GetLogger(typeof(ChangeEventInitialization));
        protected readonly Injected<IAzureAITextAnalyticsService> _azureTextAnalyticsService;
        private IContentEvents _contentEvents = null;
        private IContentLoader _contentLoader = null;

        public void Initialize(InitializationEngine context)
        {
            ServiceProviderHelper serviceLocationHelper = context.Locate;
            _contentEvents = context.Locate.Advanced.GetInstance<IContentEvents>();
            _contentLoader = serviceLocationHelper.ContentLoader();
            _contentEvents.PublishingContent += Events_PublishingContent;
        }

        void IConfigurableModule.ConfigureContainer(ServiceConfigurationContext context)
        {
            context.Services.AddSingleton<IAzureAITextAnalyticsService, AzureAITextAnalyticsService>();
            //context.Services.AddMvc(options => options.EnableEndpointRouting = false);
        }

        private void Events_PublishingContent(object sender, ContentEventArgs e)
        {
            var getIContentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
            var getStartPage = getIContentLoader.Get<IContent>(ContentReference.StartPage);
            if (getStartPage != null)
            {
                if (e.Content is IContent content)
                {
                    var detectIfTextAnalyticsAllowed = OptimizelyCmsHelpers.GetPagePropertiesWithAttribute(getStartPage, typeof(TextAnalyticsAllowedAttribute));
                    if (detectIfTextAnalyticsAllowed.Any() && detectIfTextAnalyticsAllowed.Count == 1)
                    {
                        var getFirstDefaultOption = detectIfTextAnalyticsAllowed.FirstOrDefault();
                        var getAllowedValue = getFirstDefaultOption.Property.GetValue(getFirstDefaultOption.Content);
                        if (getAllowedValue is bool)
                        {
                            bool value = (bool)getAllowedValue;
                            if (value)
                            {   
                                var listErrorMessages = new List<string>();
                                // Sentiment analysis
                                var processSentiment = OptimizelyCmsHelpers.ProcessSentimentAnalysis(content);
                                if (processSentiment.Any())
                                {
                                    foreach (var sentimentText in processSentiment)
                                    {
                                        listErrorMessages.Add(sentimentText);
                                    }
                                }
                                //Langauge Detection
                                var processLanguageDetection = OptimizelyCmsHelpers.ProcessLanguageDetectionAnalysis(content);
                                if (processLanguageDetection.Any())
                                {
                                    foreach (var sentimentText in processLanguageDetection)
                                    {
                                        listErrorMessages.Add(sentimentText);
                                    }
                                }
                                //Healthcare Content
                                var detectIfHealthCareContentAllowed = OptimizelyCmsHelpers.GetPagePropertiesWithAttribute(getStartPage, typeof(HealthcareContentAllowedAttribute));
                                if (detectIfHealthCareContentAllowed.Any() && detectIfHealthCareContentAllowed.Count == 1)
                                {
                                    var determineHealthcareContentAllowed = OptimizelyCmsHelpers.DetermineHealthcareContentAllowed(detectIfHealthCareContentAllowed.FirstOrDefault());
                                    if (!determineHealthcareContentAllowed)
                                    {
                                        var processHealthCareContent = OptimizelyCmsHelpers.ProcessHealthcareContentAnalysis(content);
                                        if (!string.IsNullOrWhiteSpace(processHealthCareContent))
                                        {
                                            listErrorMessages.Add(processHealthCareContent);
                                        }
                                    }
                                }
                                else
                                {
                                    if (detectIfHealthCareContentAllowed.Count > 1)
                                    {
                                        e.CancelReason = string.Format("Please only have 1 CMS boolean property with attribute HealthcareContentAllowed");
                                        e.CancelAction = true;
                                    }
                                }

                                if (listErrorMessages.Any())
                                {
                                    var startString = "Unable to publish - Azure AI Language - Text Analytics has detected the following ";
                                    var errorMessagesString = string.Join(", ", listErrorMessages);
                                    var endString = "Please review content and publish again";

                                    e.Content = content;
                                    e.CancelReason = startString + errorMessagesString + endString;
                                    e.CancelAction = true;
                                }
                                

                            }
                        }
                    }
                    else
                    {
                        if (detectIfTextAnalyticsAllowed.Count > 1)
                        {
                            e.Content = content;
                            e.CancelReason = string.Format("Please only have 1 CMS boolean property with attribute TextAnalyticsAllowed");
                            e.CancelAction = true;
                        }
                    }
                    //if (getStartPage.AnalyseText)
                    //{
                    //    //if (getStartPage.DetectTextRequest)
                    //    //{
                    //    //    if (!string.IsNullOrWhiteSpace(standardPage.LanguageDetectionText))
                    //    //    {
                    //    //        var textRequest = _azureTextAnalyticsService.Service.LanguageDetectionExample(standardPage.LanguageDetectionText).Result;
                    //    //        if (!string.IsNullOrWhiteSpace(textRequest.Iso6391Name) && !string.IsNullOrWhiteSpace(textRequest.Name))
                    //    //        {
                    //    //            if (textRequest.Iso6391Name != standardPage.Language.TwoLetterISOLanguageName)
                    //    //            {
                    //    //                e.Content = standardPage;
                    //    //                e.CancelReason = string.Format("Unable to publish content - Azure AI Language has decteted {0} for the content published which is different to the current page language: {1}:  Please review content and publish again",
                    //    //                    textRequest.Name, standardPage.Language.NativeName);
                    //    //                e.CancelAction = true;
                    //    //            }
                    //    //        }
                    //    //    }
                    //    //}

                    //    //if (getStartPage.DetectLanguageTextPage)
                    //    //{
                    //    //    var detectLanguageResults = _azureTextAnalyticsService.Service.LanguageDetectionExamplePage(standardPage).Result;

                    //    //    if (detectLanguageResults != null && detectLanguageResults.Any())
                    //    //    {
                    //    //        var getOccurances = detectLanguageResults.Where(a => a.PrimaryLanguage.Iso6391Name != standardPage.Language.TwoLetterISOLanguageName).Select(a => a.PrimaryLanguage.Name).ToList();
                    //    //        var result = String.Join(", ", getOccurances);
                    //    //        e.Content = standardPage;
                    //    //        e.CancelReason = string.Format("Unable to publish content - Azure AI Language has decteted: {0} for the content published which is different to the current page language: {1}:  Please review content and publish again",
                    //    //            result, standardPage.Language.NativeName);
                    //    //        e.CancelAction = true;
                    //    //    }
                    //    //}

                    //    //if (getStartPage.AbstractText)
                    //    //{
                    //    //    if (!string.IsNullOrWhiteSpace(standardPage.AbstractiveSummarisationText))
                    //    //    {
                    //    //        standardPage.AbstractiveSummarisationList = new List<string>();

                    //    //        var summarisationList = _azureTextAnalyticsService.Service.AbstractiveSummarisation(standardPage.AbstractiveSummarisationText).GetAwaiter().GetResult();
                    //    //        if (summarisationList.Any() && summarisationList != null)
                    //    //        {
                    //    //            if (summarisationList.Count > 1)
                    //    //            {
                    //    //                standardPage.AbstractiveSummarisationList = summarisationList;
                    //    //            }
                    //    //            else
                    //    //            {
                    //    //                standardPage.AbstractiveSummarisationSentence = summarisationList.FirstOrDefault();
                    //    //            }
                    //    //        }
                    //    //    }
                    //    //}

                    //    //if (getStartPage.AbstractTextMultipleProperties)
                    //    //{
                    //    //    standardPage.AbstractiveSummarisationList = new List<string>();

                    //    //    var summarisationList = _azureTextAnalyticsService.Service.AbstractiveSummarisationOfMultipleProperties(standardPage).GetAwaiter().GetResult();
                    //    //    if (summarisationList.Any() && summarisationList != null)
                    //    //    {
                    //    //        if (summarisationList.Count > 1)
                    //    //        {
                    //    //            standardPage.AbstractiveSummarisationList = summarisationList;
                    //    //        }
                    //    //        else
                    //    //        {
                    //    //            standardPage.AbstractiveSummarisationSentence = summarisationList.FirstOrDefault();
                    //    //        }
                    //    //    }
                    //    //}

                    //    //if (getStartPage.AnalyseSentimentText)
                    //    //{
                    //    //    if (!string.IsNullOrWhiteSpace(standardPage.SentimentText))
                    //    //    {
                    //    //        var analyseText = _azureTextAnalyticsService.Service.AnalyseSentimentTextField(standardPage.SentimentText).Result;
                    //    //        if (analyseText == Azure.AI.TextAnalytics.TextSentiment.Mixed)
                    //    //        {
                    //    //            e.Content = standardPage;
                    //    //            e.CancelReason = string.Format("Unable to publish content - Azure AI Language has decteted Mixed Sentiment content : {0} Please review content and publish again", standardPage.SentimentText);
                    //    //            e.CancelAction = true;
                    //    //        }
                    //    //        if (analyseText == Azure.AI.TextAnalytics.TextSentiment.Negative)
                    //    //        {
                    //    //            e.Content = standardPage;
                    //    //            e.CancelReason = string.Format("Unable to publish content - Azure AI Language has decteted Negative Sentiment content : {0} Please review content and publish again", standardPage.SentimentText);
                    //    //            e.CancelAction = true;
                    //    //        }
                    //    //    }
                    //    //}

                    //    //if (getStartPage.AnalyseSentimentTextMultipleProperties)
                    //    //{
                    //    //    var analyseText = _azureTextAnalyticsService.Service.AnalyseSentimentOfMultiplePageProperties(standardPage).Result;

                    //    //    if (analyseText != null && analyseText.Any())
                    //    //    {
                    //    //        var getOccurancesNegative = analyseText.Where(a => a.Sentiment == TextSentiment.Negative).ToList();
                    //    //        var getOccurancesMixed = analyseText.Where(a => a.Sentiment == TextSentiment.Mixed).ToList();
                    //    //        e.Content = standardPage;
                    //    //        e.CancelReason = string.Format("Unable to publish content - Azure AI Language has detected {0} counts of Negative Sentiment and {1} counts of Mixed Sentiment in the Standard Page. Please review content and publish again",
                    //    //            getOccurancesNegative.Count, getOccurancesMixed.Count);
                    //    //        e.CancelAction = true;
                    //    //    }
                    //    //}

                    //    //if (getStartPage.KeyPhrase)
                    //    //{
                    //    //    if (!string.IsNullOrWhiteSpace(standardPage.KeyPhraseText))
                    //    //    {
                    //    //        var keyPhrase = _azureTextAnalyticsService.Service.ExtractKeyPhrasesFromText(standardPage.KeyPhraseText).Result;
                    //    //        if (keyPhrase != null && keyPhrase.Any())
                    //    //        {
                    //    //            standardPage.KeyPhrasesList = keyPhrase;
                    //    //        }
                    //    //    }
                    //    //}

                    //    //if (getStartPage.KeyPhraseCollection)
                    //    //{
                    //    //    var keyPhraseCollection = _azureTextAnalyticsService.Service.ExtractKeyPhrasesFromMultipleProperties(standardPage).Result;
                    //    //    if (keyPhraseCollection != null && keyPhraseCollection.Any())
                    //    //    {
                    //    //        standardPage.KeyPhraseCollectionList = keyPhraseCollection;
                    //    //    }
                    //    //}

                    //    //if (getStartPage.ExtractText)
                    //    //{
                    //    //    if (!string.IsNullOrWhiteSpace(standardPage.ExtractiveSummarisationText))
                    //    //    {
                    //    //        standardPage.ExtractiveSummarisationTextList = new List<string>();
                    //    //        standardPage.ExtractiveSummarisationTextList = _azureTextAnalyticsService.Service.ExtractiveSummarisation(standardPage.ExtractiveSummarisationText).GetAwaiter().GetResult();
                    //    //    }
                    //    //}

                    //    //if (getStartPage.ExtractiveSummarisationPage)
                    //    //{
                    //    //    standardPage.ExtractiveSummarisationTextList = new List<string>();
                    //    //    standardPage.ExtractiveSummarisationTextList = _azureTextAnalyticsService.Service.ExtractiveSummarisationOfMultipleProperties(standardPage).GetAwaiter().GetResult();
                    //    //}

                    //    //if (getStartPage.RecognizeLinkedEntities)
                    //    //{
                    //    //    if (!string.IsNullOrWhiteSpace(standardPage.LinkedEntitiesText))
                    //    //    {
                    //    //        standardPage.LinkedEntitiesTextList = _azureTextAnalyticsService.Service.RecogniseLinkedEntitiesFromText(standardPage.LinkedEntitiesText).GetAwaiter().GetResult();
                    //    //    }
                    //    //}

                    //    //if (getStartPage.RecognizeLinkedEntitiesMultipleProperties)
                    //    //{
                    //    //    standardPage.LinkedEntitiesTextList = _azureTextAnalyticsService.Service.RecogniseLinkedEntitiesFromMultiplePageProperties(standardPage).GetAwaiter().GetResult();
                    //    //}

                    //    //if (getStartPage.HealthcareAnalysisText)
                    //    //{
                    //    //    if (!string.IsNullOrWhiteSpace(standardPage.HealthcareText))
                    //    //    {
                    //    //        standardPage.HealthCareTextList = new List<string>();
                    //    //        var healthCareTextList = _azureTextAnalyticsService.Service.AnalyseHealthcareContentFromText(standardPage.HealthcareText).GetAwaiter().GetResult();
                    //    //        if (getStartPage.AllowHealthCareContent)
                    //    //        {
                    //    //            if (healthCareTextList != null && healthCareTextList.Any())
                    //    //            {
                    //    //                foreach (var healthCare in healthCareTextList)
                    //    //                {
                    //    //                    standardPage.HealthCareTextList.Add(healthCare.Text);
                    //    //                }
                    //    //            }
                    //    //        }
                    //    //        else
                    //    //        {
                    //    //            if (healthCareTextList != null && healthCareTextList.Any())
                    //    //            {
                    //    //                e.Content = standardPage;
                    //    //                e.CancelReason = string.Format("Unable to publish content - Azure AI Language has detected {0} counts of Healthcare related content in the text field 'Healthcare text'. Please review content and publish again",
                    //    //                    healthCareTextList.Count, standardPage.HealthcareText);
                    //    //                e.CancelAction = true;
                    //    //            }
                    //    //        }
                    //    //    }

                    //    //}

                    //    //if (getStartPage.HealthCareAnalysisMultipleProperties)
                    //    //{
                    //    //    standardPage.HealthCareTextList = new List<string>();
                    //    //    var healthCareMultiplePropertiesList = _azureTextAnalyticsService.Service.AnalyseHealthcareContentFromMultipleProperties(standardPage).GetAwaiter().GetResult();
                    //    //    if (getStartPage.AllowHealthCareContent)
                    //    //    {
                    //    //        if (healthCareMultiplePropertiesList != null && healthCareMultiplePropertiesList.Any())
                    //    //        {
                    //    //            standardPage.HealthCareTextList = healthCareMultiplePropertiesList;
                    //    //        }
                    //    //    }
                    //    //    else
                    //    //    {
                    //    //        if (healthCareMultiplePropertiesList != null && healthCareMultiplePropertiesList.Any())
                    //    //        {
                    //    //            e.Content = standardPage;
                    //    //            e.CancelReason = string.Format("Unable to publish content - Azure AI Language has detected {0} counts of Healthcare content across multiple properties on the Standard Page. Please review content and publish again", healthCareMultiplePropertiesList.Count, standardPage.HealthcareText);
                    //    //            e.CancelAction = true;
                    //    //        }
                    //    //    }
                    //    //}
                    //}

                }
            }
            
        }

        public void Uninitialize(InitializationEngine context)
        {
        }

        public void Preload(string[] parameters)
        {
        }
    }
}
