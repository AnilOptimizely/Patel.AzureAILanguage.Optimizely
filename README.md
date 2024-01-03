# Patel.AzureAILanguage.Optimizely


## Intro

This is an Add-On which integrates Azure AI Language - Text Analytics within Optimizely CMS 12. The Add-On provides users, the features and ability to integrate various functions which are part of the Azure AI Language Service within Optimizely CMS to allow content users to enchance, moderate and leverage content which is being published within the CMS. Some examples of functionality within this Add-On consists of the following.

- [Key Phrase Extraction](https://learn.microsoft.com/en-gb/azure/ai-services/language-service/key-phrase-extraction/overview)
- [Sentiment Analysis](https://learn.microsoft.com/en-gb/azure/ai-services/language-service/sentiment-opinion-mining/overview?tabs=prebuilt)
- [Text Analytics for Healthcare](https://learn.microsoft.com/en-gb/azure/ai-services/language-service/text-analytics-for-health/overview?tabs=ner)
- [Language Detection](https://learn.microsoft.com/en-gb/azure/ai-services/language-service/language-detection/overview)
- [Entity Linking](https://learn.microsoft.com/en-gb/azure/ai-services/language-service/entity-linking/overview)
- [Extractive Summarisation](https://learn.microsoft.com/en-gb/azure/ai-services/language-service/summarization/overview?tabs=document-summarization)
- [Abstractive Summarisation](https://learn.microsoft.com/en-gb/azure/ai-services/language-service/summarization/overview?tabs=document-summarization)

## Installation

```
dotnet add package Patel.AzureAILanguage.Optimizely
```
## Setup

After installing the package, the following steps are required to be done to setup the Add-On correctly.

### Create Azure AI Services Resource
1. Navigate to the Azure Portal by clicking [here](https://portal.azure.com/)
1. Click on create new resource 
1. Search for Azure AI Services
2. Select the option Azure AI Services
3. Fill out details in relation to Project Details (Choose Subscription) and Instance Details (Region/Name/Pricing Tier)
4. Click Create
5. When resource has been created, Navigate to the Keys and Endpoint section. An example screenshot of this is shown below

![ResourceKey.](https://github.com/AnilOptimizely/Patel-Azure.AI.Language.Optimizely/blob/main/docs/Images/AzureAIServicesResourceKeyEndpointInfo.JPG)

7. Make a note of the Key and Endpoint variables - This will be needed in the Configuration section of Setup.

### Configuration

For the Add-On to work, you will have to call the `.AddAzureAILanguageOptimizely()` extension method in the Startup.ConfigureServices method.

Below is a code snippet with all possible configuration options. Using the Key and Endpoint variables which have been retrieved from the Azure AI Resource, populate these details into the 'TextAnalyticsSubscriptionKey' and 'TextAnalyticsEndpoint' variables as shown below

```csharp
.AddAzureAILanguageOptimizely(x => {
    x.TextAnalyticsSubscriptionKey = "************";
    x.TextAnalyticsEndpoint = "******************";
})

```

## Attributes
Please visit [here](https://github.com/AnilOptimizely/Patel-Azure.AI.Language.Optimizely/blob/develop/docs/Attributes.md) to find out more information about the various attributes that are contained within this Add-On and how they work.
