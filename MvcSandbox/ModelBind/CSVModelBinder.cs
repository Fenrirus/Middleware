using Microsoft.AspNetCore.Mvc.ModelBinding;
using MvcSandbox.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MvcSandbox.ModelBind
{
    public class CSVModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var rawCSV = bindingContext.ValueProvider.GetValue("csv").ToString();
            var orderListCSV = rawCSV.Split(Environment.NewLine.ToCharArray());

            var createOrdersList = new List<Order>();
            foreach (var order in orderListCSV)
            {
                var orderValues = order.Split(",");

                var newOrder = new Order()
                {
                    ProductName = orderValues[0],
                    Count = orderValues[1],
                    Description = orderValues[2]
                };
                createOrdersList.Add(newOrder);
            }

            bindingContext.Result = ModelBindingResult.Success(createOrdersList);
            return Task.CompletedTask;
        }
    }
}