using Microsoft.AspNetCore.Mvc.ModelBinding;
using MvcSandbox.Models;
using System.Collections.Generic;

namespace MvcSandbox.ModelBind
{
    public class CSVModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(List<Order>))
            {
                return new CSVModelBinder();
            }
            return null;
        }
    }
}