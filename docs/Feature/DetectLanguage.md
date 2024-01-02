# [DetectLanguage]

The following attribute enables users to incorporate a string property within the CMS, which will undergo Language detection via the Text Analytics API as an integral feature of the Azure AI Language Service. This will happen for each property 
which has this attribute applied to. 

Language detection is one of the features offered by Azure AI Language as part of Azure Text Analytics. The feature is built on a collection of machine learning and AI algorithms in the cloud for developing intelligent applications that involve written language. The Language detection feature can detect the language the content is written in before being published. It can then be compared to the current language branch used on a page type within Optimizely CMS. The return model response also returns a score between 0 and 1 that reflects the confidence of the result.

The attribute may be applied to the following property types:
- **String:** String value indicating the content being used for the Language detection feature.

The attribute can exclusively be added to one or more string properties which are on CMS page types, which are derived from IContent (the base content type in Optimizely CMS).  

**Example**
``` C#
public class StartPage : SitePageData
{
     [Display(
         GroupName = SystemTabNames.Content,
         Name = "Language Detection Text",
         Description = "Text used for the Language Detection",
         Order = 10)]
     [CultureSpecific]
     [UIHint(UIHint.Textarea)]
     [DetectLanguage]
     public virtual string LanguageDetectionText { get; set; }
}
```
**Screenshot of Attribute being used in the CMS**
![LanguageDetectionCMS.](https://github.com/AnilOptimizely/Patel-Azure.AI.Language.Optimizely/blob/main/docs/Images/LanguageDetectionCMS.jpg)

**Screenshot of Attribute returning back error for one language**
![LanguageDetectionCMSError.](/docs/Images/LanguageDetectionCMSError.jpg)

**Screenshot of Attribute returning back error for multiple languages**
![LanguageDetectionCMSErrorMultipleLanguage.](/docs/Images/LanguageDetectionCMSErrorMultipleLanguages.jpg)
