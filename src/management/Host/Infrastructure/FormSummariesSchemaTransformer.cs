using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace CascVel.Modules.Evaluations.Management.Host.Infrastructure;

/// <summary>
/// Transforms OpenAPI schema for the form summaries contract to describe the response structure
/// without additional DTO classes.
/// </summary>
internal sealed class FormSummariesSchemaTransformer : IOpenApiSchemaTransformer
{
    /// <summary>
    /// Transforms the schema for the form summaries contract to provide proper OpenAPI documentation.
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

        // Only transform IFormSummaries
        if (context.JsonTypeInfo.Type != typeof(IFormSummaries))
        {
            return Task.CompletedTask;
        }

        // Define the response schema
        schema.Type = JsonSchemaType.Object;
        schema.Description = "Response containing a collection of evaluation form summaries";

        schema.Properties = new Dictionary<string, IOpenApiSchema>
        {
            ["forms"] = new OpenApiSchema
            {
                Type = JsonSchemaType.Array,
                Description = "Collection of evaluation form summaries",
                Items = new OpenApiSchema
                {
                    Type = JsonSchemaType.Object,
                    Description = "Summary information about an evaluation form",
                    Properties = new Dictionary<string, IOpenApiSchema>
                    {
                        ["id"] = new OpenApiSchema
                        {
                            Type = JsonSchemaType.String,
                            Format = "uuid",
                            Description = "Unique identifier of the evaluation form"
                        },
                        ["name"] = new OpenApiSchema
                        {
                            Type = JsonSchemaType.String,
                            Description = "Display name of the evaluation form"
                        },
                        ["description"] = new OpenApiSchema
                        {
                            Type = JsonSchemaType.String,
                            Description = "Detailed description of the form's purpose and usage"
                        },
                        ["code"] = new OpenApiSchema
                        {
                            Type = JsonSchemaType.String,
                            Description = "Unique code identifier for the form"
                        },
                        ["tags"] = new OpenApiSchema
                        {
                            Type = JsonSchemaType.Array,
                            Description = "Collection of tags for categorization and filtering",
                            Items = new OpenApiSchema { Type = JsonSchemaType.String }
                        },
                        ["groupsCount"] = new OpenApiSchema
                        {
                            Type = JsonSchemaType.Integer,
                            Format = "int32",
                            Description = "Number of groups in the form structure"
                        },
                        ["criteriaCount"] = new OpenApiSchema
                        {
                            Type = JsonSchemaType.Integer,
                            Format = "int32",
                            Description = "Number of evaluation criteria in the form"
                        },
                        ["calculation"] = new OpenApiSchema
                        {
                            Type = JsonSchemaType.String,
                            Description = "Calculation method for aggregating scores",
                            Enum = 
                            [
                                "average",
                                "weighted"
                            ]
                        }
                    },
                    Required = new HashSet<string>
                    {
                        "id", "name", "description", "code", "tags",
                        "groupsCount", "criteriaCount", "calculation"
                    }
                }
            }
        };

        schema.Required = new HashSet<string> { "forms" };

        return Task.CompletedTask;
    }
}
