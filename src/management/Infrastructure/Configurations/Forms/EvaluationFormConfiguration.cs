using System.Collections.Immutable;
using System.Linq;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

        builder.ComplexProperty<FormMeta>("meta", b =>
        {
            var nameConv = new ValueConverter<FormName, string>(
                v => v.Value,
                v => new FormName(v));

            var codeConv = new ValueConverter<FormCode, string>(
                v => v.Value,
                v => new FormCode(v));

            var tagsConv = new ValueConverter<IImmutableList<string>, string[]>(
                v => v.ToArray(),
                v => v.ToImmutableList());

            var tagsComparer = new ValueComparer<IImmutableList<string>>(
                (l, r) => ReferenceEquals(l, r) || (l != null && r != null && l.SequenceEqual(r)),
                v => v != null ? v.Aggregate(0, (a, e) => System.HashCode.Combine(a, e != null ? System.StringComparer.Ordinal.GetHashCode(e) : 0)) : 0,
                v => v != null ? v.ToImmutableList() : ImmutableList<string>.Empty);

            b.Property(m => m.Name)
                .HasConversion(nameConv)
                .HasColumnName("meta_name")
                .IsRequired();

            b.Property(m => m.Description)
                .HasColumnName("meta_description")
                .IsRequired();

            b.Property(m => m.Tags)
                .HasConversion(tagsConv)
                .Metadata.SetValueComparer(tagsComparer);
            b.Property(m => m.Tags)
                .HasColumnType("text[]")
                .HasColumnName("meta_tags")
                .IsRequired();

            b.Property(m => m.Code)
                .HasConversion(codeConv)
                .HasColumnName("meta_code")
                .IsRequired();
        });

        builder.HasIndex("meta.Code").IsUnique();



    }
}
