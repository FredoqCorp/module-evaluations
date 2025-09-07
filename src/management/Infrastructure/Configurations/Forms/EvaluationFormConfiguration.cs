using System.Collections.Immutable;
using System.Runtime.Intrinsics.X86;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Configurations.Forms;
/// <summary>
/// Infrastructure layer mapping for <see cref="EvaluationForm"/> aggregate using PostgreSQL jsonb columns for rich value objects.
/// Uses value converters to comply with domain encapsulation and constructor binding.
/// </summary>
internal sealed class EvaluationFormConfiguration : IEntityTypeConfiguration<EvaluationForm>
{
    /// <summary>
    /// Creates a configuration with a default JSON converter implementation.
    /// </summary>
    public EvaluationFormConfiguration()
    {
    }

    /// <summary>
    /// Configures entity mapping for <see cref="EvaluationForm"/>.
    /// </summary>
    public void Configure(EntityTypeBuilder<EvaluationForm> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable("forms");

        var idConv = new ValueConverter<EvaluationFormId, Guid>(
            v => v.Value,
            v => new EvaluationFormId(v));

        builder.Property<EvaluationFormId>("id")
            .HasConversion(idConv)
            .HasColumnName("id")
            .IsRequired();

        builder.HasKey("id");

    }
}
