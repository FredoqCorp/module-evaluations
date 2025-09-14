using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NpgsqlTypes;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects;
using CascVel.Modules.Evaluations.Management.Infrastructure.Serialization;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Policies;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Configurations.Forms;
/// <summary>
/// Infrastructure layer mapping for <see cref="EvaluationForm"/> aggregate using PostgreSQL jsonb columns for rich value objects.
/// Uses value converters to comply with domain encapsulation and constructor binding.
/// </summary>
internal sealed class EvaluationFormConfiguration : IEntityTypeConfiguration<EvaluationForm>
{
    /// <summary>
    /// Creates a configuration for EvaluationForm.
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

        builder.Property<EvaluationFormId>("_id")
            .HasConversion(idConv)
            .HasColumnName("id")
            .IsRequired();

        builder.HasKey("_id");

        // Complex field-only property '_meta'
        builder.ComplexProperty<FormMeta>("_meta", b =>
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


        // Complex field-only property '_lifecycle'
        builder.ComplexProperty<FormLifecycle>("_lifecycle", life =>
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

        var criteriaConv = new ValueConverter<FormCriteriaList, string>(
            v => FormCriteriaJson.Serialize(v),
            s => FormCriteriaJson.Deserialize(s));

        var criteriaComparer = new ValueComparer<FormCriteriaList>(
            (l, r) => ReferenceEquals(l, r) || (l != null && r != null && FormCriteriaJson.Serialize(l) == FormCriteriaJson.Serialize(r)),
            v => v != null ? StringComparer.Ordinal.GetHashCode(FormCriteriaJson.Serialize(v)) : 0,
            v => v != null ? FormCriteriaJson.Deserialize(FormCriteriaJson.Serialize(v)) : new FormCriteriaList(new List<FormCriterion>(0)));

        var prop = builder.Property<FormCriteriaList>("_criteria")
            .HasColumnName("criteria")
            .HasColumnType("jsonb")
            .HasConversion(criteriaConv)
            .IsRequired();

        prop.Metadata.SetValueComparer(criteriaComparer);
        prop.UsePropertyAccessMode(PropertyAccessMode.Field);

        var defConv = new ValueConverter<ICalculationPolicyDefinition, string>(
            d => PolicyDefinitionJson.Serialize(d),
            s => PolicyDefinitionJson.Deserialize(s));

        var defProp = builder.Property<ICalculationPolicyDefinition>("_definition")
            .HasColumnName("definition")
            .HasColumnType("jsonb")
            .HasConversion(defConv)
            .IsRequired();
        defProp.UsePropertyAccessMode(PropertyAccessMode.Field);
        // definition mapping configured above via field-only property '_definition'

    }

    
}
