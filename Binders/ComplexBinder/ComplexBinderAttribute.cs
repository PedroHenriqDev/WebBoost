using Microsoft.AspNetCore.Mvc;

namespace WebBoost.Binders
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class ComplexBinderAttribute : ModelBinderAttribute
    {
        public string[] FromQuery { get; set; }

        public ComplexBinderAttribute(params string[] fromQuery) 
        {
            FromQuery = fromQuery ?? [];
            BinderType = typeof(ComplexBinderProvider);
        }
    }
}