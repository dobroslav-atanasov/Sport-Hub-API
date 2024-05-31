namespace SportData.WebAPI.Infrastructure.Filters.Swagger;

using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

public class SwaggerTestOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();

        ;


        if (context.ApiDescription.ActionDescriptor is ControllerActionDescriptor descriptor && descriptor.ControllerName.StartsWith("Weather"))
        {
            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Description = "Bearer authorization token.",
                Required = true,
            });
        }
    }
}