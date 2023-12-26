# [HealthcareContent]

The following attribute enables users to incorporate a string property within the CMS, which will undergo Text Analytics for Health Analysis via the Text Analytics API as an integral feature of the Azure AI Language Service. This will happen for each property 
which has this attribute applied to. 

The Text Analytics for Health Analysis can extract medical information from the content which is being published in the CMS.
The [HealthcareContentAllowed] attribute is then checked to determine if Healthcare content is able to be published within the CMS.
If the attribute value is ticked, then the healthcare content will be able to be published in the CMS. If the attribute value is not ticked, the
healthcare content will not be published, with an error being shown the user about this in the CMS.

The attribute may be applied to the following property types:
- **String:** String value indicating the content being used for the Text Analytics for Health Analysis.

The attribute can exclusively be added to one or more string properties and is applicable to any content type derived from IContent (the base content type in Optimizely CMS).

**Example**
``` C#
public class StartPage : SitePageData
{
     [Display(
         GroupName = SystemTabNames.Content,
         Name = "Healthcare Content",
         Description = "Text used for the Healthcare Content",
         Order = 10)]
     [CultureSpecific]
     [UIHint(UIHint.Textarea)]
     [HealthcareContent]
     public virtual string HealthcareContent { get; set; }
}
```
Screenshot of Attribute being used

For more information about the [HealthcareContentAllowed] attribute, click [here](https://github.com/AnilOptimizely/Patel-Azure.AI.Language.Optimizely/blob/develop/docs/Feature/HealthcareContentAllowed.md)
