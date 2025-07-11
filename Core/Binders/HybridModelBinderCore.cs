
using System.Reflection;
using System.Web.Http.ModelBinding;
using Newtonsoft.Json;
using WebBoost.Exceptions;

namespace WebBoost.Core.Binders
{
    public class HybridModelBinderCore
    {
        public static void BindQueryString(IEnumerable<KeyValuePair<string, string>> queryString, ModelBindingContext modelContext)
        {
            object model = modelContext.Model;
            PropertyInfo[] properties = model.GetType().GetProperties();
            
            foreach (KeyValuePair<string, string> kvp in queryString)
            {
                string key = kvp.Key;
                object value = kvp.Value;

                PropertyInfo? property = properties.FirstOrDefault(p => p.Name == key);

                if(property == null)
                    throw new HybridBindNotFoundProperty($"Not found {key} property in {model.GetType} to bind.");

                property.SetValue(model, value);
            }
        }

        public static void BindBody<T>(string bodyAsJson, ModelBindingContext modelContext)
        {
            modelContext.Model = JsonConvert.DeserializeObject<T>(bodyAsJson) ?? new object();
        }
    }
}
