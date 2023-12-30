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
using System.Collections.Generic;
using System.Linq;
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
                                var startString = "Unable to publish - Azure AI Language - Text Analytics has detected the following ";
                                var endString = "Please review content and publish again";
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
                                //Abstractive Summarisation
                                var processAbstractiveSummarisation = OptimizelyCmsHelpers.ProcessAbstractiveSummarisation(content);
                                if (!string.IsNullOrWhiteSpace(processAbstractiveSummarisation))
                                {
                                    listErrorMessages.Add(processAbstractiveSummarisation);
                                }

                                //Key Phrase Summarisation
                                var processKeyPhraseExtraction = OptimizelyCmsHelpers.ProcessKeyPhraseExtraction(content);
                                if (!string.IsNullOrWhiteSpace(processKeyPhraseExtraction))
                                {
                                    listErrorMessages.Add(processKeyPhraseExtraction);
                                }

                                // Extractive Summarisation
                                var processExtractionSummarisation = OptimizelyCmsHelpers.ProcessExtractiveSummarisation(content);
                                if (!string.IsNullOrWhiteSpace(processExtractionSummarisation))
                                {
                                    listErrorMessages.Add(processExtractionSummarisation);
                                }
                                // Linked Entity 
                                var processLinkedEntities = OptimizelyCmsHelpers.ProcessLinkedEntities(content);
                                if (!string.IsNullOrWhiteSpace(processLinkedEntities))
                                {
                                    listErrorMessages.Add(processLinkedEntities);
                                }

                                if (listErrorMessages.Any())
                                {
                                    var errorMessagesString = string.Join(". ", listErrorMessages);
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
