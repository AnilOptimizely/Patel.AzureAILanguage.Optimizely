using System.Reflection;

namespace Azure.AI.Language.Optimizely.Models
{
    public class ContentProperty
    {
        public object Content { get; set; }
        public PropertyInfo Property { get; set; }
    }
}
