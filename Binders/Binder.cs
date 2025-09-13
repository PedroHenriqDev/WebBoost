using Newtonsoft.Json;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
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
        
        public static void BindHeader(IEnumerable<KeyValuePair<string, string>> headers, ref object modelObj)
        {
            PropertyInfo[] properties = modelObj.GetType().GetProperties();

            foreach (KeyValuePair<string, string> kvp in headers)
            {
                string key = kvp.Key;
                string value = kvp.Value;

                PropertyInfo? property = properties.FirstOrDefault(p => p.Name == key);

                if (property == null)
                    throw new ComplexBindNotFoundPropertyException($"Not found {key} property in {modelObj.GetType} to bind.");

                property.SetValue(modelObj, ConvertTo(value, property.PropertyType));
            }
        }

        public static void BindBody(string bodyAsJson, Type modelType, ref object? modelObj)
        {
            MethodInfo method = typeof(Binder).GetMethod(nameof(ConvertBody), BindingFlags.NonPublic | BindingFlags.Static)!;
            MethodInfo genericMethod = method.MakeGenericMethod(modelType);
            object? model = genericMethod.Invoke(null, [bodyAsJson]);
            
            if (model == null)
                throw new ArgumentNullException("An error ocurred in BindBody");

            modelObj = model;
        }

        private static object ConvertBody<T>(string bodyAsJson)
        {
            return JsonConvert.DeserializeObject<T>(bodyAsJson) ?? new object();
        }

        public static object? ConvertTo(string value, Type type)
        {
            if (type == typeof(string)) return value;

            TypeConverter? converter = TypeDescriptor.GetConverter(type);
            if (converter is not null && converter.CanConvertFrom(typeof(string)))
                return converter.ConvertFromInvariantString(value);

            if (type.IsEnum)
                return Enum.Parse(type, value, ignoreCase: true);

            throw new InvalidOperationException($"Cannot possible convert '{value}' to {type.Name}.");
        }

    }
}
