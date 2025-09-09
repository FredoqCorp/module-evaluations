using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.Events.Forms;

/// <summary>
/// Domain event emitted when an evaluation form content is edited.
/// Includes a change set with field-level diffs.
/// </summary>
public sealed record FormEdited(EvaluationFormId Form, Stamp Stamp, IImmutableList<FormChange> Changes);

