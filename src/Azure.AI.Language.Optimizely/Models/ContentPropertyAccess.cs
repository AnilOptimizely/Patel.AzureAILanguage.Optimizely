using EPiServer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AzureAIContentSafety.ContentSafety.Models
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
