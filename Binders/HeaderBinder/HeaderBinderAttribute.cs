using Microsoft.AspNetCore.Mvc;

namespace WebBoost.Binders.HeaderBinder
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class HeaderBinderAttribute : ModelBinderAttribute
    { 
        public string[] Headers { get; set; }
        
        public HeaderBinderAttribute(params string[] headers)
        {
            Headers = headers ?? [];
            BinderType = typeof(HeaderBinderProvider);
        }
    }
}
