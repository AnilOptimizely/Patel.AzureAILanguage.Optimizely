using AzureAIContentSafety.ContentSafety.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Azure.AI.Language.Optimizely.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class TextAnalyticsBaseContentAttribute : Attribute
    {
        //public abstract void UpdateContent(AnalyzeTextResult analyseContentSafetyTextResult,
        //    AzureAIContentSafetyService azureAIContentSafetyService, ContentPropertyAccess contentPropertyAccess);

        public virtual bool AnalyzeCMSContent => false;

        protected static bool IsBooleanProperty(PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType == typeof(bool);
        }
        protected static bool IsIntProperty(PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType == typeof(int);
        }

        protected static bool IsDoubleProperty(PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType == typeof(double);
        }

        protected static bool IsStringProperty(PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType == typeof(string);
        }

        protected static bool IsStringListProperty(PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType == typeof(IList<string>) ||
                   propertyInfo.PropertyType == typeof(IEnumerable<string>) ||
                   propertyInfo.PropertyType == typeof(ICollection);
        }
    }
}
