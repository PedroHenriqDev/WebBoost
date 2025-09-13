using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebBoost.Extensions
{
    public static class BindingContextExtension
    {
        public static T? GetBinderAttribute<T>(this ModelBindingContext bindingContext) where T : ModelBinderAttribute
        {
            ControllerParameterDescriptor? parameter = bindingContext
               .ActionContext
               .ActionDescriptor
               .Parameters
               .OfType<ControllerParameterDescriptor>()
               .FirstOrDefault(p => p.Name == bindingContext.FieldName);

            return parameter?.ParameterInfo
                .GetCustomAttributes(typeof(T), false)
                .Cast<T>()
                .FirstOrDefault();
        }
    }
}
