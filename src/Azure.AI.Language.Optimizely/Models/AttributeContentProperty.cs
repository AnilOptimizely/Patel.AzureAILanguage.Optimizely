using Patel.AzureAILanguage.Optimizely.Attributes;
using System.Reflection;

namespace Patel.AzureAILanguage.Optimizely.Models
{
    public class AttributeContentProperty
    {
        public TextAnalyticsBaseContentAttribute Attribute { get; set; }
        public object Content { get; set; }
        public PropertyInfo Property { get; set; }
    }
}
