# [HealthcareContentAllowed]

This attribute will allow Healthcare Content to be published in the Optimizely CMS. The Text Analytics for health feature, part of the Azure Text Analytics service, will analyse content related to healthcare to enrich the data
and to provide further insights around it. 

Within this package, it is used to moderate content being published in the CMS. If the bool property is ticked, this will enable healthcare related content to be published. If the bool
property is not ticked, the Text Analytics for Health API will be ran which will process the content as its being published.

It may be applied to the following property types:

- **Bool:** True/false indicating if Image Anaysis is able to be used.
  
The attribute can only be appended to bool properties and is intended for use exclusively on a Start Page. 

**Example**
``` C#
public class StartPage : SitePageData
{
  [Display(GroupName = SystemTabNames.Content,
    Order = 15,
    Description = "Boolean to determine if Healthcare Content for Azure AI Language is allowed",
    Name = "Healthcare Content Allowed")]
  [HealthcareContentAllowed]
  public virtual bool TextAnalyticsLanguageAllowed { get; set; }
}
```

Screenshot of Property being used
