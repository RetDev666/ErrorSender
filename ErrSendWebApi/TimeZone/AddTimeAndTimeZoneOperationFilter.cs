using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ErrSendWebApi.TimeZone
{
    public class AddTimeAndTimeZoneOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Responses.ContainsKey("200"))
            {
                var content = new OpenApiObject
                {
                    ["time"] = new OpenApiString(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                    ["timezone"] = new OpenApiString(TimeZoneInfo.FindSystemTimeZoneById("Europe/Kiev").DisplayName)
                };

                operation.Responses["200"].Content["application/json"].Example = content;
            }
        }
    }
}
