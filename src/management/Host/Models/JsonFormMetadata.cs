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

    internal JsonFormMetadata(JsonElement element)
    {
        _metadata = element;
    }

    /// <inheritdoc />
    public IMedia<TOutput> Print<TOutput>(IMedia<TOutput> media)
    {
        ArgumentNullException.ThrowIfNull(media);
        media
            .With("name", new ValidFormName(new TrimmedFormName(new JsonFormName(_metadata))).Text())
            .With("description", new ValidFormDescription(new TrimmedFormDescription(new JsonFormDescription(_metadata))).Text())
            .With("code", new ValidFormCode(new TrimmedFormCode(new JsonFormCode(_metadata))).Text());
        new JsonFormTags(_metadata).Print(media, "tags");
        return media;
    }
}
