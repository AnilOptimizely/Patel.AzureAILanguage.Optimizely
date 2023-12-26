# [ExtractionSummarisationList]




The attribute may be applied to the following property types:
- **String:** Liat of  value indicating the content being used for the Extraction Summarisation feature.

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
