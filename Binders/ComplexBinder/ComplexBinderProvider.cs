using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace WebBoost.Binders
{
    public sealed class ComplexBinderProvider : IModelBinder
    {

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            ActionContext actionContext = bindingContext.ActionContext;
            string[] fromQueryString = actionContext.ActionDescriptor
                .EndpointMetadata
                .OfType<ComplexBinderAttribute>()
                .FirstOrDefault()
                ?.FromQuery
                ?? [];

            object? model = Activator.CreateInstance(bindingContext.ModelType);

            string bodyAsJson = string.Empty;
            using (var reader = new StreamReader(actionContext.HttpContext.Request.Body, Encoding.UTF8))
            {
                bodyAsJson = reader.ReadToEndAsync().GetAwaiter().GetResult();
            }

            model = typeof(Binder)?.GetMethod(nameof(Binder.BindBody))?.MakeGenericMethod(new Type[] {bindingContext.ModelType }).Invoke(null, [bodyAsJson]);

            IQueryCollection qs = actionContext.HttpContext.Request.Query;
            IDictionary<string, string> queryString = qs.ToDictionary(qs => qs.Key.ToString(), qs => qs.Value.ToString());

            if (queryString != null && queryString.Any())
                Binder.BindQueryString(queryString, ref model);

            bindingContext.Model = model;

            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
            return Task.CompletedTask;
        }
    }
}
