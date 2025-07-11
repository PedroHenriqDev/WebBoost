using System.Collections.Specialized;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using WebBoost.Core.Binders;

namespace WebBoost.Api.Binders
{
    public class HybridBind : IHybridBinder, IModelBinder
    {
        public string[] FromQueryString { get; set; }

        public HybridBind(params string[] fromQueryString)
        {
            FromQueryString = fromQueryString;
        }

        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            string bodyAsJson = actionContext?.Request?.Content?.ReadAsStringAsync().GetAwaiter().GetResult() ?? string.Empty;
            object model = bindingContext.Model;

            typeof(HybridModelBinderCore)?.GetMethod(nameof(HybridModelBinderCore.BindBody))?.MakeGenericMethod(new Type[] {model.GetType()}).Invoke(null, [bodyAsJson]);

            if (FromQueryString != null && FromQueryString.Any())
            {
                IDictionary<string, string> queryString = actionContext.Request.GetQueryNameValuePairs().ToDictionary(qs => qs.Key, qs => qs.Key);
                HybridModelBinderCore.BindQueryString(queryString, bindingContext);
            }

        }
    }
}
