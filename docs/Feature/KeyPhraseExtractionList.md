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
