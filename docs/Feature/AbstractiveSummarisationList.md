# [AbstractiveSummarisationList]

The following attribute enables users to see a list of strings which show abstractive summarisations, which have been generated by the Abstractive Summarisation Analysis feature, via the Text Analytics API which is part of the Azure AI Language Service.

The IList string property can be amended following generation of abstractive summarisations. 
However, when content in the string properties, which have the [AbstractiveSummarisation] attribute are published again, 
the IList string property will be regenerated with a list of abstractive summarisations , 
based on the content in the string properties with the [AbstractiveSummarisation] attribute.

Please add only one instance of the [AbstractiveSummarisationList] attribute per content type derived from IContent 
(the base content type in Optimizely CMS).

The attribute may be applied to the following property types:
- **String:** List of String values which show abstractive summarisations, generated from the Abstractive Summarisation Analysis feature , based on content being published in string properties which have the [AbstractiveSummarisation] attribute.

**Example of [AbstractiveSummarisationList] attribute**
``` C#
public class StartPage : SitePageData
{
     [Display(
         GroupName = SystemTabNames.Content,
         Name = "Abstractive Summarisation List",
         Description = "List of strings used for the Abstraction Summarisation feature",
         Order = 10)]
     [CultureSpecific]
     [AbstractiveSummarisationList]
     public virtual IList<string> AbstractiveSummarisationList { get; set; }
}
```

**Screenshot of Attribute being used in the CMS and being populated with abstractive summarisations from the API**

![AbstractiveSummarisationList.](/docs/Images/AbstractiveSummarisationList.jpg)

For more information about the [AbstractiveSummarisation] attribute, click [here](https://github.com/AnilOptimizely/Patel-Azure.AI.Language.Optimizely/blob/develop/docs/Feature/AbstractiveSummarisation.md)
