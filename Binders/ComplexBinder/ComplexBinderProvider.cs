using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebBoost.Extensions;

namespace WebBoost.Binders
{
    public sealed class ComplexBinderProvider : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            ActionContext actionContext = bindingContext.ActionContext;
            string[] fromQueryString =  bindingContext.GetBinderAttribute<ComplexBinderAttribute>()?.FromQuery ?? [];

            object? model = Activator.CreateInstance(bindingContext.ModelType);

            string bodyAsJson = await actionContext.HttpContext.ReadBodyAsStringAsync();

            if(!string.IsNullOrEmpty(bodyAsJson))
               Binder.BindBody(bodyAsJson, bindingContext.ModelType, ref model);

            IQueryCollection qs = actionContext.HttpContext.Request.Query;
            IDictionary<string, string> queryString = qs.Where(param => fromQueryString.Contains(param.Key))
                .ToDictionary(qs => qs.Key.ToString(), qs => qs.Value.ToString());

            if (queryString != null && queryString.Any())
                Binder.BindQueryString(queryString, ref model!);

            bindingContext.Model = model;

            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
        }
    }
}
