# [TextAnalyticsAllowed]

This attribute will utilise the Text Analytics feature, a key part of the Azure AI Language service, to analyse text for various use cases, during the publishing of content within Optimizely CMS. These use cases are covered in 
the other Readme files and functionality within the package such as:

+ Abstractive Summarisation
+ Language Detection
+ Extractive Summarisation
+ Healthcare Content
+ Key Phrase Extraction
+ Recongnise Linked Entities
+ Sentiment Analysis

The attribute may be applied to the following property types:

- **Bool:** True/false indicating if Text Analytics is able to be used.

The attribute can only be appended to bool properties and is intended for use exclusively on a Start Page. 

**Example**
``` C#
public class StartPage : SitePageData
{
  [Display(GroupName = SystemTabNames.Content,
    Order = 10,
    Description = "Boolean to determine if Text Analytics API for Azure AI Language is allowed",
    Name = "Text Analytics Allowed")]
[TextAnalyticsAllowed]
public virtual bool TextAnalyticsAllowed { get; set; }
}
```
**Example of boolean property being used in the CMS with TextAnalyticsAllowed attribute used**
![TextAnalyticsAllowed](https://github.com/AnilOptimizely/Patel-Azure.AI.Language.Optimizely/blob/main/docs/Images/AIAzureLanguageBooleans.JPG)
