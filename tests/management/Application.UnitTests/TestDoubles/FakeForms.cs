using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using CascVel.Modules.Evaluations.Management.Application.Ports;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

namespace CascVel.Modules.Evaluations.Management.Application.UnitTests.TestDoubles;

internal sealed class FakeForms : IForms
{
    private readonly List<IFormSummary> _summaries = [];
    private bool _shouldThrow;
    private Exception? _exceptionToThrow;

    public void AddSummary(IFormSummary summary) => _summaries.Add(summary);

    public void SetThrowException(Exception exception)
    {
        _shouldThrow = true;
        _exceptionToThrow = exception;
    }

    public Task<IImmutableList<IFormSummary>> List(CancellationToken ct = default)
    {
        if (_shouldThrow && _exceptionToThrow is not null)
        {
            throw _exceptionToThrow;
        }

        ct.ThrowIfCancellationRequested();
        return Task.FromResult<IImmutableList<IFormSummary>>(_summaries.ToImmutableList());
    }

    [ExcludeFromCodeCoverage]
    public Task<IForm?> Get(FormId id, CancellationToken ct = default)
    {
        throw new NotImplementedException("Not needed for ListForms tests");
    }

    [ExcludeFromCodeCoverage]
    public Task Save(IForm form, CancellationToken ct = default)
    {
        throw new NotImplementedException("Not needed for ListForms tests");
    }
}
