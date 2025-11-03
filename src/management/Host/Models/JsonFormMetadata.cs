using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.Models.Forms;

namespace CascVel.Modules.Evaluations.Management.Host.Models;

/// <summary>
/// JSON-backed metadata that projects raw payload into domain value objects and prints them when required.
/// </summary>
internal sealed record JsonFormMetadata : IFormMetadata
{
    private readonly JsonElement _metadata;

    internal JsonFormMetadata(JsonDocument document)
    {
        _metadata = document.RootElement.GetProperty("metadata");
    }

    /// <inheritdoc />
    public IMedia<TOutput> Print<TOutput>(IMedia<TOutput> media)
    {
        ArgumentNullException.ThrowIfNull(media);

        var name = new ValidFormName(new TrimmedFormName(new JsonFormName(_metadata))).Text();
        var description = new ValidFormDescription(new TrimmedFormDescription(new JsonFormDescription(_metadata))).Text();
        var code = new ValidFormCode(new TrimmedFormCode(new JsonFormCode(_metadata))).Text();
        var tags = new JsonFormTags(_metadata);

        media
            .With("name", name)
            .With("description", description)
            .With("code", code);

        tags.Print(media, "tags");
        return media;
    }
}
