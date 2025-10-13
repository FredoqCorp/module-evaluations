using CascVel.Modules.Evaluations.Management.Application.UseCases.ListForms;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace CascVel.Modules.Evaluations.Management.Host.Infrastructure;

/// <summary>
/// Transforms OpenAPI schema for ListFormsResponse to describe the response structure
/// without requiring separate DTO classes.
/// </summary>
internal sealed class ListFormsResponseSchemaTransformer : IOpenApiSchemaTransformer
{
    /// <summary>
    /// Transforms the schema for ListFormsResponse type to provide proper OpenAPI documentation.
    /// </summary>
    /// <param name="schema">The schema to transform.</param>
    /// <param name="context">Context containing type information.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Completed task.</returns>
    public Task TransformAsync(
        OpenApiSchema schema,
        OpenApiSchemaTransformerContext context,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(schema);
        ArgumentNullException.ThrowIfNull(context);

        // Only transform ListFormsResponse
        if (context.JsonTypeInfo.Type != typeof(ListFormsResponse))
        {
            return Task.CompletedTask;
        }

        // Define the response schema
        schema.Type = "object";
        schema.Description = "Response containing a collection of evaluation form summaries";

        schema.Properties = new Dictionary<string, OpenApiSchema>
        {
            ["forms"] = new()
            {
                Type = "array",
                Description = "Collection of evaluation form summaries",
                Items = new OpenApiSchema
                {
                    Type = "object",
                    Description = "Summary information about an evaluation form",
                    Properties = new Dictionary<string, OpenApiSchema>
                    {
                        ["id"] = new()
                        {
                            Type = "string",
                            Format = "uuid",
                            Description = "Unique identifier of the evaluation form"
                        },
                        ["name"] = new()
                        {
                            Type = "string",
                            Description = "Display name of the evaluation form"
                        },
                        ["description"] = new()
                        {
                            Type = "string",
                            Description = "Detailed description of the form's purpose and usage"
                        },
                        ["code"] = new()
                        {
                            Type = "string",
                            Description = "Unique code identifier for the form"
                        },
                        ["tags"] = new()
                        {
                            Type = "array",
                            Description = "Collection of tags for categorization and filtering",
                            Items = new OpenApiSchema { Type = "string" }
                        },
                        ["groupsCount"] = new()
                        {
                            Type = "integer",
                            Format = "int32",
                            Description = "Number of groups in the form structure"
                        },
                        ["criteriaCount"] = new()
                        {
                            Type = "integer",
                            Format = "int32",
                            Description = "Number of evaluation criteria in the form"
                        },
                        ["calculationType"] = new()
                        {
                            Type = "string",
                            Description = "Calculation method for aggregating scores",
                            Enum = new List<Microsoft.OpenApi.Any.IOpenApiAny>
                            {
                                new Microsoft.OpenApi.Any.OpenApiString("Average"),
                                new Microsoft.OpenApi.Any.OpenApiString("WeightedAverage")
                            }
                        }
                    },
                    Required = new HashSet<string>
                    {
                        "id", "name", "description", "code", "tags",
                        "groupsCount", "criteriaCount", "calculationType"
                    }
                }
            }
        };

        schema.Required = new HashSet<string> { "forms" };

        return Task.CompletedTask;
    }
}
