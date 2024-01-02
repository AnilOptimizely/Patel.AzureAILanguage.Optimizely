using Azure.AI.Language.Optimizely.Attributes;
using System.Reflection;

namespace AzureAIContentSafety.ContentSafety.Models
{
    public class AttributeContentProperty
    {
        public TextAnalyticsBaseContentAttribute Attribute { get; set; }
        public object Content { get; set; }
        public PropertyInfo Property { get; set; }
    }
}
