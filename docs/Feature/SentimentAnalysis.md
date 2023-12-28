# [SentimentAnalysis]

The following attribute enables users to incorporate a string property within the CMS, which will undergo Sentiment Analysis via the Text Analytics API as an integral feature of the Azure AI Language Service. This will happen for each property which has this attribute applied to. 

Sentiment analysis and opinion mining are features offered by the Azure AI Language service, a collection of machine learning and AI algorithms in the cloud for developing intelligent applications that involve written language. These features help you find out what people think of your brand or topic by mining text for clues about positive or negative sentiment, and can associate them with specific aspects of the text. 

The sentiment analysis feature returns sentiment labels (such as "negative", "neutral" and "positive") based on the highest confidence score found by the service at a sentence and document-level. This feature also will return confidence scores between 0 and 1 for each document & sentences within it for positive, neutral and negative sentiment.

The attribute may be applied to the following property types:
- **String:** String value indicating the content being used for the Sentiment Analysis feature.

The attribute can exclusively be added to one or more string properties and is applicable to any content type derived from IContent (the base content type in Optimizely CMS).

**Example**
``` C#
public class StartPage : SitePageData
{
     [Display(
       GroupName = SystemTabNames.Content,
       Name = "Sentiment Text",
       Description = "Text to be used for the Sentiment Analysis feature as part of the Azure Text Analytics feature",
       Order = 10)]
   [CultureSpecific]
   [UIHint(UIHint.Textarea)]
   [SentimentAnalysis]
   public virtual string SentimentAnalysis { get; set; }
}
```
**Screenshot of Attribute being used in the CMS and returning error for Negative Sentiment**
![Negative Sentiment.](/docs/Images/SentimentNegative.jpg)

**Screenshot of Attribute returning back error for Mixed Sentiment**
![Mixed Sentiment.](/docs/Images/SentimentMixed.jpg)
