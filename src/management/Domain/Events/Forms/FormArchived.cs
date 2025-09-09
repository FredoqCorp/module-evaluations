using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects;

namespace CascVel.Modules.Evaluations.Management.Domain.Events.Forms;

/// <summary>
/// Domain event emitted when an evaluation form is archived.
/// </summary>
public sealed record FormArchived(EvaluationFormId Form, Stamp Stamp);

