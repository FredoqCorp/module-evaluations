using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace CascVel.Modules.Evaluations.Management.Host.Infrastructure;

/// <summary>
/// Transforms OpenAPI schema for IForm to describe the created form structure.
/// </summary>
internal sealed class FormSchemaTransformer : IOpenApiSchemaTransformer
{
    /// <summary>
    /// Transforms the schema for IForm type to provide OpenAPI documentation.
    /// </summary>
    /// <param name="schema">Schema to transform.</param>
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

        var type = context.JsonTypeInfo.Type;
        if (type != typeof(IForm) && type != typeof(JsonForm))
        {
            return Task.CompletedTask;
        }

        schema.Type = "object";
        schema.Description = "Representation of a created evaluation form aggregate";
        schema.Properties = new Dictionary<string, OpenApiSchema>
        {
            ["formId"] = new()
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
                Description = "Detailed description of the form's purpose"
            },
            ["code"] = new()
            {
                Type = "string",
                Description = "Unique code assigned to the form"
            },
            ["calculation"] = new()
            {
                Type = "string",
                Description = "Calculation mode that determines weighting strategy",
                Enum = new List<Microsoft.OpenApi.Any.IOpenApiAny>
                {
                    new Microsoft.OpenApi.Any.OpenApiString("average"),
                    new Microsoft.OpenApi.Any.OpenApiString("weighted")
                }
            },
            ["tags"] = new()
            {
                Type = "array",
                Description = "Tags applied to categorize the form",
                Items = new OpenApiSchema { Type = "string" }
            },
            ["groups"] = new()
            {
                Type = "array",
                Description = "Collection of hierarchical groups defined in the form",
                Items = new OpenApiSchema
                {
                    Type = "object",
                    Properties = new Dictionary<string, OpenApiSchema>
                    {
                        ["id"] = new()
                        {
                            Type = "string",
                            Format = "uuid",
                            Description = "Unique identifier of the group"
                        },
                        ["formId"] = new()
                        {
                            Type = "string",
                            Format = "uuid",
                            Description = "Identifier of the form that owns the group"
                        },
                        ["parentId"] = new()
                        {
                            Type = "string",
                            Format = "uuid",
                            Nullable = true,
                            Description = "Identifier of the parent group when nested"
                        },
                        ["title"] = new()
                        {
                            Type = "string",
                            Description = "Display title of the group"
                        },
                        ["description"] = new()
                        {
                            Type = "string",
                            Description = "Optional group description"
                        },
                        ["groupType"] = new()
                        {
                            Type = "string",
                            Description = "Group calculation mode aligning with the form",
                            Enum = new List<Microsoft.OpenApi.Any.IOpenApiAny>
                            {
                                new Microsoft.OpenApi.Any.OpenApiString("average"),
                                new Microsoft.OpenApi.Any.OpenApiString("weighted")
                            }
                        },
                        ["orderIndex"] = new()
                        {
                            Type = "integer",
                            Format = "int32",
                            Description = "Display order of the group"
                        },
                        ["weightBasisPoints"] = new()
                        {
                            Type = "integer",
                            Format = "int32",
                            Nullable = true,
                            Description = "Weight of the group expressed in basis points when weighted"
                        }
                    },
                    Required = new HashSet<string>
                    {
                        "id", "formId", "title", "description", "groupType", "orderIndex"
                    }
                }
            },
            ["criteria"] = new()
            {
                Type = "array",
                Description = "Collection of evaluation criteria defined in the form",
                Items = new OpenApiSchema
                {
                    Type = "object",
                    Properties = new Dictionary<string, OpenApiSchema>
                    {
                        ["id"] = new()
                        {
                            Type = "string",
                            Format = "uuid",
                            Description = "Unique identifier of the criterion"
                        },
                        ["formId"] = new()
                        {
                            Type = "string",
                            Format = "uuid",
                            Nullable = true,
                            Description = "Identifier of the form when the criterion is at root level"
                        },
                        ["groupId"] = new()
                        {
                            Type = "string",
                            Format = "uuid",
                            Nullable = true,
                            Description = "Identifier of the parent group when nested"
                        },
                        ["title"] = new()
                        {
                            Type = "string",
                            Description = "Display title of the criterion"
                        },
                        ["text"] = new()
                        {
                            Type = "string",
                            Description = "Detailed text shown to the evaluator"
                        },
                        ["criterionType"] = new()
                        {
                            Type = "string",
                            Description = "Criterion calculation mode aligning with the form",
                            Enum = new List<Microsoft.OpenApi.Any.IOpenApiAny>
                            {
                                new Microsoft.OpenApi.Any.OpenApiString("average"),
                                new Microsoft.OpenApi.Any.OpenApiString("weighted")
                            }
                        },
                        ["orderIndex"] = new()
                        {
                            Type = "integer",
                            Format = "int32",
                            Description = "Display order of the criterion inside its parent"
                        },
                        ["ratingOptions"] = new()
                        {
                            Type = "string",
                            Description = "Serialized JSON describing rating options for the criterion"
                        },
                        ["weightBasisPoints"] = new()
                        {
                            Type = "integer",
                            Format = "int32",
                            Nullable = true,
                            Description = "Weight of the criterion expressed in basis points when weighted"
                        }
                    },
                    Required = new HashSet<string>
                    {
                        "id", "title", "text", "criterionType", "orderIndex", "ratingOptions"
                    }
                }
            }
        };

        schema.Required = new HashSet<string>
        {
            "formId", "name", "description", "code", "calculation", "tags", "groups", "criteria"
        };

        return Task.CompletedTask;
    }
}
