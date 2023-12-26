# [AbstractiveSummarisation]

The following attribute enables users to incorporate a string property within the CMS, which will undergo Abstractive Summarisation Analysis 
via the Text Analytics API as an integral feature of the Azure AI Language Service. This will happen for each property 
which has this attribute applied to.

When content is being published in the CMS, For each [AbstractiveSummarisation] attribute used, 
the Text Analytics API will generate a summary with concise, coherent sentences or words in the content being published. 

The API will return a list of the summarised text, which is then used to populate an IList property of type IList string which 
has a [AbstractiveSummarisationList] attribute assigned to it. 
It is advised that only one instance of the [AbstractiveSummarisationList] attribute is 
added to each content type, when the AbstractiveSummarisation attribute is used

The attribute may be applied to the following property types:

String: String value indicating the content being used for the Abstractive Summarisation Analysis.
The attribute can exclusively be added to one or more string properties and is applicable to any content type derived from IContent (the base content type in Optimizely CMS).

**Example of [AbstractiveSummarisation] attribute**
``` C#
public class StartPage : SitePageData
{
     [Display(
         GroupName = SystemTabNames.Content,
         Name = "Abstractive Summarisation Text",
         Description = "Text used for the Abstraction Summarisation",
         Order = 10)]
     [CultureSpecific]
     [UIHint(UIHint.Textarea)]
     [AbstractiveSummarisation]
     public virtual string AbstractiveSummarisationText { get; set; }
}
```
**Example of [AbstractiveSummarisationList] attribute**
``` C#
public class StartPage : SitePageData
{
    [Display(
         GroupName = SystemTabNames.Content,
         Name = "Abstractive Summarisation List",
         Description = "List of strings used for the Abstraction Summarisation feature",
         Order = 20)]
     [CultureSpecific]
     [AbstractiveSummarisationList]
     public virtual IList<string> AbstractiveSummarisationList { get; set; }
}
```
Screenshot of Attribute being used

For more information about the [AbstractiveSummarisationList] attribute, click [here](https://github.com/AnilOptimizely/Patel-Azure.AI.Language.Optimizely/blob/develop/docs/Feature/AbstractiveSummarisationList.md)
