using Azure.AI.TextAnalytics;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using Patel.AzureAILanguage.Optimizely.Attributes;
using Patel.AzureAILanguage.Optimizely.Interfaces;
using Patel.AzureAILanguage.Optimizely.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Patel.AzureAILanguage.Optimizely.Helpers
{
    public class OptimizelyCmsHelpers
    {
        protected static readonly Injected<IAzureAITextAnalyticsService> _azureTextAnalyticsService;
        protected static readonly Injected<IContentLoader> _contentLoader;

        public static IList<ContentProperty> GetPropertiesWithAttribute(IContent content, Type attribute)
        {
            var pageProperties = GetPagePropertiesWithAttribute(content, attribute);
            var blockProperties = GetBlockPropertiesWithAttribute(content, attribute);
            return pageProperties.Union(blockProperties).ToList();
        }

        public static IList<ContentProperty> GetPagePropertiesWithAttribute(IContent content, Type attribute)
        {
            var getList = content.GetType().GetProperties()
                .Where(pageProperty => Attribute.IsDefined(pageProperty, attribute))
                .Select(property => new ContentProperty { Content = content, Property = property });
            return getList.ToList();
        }

        public static IList<ContentProperty> GetBlockPropertiesWithAttribute(IContent content, Type attribute)
        {
            return content.GetType().GetProperties()
                .Where(pageProperty => typeof(BlockData).IsAssignableFrom(pageProperty.PropertyType))
                .Select(propertyInfo => GetBlockPropertiesWithAttributeForSingleBlock(content, propertyInfo, attribute)).SelectMany(x => x).ToList();
        }

        public static IList<ContentProperty> GetBlockPropertiesWithAttributeForSingleBlock(IContent content, PropertyInfo localBlockProperty, Type attribute)
        {
            var blockPropertiesWithAttribute = localBlockProperty.PropertyType.GetProperties().Where(blockProperty => Attribute.IsDefined(blockProperty, attribute));
            var block = content.Property[localBlockProperty.Name].GetType().GetProperties().Single(x => x.Name == "Block").GetValue(content.Property[localBlockProperty.Name]);
            return blockPropertiesWithAttribute.Select(property => new ContentProperty { Content = block, Property = property }).ToList();
        }

        public static IEnumerable<AttributeContentProperty> GetAttributeContentPropertyList(IEnumerable<ContentProperty> contentProperties)
        {
            if (contentProperties.Any() && contentProperties != null)
            {
                foreach (var contentProperty in contentProperties)
                {
                    var attribute = contentProperty.Property.GetCustomAttributes(typeof(TextAnalyticsBaseContentAttribute)).Cast<TextAnalyticsBaseContentAttribute>().FirstOrDefault();
                    if (attribute != null)
                    {
                        yield return new AttributeContentProperty
                        {
                            Attribute = attribute,
                            Content = contentProperty.Content,
                            Property = contentProperty.Property
                        };
                    }
                }
            }
        }

        public static List<string> ProcessSentimentAnalysis(IContent content)
        {
            var sentimentTextList = new List<string>();
            var listSentiments = new List<TextSentiment>();
            var getSentimentAnalysisAttributes = GetPropertiesWithAttribute(content, typeof(SentimentAnalysisAttribute));
            if (getSentimentAnalysisAttributes.Any() && getSentimentAnalysisAttributes != null)
            {
                foreach (var attribute in getSentimentAnalysisAttributes)
                {
                    var checkTextValue = attribute.Property.GetValue(attribute.Content);
                    if (checkTextValue != null)
                    {
                        var getTextValue = attribute.Property.GetValue(attribute.Content).ToString();
                        var analyseSentiment = _azureTextAnalyticsService.Service.AnalyseSentimentTextField(getTextValue);
                        listSentiments.Add(analyseSentiment);
                    }
                    else
                    {
                        continue;
                    }

                }
            }
            if (listSentiments != null && listSentiments.Any())
            {
                var negativeText = "";
                var mixedText = "";
                var negativeSentiments = listSentiments.Where(a => a == TextSentiment.Negative).ToList();
                var mixedSentiments = listSentiments.Where(a => a == TextSentiment.Mixed).ToList();
                if (negativeSentiments.Any() || mixedSentiments.Any())
                {
                    if (negativeSentiments.Any())
                    {
                        if (negativeSentiments.Count == 1)
                        {
                            negativeText = string.Format("1 count of Negative Sentiment content in the content published");
                            sentimentTextList.Add(negativeText);
                        }
                        if (negativeSentiments.Count > 1)
                        {
                            negativeText = string.Format("{0} counts of Negative Sentiment content in the content published", negativeSentiments.Count);
                            sentimentTextList.Add(negativeText);
                        }
                    }
                    if (mixedSentiments.Any())
                    {
                        if (mixedSentiments.Count == 1)
                        {
                            mixedText = string.Format("1 count of Mixed Sentiment content in the content published");
                            sentimentTextList.Add(mixedText);
                        }
                        if (mixedSentiments.Count > 1)
                        {
                            mixedText = string.Format("{0} counts of Mixed Sentiment related content in the content published", mixedSentiments.Count);
                            sentimentTextList.Add(mixedText);
                        }
                    }
                }
            }
            return sentimentTextList;
        }

        public static List<string> ProcessLanguageDetectionAnalysis(IContent content)
        {
            var languageDetectionTextList = new List<string>();
            var listDetectedLanguages = new List<DetectedLanguage>();
            var getdetectLanguageAttributes = GetPropertiesWithAttribute(content, typeof(DetectLanguageAttribute));
            if (getdetectLanguageAttributes.Any() && getdetectLanguageAttributes != null)
            {
                foreach (var attribute in getdetectLanguageAttributes)
                {
                    var checkTextValue = attribute.Property.GetValue(attribute.Content);
                    if (checkTextValue != null)
                    {
                        var getTextValue = attribute.Property.GetValue(attribute.Content).ToString();
                        var languageDetection = _azureTextAnalyticsService.Service.LanguageDetectionExample(getTextValue);
                        listDetectedLanguages.Add(languageDetection);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            if (listDetectedLanguages != null && listDetectedLanguages.Any())
            {
                if (!ContentReference.IsNullOrEmpty(content.ContentLink))
                {
                    var getPage = _contentLoader.Service.Get<IContent>(content.ContentLink);

                    if (!ContentReference.IsNullOrEmpty(getPage.ContentLink))
                    {
                        if (getPage is PageData sitePageData)
                        {
                            var getLanguageOccurances = listDetectedLanguages.Where(a => a.Iso6391Name != sitePageData.Language.TwoLetterISOLanguageName).Select(a => a.Name).ToList();

                            if (getLanguageOccurances.Any())
                            {
                                var result = string.Join(", ", getLanguageOccurances);
                                languageDetectionTextList.Add(result);
                                var middleMessage = "for the content published which is different to the current page language:";
                                languageDetectionTextList.Add(middleMessage);
                                languageDetectionTextList.Add(sitePageData.Language.NativeName);
                            }
                        }
                    }
                }
            }
            return languageDetectionTextList;
        }

        public static bool DetermineHealthcareContentAllowed(ContentProperty contentProperty)
        {
            var healthcareAllowed = false;
            var getAllowedHealthcareAllowedValue = contentProperty.Property.GetValue(contentProperty.Content);
            if (getAllowedHealthcareAllowedValue is bool v)
            {
                healthcareAllowed = v;
            }
            return healthcareAllowed;
        }

        public static string ProcessHealthcareContentAnalysis(IContent content)
        {
            var listHealthcareContent = new List<int>();
            var healthcareContentMessage = "";
            var getHealthcareContentAttributes = GetPropertiesWithAttribute(content, typeof(HealthcareContentAttribute));
            if (getHealthcareContentAttributes.Any() && getHealthcareContentAttributes != null)
            {
                foreach (var attribute in getHealthcareContentAttributes)
                {
                    var checkTextValue = attribute.Property.GetValue(attribute.Content);
                    if (checkTextValue != null)
                    {
                        var getTextValue = attribute.Property.GetValue(attribute.Content).ToString();
                        var healthCareContentList = _azureTextAnalyticsService.Service.ProcessHealthcareContentFromText(getTextValue);
                        listHealthcareContent.Add(healthCareContentList.Count);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            if (listHealthcareContent != null && listHealthcareContent.Any())
            {
                int res = listHealthcareContent.AsQueryable().Sum();
                if (res == 1)
                {
                    healthcareContentMessage = " 1 count of Healthcare related content";
                }
                if (res > 1)
                {
                    healthcareContentMessage = string.Format(" {0} counts of Healthcare related content", res);
                }
            }
            return healthcareContentMessage;
        }

        public static string ProcessAbstractiveSummarisation(IContent content)
        {
            var getAbstractiveSummarisationAttributes = GetPropertiesWithAttribute(content, typeof(AbstractiveSummarisationAttribute));
            var getAbstractiveSummarisationListAttribute = GetPropertiesWithAttribute(content, typeof(AbstractiveSummarisationListAttribute));
            var listStringsForAbstractiveSummarisation = new List<string>();

            if (getAbstractiveSummarisationAttributes.Any() && getAbstractiveSummarisationAttributes != null)
            {
                if (!getAbstractiveSummarisationListAttribute.Any() && getAbstractiveSummarisationListAttribute == null)
                {
                    return "Please create a IList<String> property with attribute TextAnalyticsAllowed in order to process";
                }
                foreach (var attribute in getAbstractiveSummarisationAttributes)
                {
                    var checkTextValue = attribute.Property.GetValue(attribute.Content);
                    if (checkTextValue != null)
                    {
                        var getTextValue = attribute.Property.GetValue(attribute.Content).ToString();
                        if (!string.IsNullOrWhiteSpace(getTextValue))
                        {
                            listStringsForAbstractiveSummarisation.Add(getTextValue);
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                if (listStringsForAbstractiveSummarisation.Any() && listStringsForAbstractiveSummarisation != null)
                {
                    var listAbstractiveSummarisations = _azureTextAnalyticsService.Service.AbstractiveSummarisation(listStringsForAbstractiveSummarisation).Result;
                    if (listAbstractiveSummarisations.Any() && listAbstractiveSummarisations != null)
                    {
                        var getPropertyContent = getAbstractiveSummarisationListAttribute.Where(a => a.Content == content).FirstOrDefault();
                        if (getPropertyContent != null)
                        {
                            getPropertyContent.Property.SetValue(getPropertyContent.Content, listAbstractiveSummarisations);
                            var getObject = getPropertyContent.Property.GetValue(getPropertyContent.Content);
                            var checkObject = getObject.GetType().GetProperties().Any();
                            return checkObject ? "" : "Error with saving the Key phrase extraction. Please try again";
                        }
                    }
                }
            }
            return "";
        }

        public static string ProcessKeyPhraseExtraction(IContent content)
        {
            var getKeyPhraseExtractionAttributes = GetPropertiesWithAttribute(content, typeof(KeyPhraseExtractionAttribute));
            var getKeyPhraseExtractionListAttribute = GetPropertiesWithAttribute(content, typeof(KeyPhraseExtractionListAttribute));

            var listStringsForKeyPhraseExtraction = new List<string>();

            if (getKeyPhraseExtractionAttributes.Any() && getKeyPhraseExtractionAttributes != null)
            {
                if (!getKeyPhraseExtractionListAttribute.Any() && getKeyPhraseExtractionListAttribute == null)
                {
                    return "Please create a IList<String> property with attribute TextAnalyticsAllowed in order to process";
                }

                foreach (var attribute in getKeyPhraseExtractionAttributes)
                {
                    var checkTextValue = attribute.Property.GetValue(attribute.Content);
                    if (checkTextValue != null)
                    {
                        var getTextValue = attribute.Property.GetValue(attribute.Content).ToString();
                        if (!string.IsNullOrWhiteSpace(getTextValue))
                        {
                            listStringsForKeyPhraseExtraction.Add(getTextValue);
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                if (listStringsForKeyPhraseExtraction.Any() && listStringsForKeyPhraseExtraction != null)
                {
                    var keyPhraseExtractionList = _azureTextAnalyticsService.Service.ExtractKeyPhrasesFromText(listStringsForKeyPhraseExtraction);
                    if (keyPhraseExtractionList.Any() && keyPhraseExtractionList != null)
                    {
                        var getPropertyContent = getKeyPhraseExtractionListAttribute.Where(a => a.Content == content).FirstOrDefault();
                        if (getPropertyContent != null)
                        {
                            getPropertyContent.Property.SetValue(getPropertyContent.Content, keyPhraseExtractionList);
                            var getObject = getPropertyContent.Property.GetValue(getPropertyContent.Content);
                            var checkObject = getObject.GetType().GetProperties().Any();
                            return checkObject ? "" : "Error with saving the Key phrase extraction. Please try again";
                        }
                    }
                }
            }
            return "";
        }

        public static string ProcessExtractiveSummarisation(IContent content)
        {
            var getExtractionSummarisationAttributes = GetPropertiesWithAttribute(content, typeof(ExtractionSummarisationAttribute));
            var getExtractionSummarisationListAttribute = GetPropertiesWithAttribute(content, typeof(ExtractionSummarisationListAttribute));
            var listStringsForExtractionSummarisation = new List<string>();

            if (getExtractionSummarisationAttributes.Any() && getExtractionSummarisationAttributes != null)
            {
                if (!getExtractionSummarisationListAttribute.Any() && getExtractionSummarisationListAttribute == null)
                {
                    return "Please create a IList<String> property with attribute TextAnalyticsAllowed in order to process";
                }
                foreach (var attribute in getExtractionSummarisationAttributes)
                {
                    var checkTextValue = attribute.Property.GetValue(attribute.Content);
                    if (checkTextValue != null)
                    {
                        var getTextValue = attribute.Property.GetValue(attribute.Content).ToString();
                        if (!string.IsNullOrWhiteSpace(getTextValue))
                        {
                            listStringsForExtractionSummarisation.Add(getTextValue);
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                if (listStringsForExtractionSummarisation.Any() && listStringsForExtractionSummarisation != null)
                {
                    var extractionSummarisationList = _azureTextAnalyticsService.Service.ProcessExtractiveSummarisation(listStringsForExtractionSummarisation);
                    if (extractionSummarisationList.Any() && extractionSummarisationList != null)
                    {
                        var getPropertyContent = getExtractionSummarisationListAttribute.Where(a => a.Content == content).FirstOrDefault();
                        if (getPropertyContent != null)
                        {
                            getPropertyContent.Property.SetValue(getPropertyContent.Content, extractionSummarisationList);
                            var getObject = getPropertyContent.Property.GetValue(getPropertyContent.Content);
                            var checkObject = getObject.GetType().GetProperties().Any();
                            return checkObject ? "" : "Error with saving the Key phrase extraction. Please try again";
                        }
                    }
                }
            }
            return "";
        }

        public static string ProcessLinkedEntities(IContent content)
        {
            var getLinkedEntityAttributes = GetPropertiesWithAttribute(content, typeof(RecongniseLinkedEntitiesAttribute));
            var getLinkedEntityListAttribute = GetPropertiesWithAttribute(content, typeof(RecongniseLinkedEntitiesListAttribute));
            var listStringsForLinkedEntities = new List<string>();

            if (getLinkedEntityAttributes.Any() && getLinkedEntityAttributes != null)
            {
                if (!getLinkedEntityListAttribute.Any() && getLinkedEntityListAttribute == null)
                {
                    return "Please create a IList<String> property with attribute TextAnalyticsAllowed in order to process";
                }
                foreach (var attribute in getLinkedEntityAttributes)
                {
                    var checkTextValue = attribute.Property.GetValue(attribute.Content);
                    if (checkTextValue != null)
                    {
                        var getTextValue = attribute.Property.GetValue(attribute.Content).ToString();
                        if (!string.IsNullOrWhiteSpace(getTextValue))
                        {
                            listStringsForLinkedEntities.Add(getTextValue);
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                if (listStringsForLinkedEntities.Any() && listStringsForLinkedEntities != null)
                {
                    var extractionSummarisationList = _azureTextAnalyticsService.Service.RecogniseLinkedEntitiesFromText(listStringsForLinkedEntities);
                    if (extractionSummarisationList.Any() && extractionSummarisationList != null)
                    {
                        var getPropertyContent = getLinkedEntityListAttribute.Where(a => a.Content == content).FirstOrDefault();
                        if (getPropertyContent != null)
                        {
                            getPropertyContent.Property.SetValue(getPropertyContent.Content, extractionSummarisationList);
                            var getObject = getPropertyContent.Property.GetValue(getPropertyContent.Content);
                            var checkObject = getObject.GetType().GetProperties().Any();
                            return checkObject ? "" : "Error with saving the Key phrase extraction. Please try again";
                        }
                    }
                }
            }
            return "";
        }
    }
}
