# [DetectLanguage]

The following attribute enables users to incorporate a string property within the CMS, which will undergo Language detection via the Text Analytics API as an integral feature of the Azure AI Language Service. This will happen for each property 
which has this attribute applied to. 

The Language detection feature can detect the language, the content is written in. You can parse the results of this analysis to determine which language is used in the content being published.It can then be compared to the current language branch used on the content type within Optimizely CMS. The return model response also returns a score between 0 and 1 that reflects the confidence of the result.

The attribute may be applied to the following property types:
- **String:** String value indicating the content being used for the Language detection feature.

The attribute can exclusively be added to one or more string properties and is applicable to any content type derived from IContent (the base content type in Optimizely CMS).

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
Screenshot of Attribute being used

