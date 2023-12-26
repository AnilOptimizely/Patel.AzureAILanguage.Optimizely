# [RecongniseLinkedEntities]

The following attribute enables users to incorporate a string property within the CMS, which will undergo Entity linking Analysis 
via the Text Analytics API as an integral feature of the Azure AI Language Service. This will happen for each property 
which has this attribute applied to.

When content is being published in the CMS, For each [RecongniseLinkedEntities] attribute used, 
the Text Analytics API will return a list of by extracting sentences that collectively represent the most important or relevant information within the original content.

The summary is then used to populate an IList property of type IList string which has a [ExtractionSummarisationList] attribute assigned to it. It is advised that only one instance of the [ExtractionSummarisationList] attribute is added to each content type, when the ExtractionSummarisation attribute is used

The attribute may be applied to the following property types:

String: String value indicating the content being used for the Extraction Summarisation feature.
The attribute can exclusively be added to one or more string properties and is applicable to any content type derived from IContent (the base content type in Optimizely CMS).

**Example of [ExtractionSummarisation] attribute**
``` C#
public class StartPage : SitePageData
{
     [Display(
    GroupName = SystemTabNames.Content,
    Name = "Extractive Summarisation Text",
    Description = "Text used for the Extraction Summarisation feature",
    Order = 10)]
[CultureSpecific]
[UIHint(UIHint.Textarea)]
[ExtractionSummarisation]
public virtual string ExtractiveSummarisationText { get; set; }
}
```
**Example of [ExtractionSummarisationList] attribute**
``` C#
public class StartPage : SitePageData
{
    [Display(
    GroupName = SystemTabNames.Content,
    Name = "Extractive Summarisation List",
    Description = "List used for the Extraction Summarisation feature",
    Order = 15)]
[CultureSpecific]
[ExtractionSummarisationList]
public virtual IList<string> ExtractiveSummarisationList { get; set; }
}
```


Screenshot of Attribute being used

For more information about the [ExtractionSummarisationList] attribute, click [here](https://github.com/AnilOptimizely/Patel-Azure.AI.Language.Optimizely/edit/develop/docs/Feature/ExtractionSummarisationList.md)
