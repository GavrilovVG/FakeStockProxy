using FakeStockProxy.Application.Miscellaneous.Json.Attributes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace FakeStockProxy.Web.Swagger;

public class SwaggerJsonIgnoreFilter : IOperationFilter
{

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var ignoredProperties = context.MethodInfo.GetParameters()
            .SelectMany(p => p.ParameterType.GetProperties()
                             .Where(prop => prop.GetCustomAttribute<SwaggerJsonIgnore>() != null));

        if (ignoredProperties.Any())
        {
            foreach (var property in ignoredProperties)
            {
                operation.Parameters = operation.Parameters
                    .Where(p => !p.Name.Equals(property.Name, StringComparison.OrdinalIgnoreCase)
                    && !p.Name.StartsWith(property.Name + ".", StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

        }
    }
}
