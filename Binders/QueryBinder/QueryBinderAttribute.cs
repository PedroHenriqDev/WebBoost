using Microsoft.AspNetCore.Mvc;

namespace WebBoost.Binders
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class QueryBinderAttribute : ModelBinderAttribute
    {
        public string[] FromQuery { get; set; }

        public QueryBinderAttribute(params string[] fromQuery) 
        {
            FromQuery = fromQuery ?? [];
            BinderType = typeof(QueryBinderProvider);
        }
    }
}