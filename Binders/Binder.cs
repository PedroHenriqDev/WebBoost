using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Reflection;
using System.Security.AccessControl;
using WebBoost.Exceptions;

namespace WebBoost.Binders
{
    public sealed class Binder
    {
        public static void BindQueryString(IEnumerable<KeyValuePair<string, string>> queryString, ref object modelObj)
        {
            PropertyInfo[] properties = modelObj.GetType().GetProperties();

            foreach (KeyValuePair<string, string> kvp in queryString)
            {
                string key = kvp.Key;
                string value = kvp.Value;

                PropertyInfo? property = properties.FirstOrDefault(p => p.Name == key);

                if (property == null)
                    throw new ComplexBindNotFoundPropertyException($"Not found {key} property in {modelObj.GetType} to bind.");

                property.SetValue(modelObj, ConvertTo(value, property.PropertyType));
            }
        }

        public static object? ConvertTo(string value, Type type)
        {
            if (type == typeof(string)) return value;

            TypeConverter? converter = TypeDescriptor.GetConverter(type);
            if(converter is not null && converter.CanConvertFrom(typeof(string)))
                return converter.ConvertFromInvariantString(value);

            if(type.IsEnum)
                return Enum.Parse(type, value, ignoreCase: true);

            throw new InvalidOperationException($"Cannot possible convert '{value}' to {type.Name}.");
        }

        public static object BindBody<T>(string bodyAsJson)
        {
            return JsonConvert.DeserializeObject<T>(bodyAsJson) ?? new object();
        }
    }
}
