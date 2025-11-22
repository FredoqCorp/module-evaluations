using System.Text.Json.Nodes;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Host.Models.Forms;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

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
        if (type != typeof(IForm) && type != typeof(JsonNewForm))
        {
            return Task.CompletedTask;
        }

        schema.Type = JsonSchemaType.String;
        schema.Description = "Representation of a created evaluation form aggregate";
        schema.Properties = new Dictionary<string, IOpenApiSchema>
        {
            ["formId"] = new OpenApiSchema
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
                Description = "Detailed description of the form's purpose"
            },
            ["code"] = new OpenApiSchema
            {
                Type = JsonSchemaType.String,
                Description = "Unique code assigned to the form"
            },
            ["calculation"] = new OpenApiSchema
            {
                Type = JsonSchemaType.String,
                Description = "Calculation mode that determines weighting strategy",
                Enum =
                [
                    "average",
                    "weighted"
                ]
            },
            ["tags"] = new OpenApiSchema
            {
                Type = JsonSchemaType.Array,
                Description = "Tags applied to categorize the form",
                Items = new OpenApiSchema { Type = JsonSchemaType.String }
            },
            ["groups"] = new OpenApiSchema
            {
                Type = JsonSchemaType.Array,
                Description = "Collection of hierarchical groups defined in the form",
                Items = new OpenApiSchema
                {
                    Type = JsonSchemaType.Object,
                    Properties = new Dictionary<string, IOpenApiSchema>
                    {
                        ["id"] = new OpenApiSchema
                        {
                            Type = JsonSchemaType.String,
                            Format = "uuid",
                            Description = "Unique identifier of the group"
                        },
                        ["formId"] = new OpenApiSchema
                        {
                            Type = JsonSchemaType.String,
                            Format = "uuid",
                            Description = "Identifier of the form that owns the group"
                        },
                        ["parentId"] = new OpenApiSchema
                        {
                            Type = JsonSchemaType.String,
                            Format = "uuid",
                            //Nullable = true,
                            Description = "Identifier of the parent group when nested"
                        },
                        ["title"] = new OpenApiSchema
                        {
                            Type = JsonSchemaType.String,
                            Description = "Display title of the group"
                        },
                        ["description"] = new OpenApiSchema
                        {
                            Type = JsonSchemaType.String,
                            Description = "Optional group description"
                        },
                        ["orderIndex"] = new OpenApiSchema
                        {
                            Type = JsonSchemaType.Integer,
                            Format = "int32",
                            Description = "Display order of the group"
                        },
                        ["weightBasisPoints"] = new OpenApiSchema
                        {
                            Type = JsonSchemaType.Integer | JsonSchemaType.Null,
                            Format = "int32",
                            Description = "Weight of the group expressed in basis points when weighted"
                        }
                    },
                    Required = new HashSet<string>
                    {
                        "id", "formId", "title", "description", "orderIndex"
                    }
                }
            },
            ["criteria"] = new OpenApiSchema
            {
                Type = JsonSchemaType.Array,
                Description = "Collection of evaluation criteria defined in the form",
                Items = new OpenApiSchema
                {
                    Type = JsonSchemaType.Object,
                    Properties = new Dictionary<string, IOpenApiSchema>
                    {
                        ["id"] = new OpenApiSchema
                        {
                            Type = JsonSchemaType.String,
                            Format = "uuid",
                            Description = "Unique identifier of the criterion"
                        },
                        ["formId"] = new OpenApiSchema
                        {
                            Type = JsonSchemaType.String | JsonSchemaType.Null,
                            Format = "uuid",
                            Description = "Identifier of the form when the criterion is at root level"
                        },
                        ["groupId"] = new OpenApiSchema
                        {
                            Type = JsonSchemaType.String,
                            Format = "uuid",
                            //Nullable = true,
                            Description = "Identifier of the parent group when nested"
                        },
                        ["title"] = new OpenApiSchema
                        {
                            Type = JsonSchemaType.String,
                            Description = "Display title of the criterion"
                        },
                        ["text"] = new OpenApiSchema
                        {
                            Type = JsonSchemaType.String,
                            Description = "Detailed text shown to the evaluator"
                        },
                        ["orderIndex"] = new OpenApiSchema
                        {
                            Type = JsonSchemaType.Integer,
                            Format = "int32",
                            Description = "Display order of the criterion inside its parent"
                        },
                        ["ratingOptions"] = new OpenApiSchema
                        {
                            Type = JsonSchemaType.String,
                            Description = "Serialized JSON describing rating options for the criterion"
                        },
                        ["weightBasisPoints"] = new OpenApiSchema
                        {
                            Type = JsonSchemaType.Integer | JsonSchemaType.Null,
                            Format = "int32",
                            Description = "Weight of the criterion expressed in basis points when weighted"
                        }
                    },
                    Required = new HashSet<string>
                    {
                        "id", "title", "text", "orderIndex", "ratingOptions"
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
