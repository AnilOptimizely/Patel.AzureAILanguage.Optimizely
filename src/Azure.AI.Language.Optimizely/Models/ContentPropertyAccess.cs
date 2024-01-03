using EPiServer.Core;
using System.Reflection;

namespace Patel.AzureAILanguage.Optimizely.Models
{
    public class ContentPropertyAccess
    {
        private readonly object _content;

        public ContentPropertyAccess(IContent contentType, object content, PropertyInfo propertyInfo)
        {
            _content = content;
            ContentType = contentType;
            Property = propertyInfo;
        }

        public void SetValue(object value)
        {
            Property.SetValue(_content, value);
        }

        public object GetValue()
        {
            return Property.GetValue(_content);
        }

        public IContent ContentType { get; }

        public PropertyInfo Property { get; }
    }
}
