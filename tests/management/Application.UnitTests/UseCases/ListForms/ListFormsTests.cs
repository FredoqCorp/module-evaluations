using System.Diagnostics.CodeAnalysis;
using CascVel.Modules.Evaluations.Management.Application.UnitTests.TestDoubles;
using CascVel.Modules.Evaluations.Management.Application.UseCases.ListForms;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Shared;
using Shouldly;
using Xunit;

namespace CascVel.Modules.Evaluations.Management.Application.UnitTests.UseCases.ListForms;

public sealed class ListFormsTests
{
    private readonly FakeForms _fakeForms = new();

    [SuppressMessage("Performance", "CA1859:Use concrete types when possible for improved performance",
        Justification = "Testing against interface is more appropriate for unit tests")]
    private readonly IListForms _sut;

    public ListFormsTests()
    {
        _sut = new Application.UseCases.ListForms.ListForms(_fakeForms);
    }

    [Fact]
    public async Task Execute_WhenNoForms_ReturnsEmptyList()
    {
        // Act
        var response = await _sut.Execute();

        // Assert
        response.ShouldNotBeNull();
        response.Forms.ShouldNotBeNull();
        response.Forms.ShouldBeEmpty();
    }

    [Fact]
    public async Task Execute_WhenSingleForm_ReturnsSingleFormSummary()
    {
        // Arrange
        var summary = CreateFormSummary("Test Form", "TEST01", 2, 5);
        _fakeForms.AddSummary(summary);

        // Act
        var response = await _sut.Execute();

        // Assert
        response.ShouldNotBeNull();
        response.Forms.ShouldNotBeNull();
        response.Forms.Count.ShouldBe(1);
        response.Forms[0].ShouldBe(summary);
    }

    [Fact]
    public async Task Execute_WhenMultipleForms_ReturnsAllFormSummaries()
    {
        // Arrange
        var summary1 = CreateFormSummary("Form 1", "FORM01", 2, 5);
        var summary2 = CreateFormSummary("Form 2", "FORM02", 3, 7);
        var summary3 = CreateFormSummary("Form 3", "FORM03", 1, 3);
        _fakeForms.AddSummary(summary1);
        _fakeForms.AddSummary(summary2);
        _fakeForms.AddSummary(summary3);

        // Act
        var response = await _sut.Execute();

        // Assert
        response.ShouldNotBeNull();
        response.Forms.ShouldNotBeNull();
        response.Forms.Count.ShouldBe(3);
        response.Forms[0].ShouldBe(summary1);
        response.Forms[1].ShouldBe(summary2);
        response.Forms[2].ShouldBe(summary3);
    }

    [Fact]
    public async Task Execute_PropagatesCancellationToken()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        // Act & Assert
        await Should.ThrowAsync<OperationCanceledException>(async () =>
            await _sut.Execute(cts.Token));
    }

    [Fact]
    public async Task Execute_WhenRepositoryThrowsException_PropagatesException()
    {
        // Arrange
        var expectedException = new InvalidOperationException("Database error");
        _fakeForms.SetThrowException(expectedException);

        // Act & Assert
        var exception = await Should.ThrowAsync<InvalidOperationException>(async () =>
            await _sut.Execute());
        exception.Message.ShouldBe("Database error");
    }

    [Fact]
    public async Task Execute_WithCancellationTokenDefault_Succeeds()
    {
        // Arrange
        var summary = CreateFormSummary("Test Form", "TEST01", 2, 5);
        _fakeForms.AddSummary(summary);

        // Act
        var response = await _sut.Execute(CancellationToken.None);

        // Assert
        response.ShouldNotBeNull();
        response.Forms.Count.ShouldBe(1);
    }

    [Fact]
    public async Task Execute_ReturnsImmutableList()
    {
        // Arrange
        var summary = CreateFormSummary("Test Form", "TEST01", 2, 5);
        _fakeForms.AddSummary(summary);

        // Act
        var response = await _sut.Execute();

        // Assert
        response.Forms.ShouldBeAssignableTo<System.Collections.Immutable.IImmutableList<IFormSummary>>();
    }

    [Fact]
    public async Task Execute_WithZeroCountsForm_ReturnsFormWithZeroCounts()
    {
        // Arrange
        var summary = CreateFormSummary("Empty Form", "EMPTY01", 0, 0);
        _fakeForms.AddSummary(summary);

        // Act
        var response = await _sut.Execute();

        // Assert
        response.ShouldNotBeNull();
        response.Forms.Count.ShouldBe(1);
        response.Forms[0].ShouldBe(summary);
    }

    private static FormSummary CreateFormSummary(
        string name,
        string code,
        int groupsCount,
        int criteriaCount)
    {
        var id = new FormId(Guid.NewGuid());
        var formName = new FormName(name);
        var description = new FormDescription($"Description for {name}");
        var formCode = new FormCode(code);
        var tags = new Tags([new Tag("tag1"), new Tag("tag2")]);
        var metadata = new FormMetadata(formName, description, formCode, tags);
        return new FormSummary(id, metadata, CalculationType.Average, groupsCount, criteriaCount);
    }
}
