# Patel-Azure.AI.Language.Optimizely

## Intro

This is an Add-On which integrates Azure AI Language - Text Analytics within Optimizely CMS 12. The Add-On provides users, the features and ability to integrate various functions which are part of the Azure AI Language Service within Optimizely CMS to allow content users to enchance, moderate and leverage content which is being published within the CMS. Some examples of functionality within this Add-On consists of

- Key Phrase Extraction
- Sentiment Analysis
- Text Analytics for Healthcare
- Language Detection
- Entity Linking
- Extractive Summarisation
- Abstractive Summarisation

## Installation

```
dotnet add package
```
## Setup

After installing the package, the following steps are required to be done to setup the Add-On correctly.

### Create Azure AI Resource
1. Navigate to the Azure Portal by clicking [here](https://portal.azure.com/)
1. Click on create new resource 
1. Search for Azure AI Content Safety
2. Select the option Azure AI Content Safety
3. Fill out details in relation to Project Details (Choose Subscription) and Instance Details (Region/Name/Pricing Tier)
4. Click Create
5. When resource has been created, Navigate to the Keys and Endpoint section. An example screenshot of this is shown below

![ResourceKey.](https://github.com/AnilOptimizely/Patel-AzureAIContentSafety/blob/main/docs/Features/Configuration/ContentSafetyResourceKeyEndpointInfo.JPG)

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

## More Information

Blog posts added soon







