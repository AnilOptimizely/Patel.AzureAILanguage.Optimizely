# [KeyPhraseExtraction]

The following attribute enables users to incorporate a string property within the CMS, which will undergo Key Phrase Extraction Analysis 
via the Text Analytics API as an integral feature of the Azure AI Language Service. This will happen for each property 
which has this attribute applied to.

When content is being published in the CMS, For each [KeyPhraseExtraction] attribute used, 
the Text Analytics API will identify the main concepts in the content being published. 

The API will return a list of these key phrases, which is then used to populate an IList property of type IList string which has a [KeyPhraseExtractionList] attribute assigned to it. It is advised that only one instance of the [KeyPhraseExtractionList] attribute is added to each content type, when the RecongniseLinkedEntities attribute is used

The attribute may be applied to the following property types:

String: String value indicating the content being used for the Key Phrase Extraction Analysis.
The attribute can exclusively be added to one or more string properties and is applicable to any content type derived from IContent (the base content type in Optimizely CMS).

**Example of [KeyPhraseExtraction] attribute**
``` C#
public class StartPage : SitePageData
{
     [Display(
         GroupName = SystemTabNames.Content,
         Name = "Key Phrase Extraction Text",
         Description = "Text used for the Key Phrase Extraction feature",
         Order = 10)]
     [CultureSpecific]
     [UIHint(UIHint.Textarea)]
     [KeyPhraseExtraction]
     public virtual string KeyPhraseExtractionText { get; set; }
}
```
**Example of [KeyPhraseExtractionList] attribute**
``` C#
public class StartPage : SitePageData
{
    [Display(
         GroupName = SystemTabNames.Content,
         Name = "Key Phrase Extraction List",
         Description = "List used for the Key Phrase Extraction feature",
         Order = 20)]
     [CultureSpecific]
     [KeyPhraseExtractionList]
     public virtual IList<string> KeyPhraseExtractionList { get; set; }
}
```


Screenshot of Attribute being used

For more information about the [KeyPhraseExtractionList] attribute, click [here](https://github.com/AnilOptimizely/Patel-Azure.AI.Language.Optimizely/blob/develop/docs/Feature/KeyPhraseExtractionList.md)
