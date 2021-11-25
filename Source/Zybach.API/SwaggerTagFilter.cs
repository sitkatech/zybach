using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Zybach.API
{
    public class SwaggerTagFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (var contextApiDescription in context.ApiDescriptions)
            {
                var actionDescriptor = (ControllerActionDescriptor)contextApiDescription.ActionDescriptor;

                if (!actionDescriptor.ControllerTypeInfo.GetCustomAttributes<SwaggerTagAttribute>().Any() &&
                    !actionDescriptor.MethodInfo.GetCustomAttributes<SwaggerTagAttribute>().Any())
                {
                    var key = $"/{contextApiDescription.RelativePath.TrimEnd('/')}";
                    swaggerDoc.Paths.Remove(key);
                }
            }
        }
    }
}