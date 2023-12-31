# [RecongniseLinkedEntitiesList]

The following attribute enables users to see a list of strings which show linked entities which have been generated by the Entity linking Analysis feature, via the Text Analytics API which is part of the Azure AI Language Service.

The IList string property can be amended following generation of the linked entities. 
However, when content in the string properties, which have the [RecongniseLinkedEntities] attribute are published again, 
the IList string property will be regenerated with a list of linked entities , 
based on the content in the string properties with the [RecongniseLinkedEntities] attribute.

Please add only one instance of the [RecongniseLinkedEntitiesList] attribute per content type derived from IContent 
(the base content type in Optimizely CMS).

The attribute may be applied to the following property types:
- **String:** List of String values which show linked entities generated from the Entity linking Analysis feature , based on content being published in string properties which have the [RecongniseLinkedEntities] attribute.

**Example of [RecongniseLinkedEntitiesList] attribute**
``` C#
public class StartPage : SitePageData
{
   [Display(
         GroupName = SystemTabNames.Content,
         Name = "Linked Entities List",
         Description = "List used for the Recongnise Linked Entities feature",
         Order = 20)]
     [CultureSpecific]
     [RecongniseLinkedEntitiesList]
     public virtual IList<string> RecongniseLinkedEntitiesList { get; set; }
}
```
**Screenshot of Attribute being used in the CMS and being populated with linked entities from the API**

![LinkedEntitiesListAttributePopulatedExampleCMS.](/docs/Images/LinkedEntitiesListAttributePopulatedExampleCMS.jpg)

For more information about the [RecongniseLinkedEntities] attribute, click [here](https://github.com/AnilOptimizely/Patel-Azure.AI.Language.Optimizely/edit/develop/docs/Feature/RecongniseLinkedEntities.md)
