using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebBoost.Extensions;

namespace WebBoost.Binders.HeaderBinder
{
    public sealed class HeaderBinderProvider : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            ActionContext actionContenxt = bindingContext.ActionContext;
            string[] fromHeaders = bindingContext.GetBinderAttribute<HeaderBinderAttribute>()?.Headers ?? [];

            object? model = Activator.CreateInstance(bindingContext.ModelType);

            string bodyAsJson = await actionContenxt.HttpContext.ReadBodyAsStringAsync();
            if (!string.IsNullOrEmpty(bodyAsJson))
                Binder.BindBody(bodyAsJson, bindingContext.ModelType, ref model);
            
            Dictionary<string, string> headers = actionContenxt.HttpContext
                .Request
                .Headers
                .Where(h => fromHeaders.Contains(h.Key))
                .ToDictionary(h => h.Key, h => h.Value.ToString());

            Binder.BindHeader(headers, ref model!);

            bindingContext.Model = model;
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
        }
    }
}
