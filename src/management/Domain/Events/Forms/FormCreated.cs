using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects;

namespace CascVel.Modules.Evaluations.Management.Domain.Events.Forms;

/// <summary>
/// Domain event emitted when an evaluation form is created.
/// </summary>
public sealed record FormCreated(EvaluationFormId Form, Stamp Stamp);

