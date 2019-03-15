using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;

namespace Training_App.MyFilter
{
    public class BaseOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            operation.parameters = new List<Parameter>();

            operation.parameters.Add(new Parameter()
            {
                name = "AUTHENTICATION",
                @in = "header",
                description = "Authentication Token",
                type = "string",
                required = false
            });
        }
    }
}