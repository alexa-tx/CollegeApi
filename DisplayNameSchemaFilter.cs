using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel.DataAnnotations;

namespace CollegeApi
{
    public class DisplayNameSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema?.Properties == null || context?.Type == null)
                return;

            foreach (var property in context.Type.GetProperties())
            {
                var displayAttribute = property.GetCustomAttributes(typeof(DisplayAttribute), true)
                                               .FirstOrDefault() as DisplayAttribute;

                if (displayAttribute != null && schema.Properties.ContainsKey(property.Name))
                {
                    schema.Properties[property.Name].Description = displayAttribute.Name;
                }
            }
        }
    }
}
