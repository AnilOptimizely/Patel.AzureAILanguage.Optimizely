# [RecongniseLinkedEntities]

The following attribute enables users to incorporate a string property within the CMS, which will undergo Entity linking Analysis 
via the Text Analytics API as an integral feature of the Azure AI Language Service. This will happen for each property 
which has this attribute applied to.

When content is being published in the CMS, For each [RecongniseLinkedEntities] attribute used, 
the Text Analytics API will indentify and disambiguate the identity of entities found in the content being published. 

The API will return a list of these linked entities which is then used to populate an IList property of type IList string which has a [RecongniseLinkedEntitiesList] attribute assigned to it. It is advised that only one instance of the [RecongniseLinkedEntitiesList] attribute is added to each content type, when the RecongniseLinkedEntities attribute is used

The attribute may be applied to the following property types:

String: String value indicating the content being used for the Entity linking feature.
The attribute can exclusively be added to one or more string properties and is applicable to any content type derived from IContent (the base content type in Optimizely CMS).

**Example of [RecongniseLinkedEntities] attribute**
``` C#
public class StartPage : SitePageData
{
     [Display(
         GroupName = SystemTabNames.Content,
         Name = "Recongnise Linked Entities Text",
         Description = "Text used for the Recongnise Linked Entities feature",
         Order = 10)]
     [CultureSpecific]
     [UIHint(UIHint.Textarea)]
     [RecongniseLinkedEntities]
     public virtual string RecongniseLinkedEntitiesText { get; set; }
}
```
**Example of [RecongniseLinkedEntitiesList] attribute**
``` C#
public class StartPage : SitePageData
{
    [Display(
         GroupName = SystemTabNames.Content,
         Name = "Recongnise Linked Entities List",
         Description = "List used for the Recongnise Linked Entities feature",
         Order = 20)]
     [CultureSpecific]
     [RecongniseLinkedEntitiesList]
     public virtual IList<string> RecongniseLinkedEntitiesList { get; set; }
}
```


Screenshot of Attribute being used

For more information about the [RecongniseLinkedEntitiesList] attribute, click [here](https://github.com/AnilOptimizely/Patel-Azure.AI.Language.Optimizely/blob/develop/docs/Feature/RecongniseLinkedEntitiesList.md)
