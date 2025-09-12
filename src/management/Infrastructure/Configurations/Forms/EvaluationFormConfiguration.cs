using System.Collections.Immutable;
using System.Linq;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NpgsqlTypes;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Configurations.Forms;
/// <summary>
/// Infrastructure layer mapping for <see cref="EvaluationForm"/> aggregate using PostgreSQL jsonb columns for rich value objects.
/// Uses value converters to comply with domain encapsulation and constructor binding.
/// </summary>
internal sealed class EvaluationFormConfiguration : IEntityTypeConfiguration<EvaluationForm>
{
    /// <summary>
    /// Deterministic JSON options for jsonb serialization.
    /// </summary>
    private static readonly JsonSerializerOptions Json = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.Never,
        WriteIndented = false,
    };
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

        builder.ComplexProperty<FormLifecycle>("lifecycle", life =>
        {
            var periodConv = new ValueConverter<Period, NpgsqlRange<DateTime>>(
                p => p.Finish() == DateTime.MaxValue
                    ? new NpgsqlRange<DateTime>(p.Start(), DateTime.MaxValue)
                    : new NpgsqlRange<DateTime>(p.Start(), p.Finish()),
                r => new Period(r.LowerBound, r.UpperBound == DateTime.MaxValue ? null : r.UpperBound));

            life.Property(l => l.Validity)
                .HasConversion(periodConv)
                .HasColumnType("tsrange")
                .HasColumnName("life_validity")
                .IsRequired();

            life.ComplexProperty<FormAuditTail>(nameof(FormLifecycle.Tail), tail =>
            {
                tail.Property<FormAuditKind>("Kind")
                    .HasField("_kind")
                    .HasColumnName("life_tail_kind")
                    .IsRequired();

                tail.ComplexProperty<Stamp>("Stamp", stamp =>
                {
                    stamp.Property(s => s.UserId)
                        .HasColumnName("life_tail_user")
                        .IsRequired();

                    stamp.Property(s => s.At)
                        .HasColumnName("life_tail_at")
                        .IsRequired();
                }).HasField("_stamp");
            });
        });

        var groupsConv = new ValueConverter<IImmutableList<FormGroup>, string>(
            v => JsonSerializer.Serialize(v ?? ImmutableList<FormGroup>.Empty, Json),
            v => JsonSerializer.Deserialize<ImmutableList<FormGroup>>(v, Json) ?? ImmutableList<FormGroup>.Empty);

        var criteriaConv = new ValueConverter<IImmutableList<FormCriterion>, string>(
            v => JsonSerializer.Serialize(v ?? ImmutableList<FormCriterion>.Empty, Json),
            v => JsonSerializer.Deserialize<ImmutableList<FormCriterion>>(v, Json) ?? ImmutableList<FormCriterion>.Empty);

        var groupsComparer = new ValueComparer<IImmutableList<FormGroup>>(
            (l, r) => ReferenceEquals(l, r) || (l != null && r != null && l.SequenceEqual(r)),
            v => v != null ? v.Aggregate(0, (a, e) => HashCode.Combine(a, e != null ? e.GetHashCode() : 0)) : 0,
            v => v != null ? v.ToImmutableList() : ImmutableList<FormGroup>.Empty);

        var criteriaComparer = new ValueComparer<IImmutableList<FormCriterion>>(
            (l, r) => ReferenceEquals(l, r) || (l != null && r != null && l.SequenceEqual(r)),
            v => v != null ? v.Aggregate(0, (a, e) => HashCode.Combine(a, e != null ? e.GetHashCode() : 0)) : 0,
            v => v != null ? v.ToImmutableList() : ImmutableList<FormCriterion>.Empty);

        builder.Property<IImmutableList<FormGroup>>("_groups")
            .HasConversion(groupsConv)
            .Metadata.SetValueComparer(groupsComparer);
        builder.Property<IImmutableList<FormGroup>>("_groups")
            .HasColumnType("jsonb")
            .HasColumnName("groups")
            .IsRequired();

        builder.Property<IImmutableList<FormCriterion>>("_criteria")
            .HasConversion(criteriaConv)
            .Metadata.SetValueComparer(criteriaComparer);
        builder.Property<IImmutableList<FormCriterion>>("_criteria")
            .HasColumnType("jsonb")
            .HasColumnName("criteria")
            .IsRequired();

    }
}
