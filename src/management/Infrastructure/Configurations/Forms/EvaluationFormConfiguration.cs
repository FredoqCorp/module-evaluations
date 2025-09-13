using System.Collections.Immutable;
using System.Collections.Generic;
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
    /// JSON options for deterministic jsonb serialization.
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
            .HasField("_id")
            .IsRequired();

        builder.HasKey("id");

        var metaProp = builder.Property<FormMeta>("meta")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
        metaProp.HasField("_meta");
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


        var lifecycleProp = builder.Property<FormLifecycle>("lifecycle")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
        lifecycleProp.HasField("_lifecycle");
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

        var criteriaConv = new ValueConverter<FormCriteriaList, string>(
            v => SerializeCriteriaList(v),
            s => DeserializeCriteriaList(s));

        var criteriaComparer = new ValueComparer<FormCriteriaList>(
            (l, r) => ReferenceEquals(l, r) || (l != null && r != null && SerializeCriteriaList(l) == SerializeCriteriaList(r)),
            v => v != null ? StringComparer.Ordinal.GetHashCode(SerializeCriteriaList(v)) : 0,
            v => v != null ? DeserializeCriteriaList(SerializeCriteriaList(v)) : new FormCriteriaList(new List<FormCriterion>(0)));

        var prop = builder.Property<FormCriteriaList>("criteria")
            .HasColumnName("criteria")
            .HasColumnType("jsonb")
            .HasConversion(criteriaConv)
            .IsRequired();

        prop.Metadata.SetValueComparer(criteriaComparer);
        prop.HasField("_criteria");
        prop.UsePropertyAccessMode(PropertyAccessMode.Field);

    }

    private static string SerializeCriteriaList(FormCriteriaList value)
    {
        if (value == null)
        {
            return "[]";
        }
        var list = value.Items();
        return JsonSerializer.Serialize(list, Json);
    }

    private static FormCriteriaList DeserializeCriteriaList(string json)
    {
        var list = JsonSerializer.Deserialize<List<FormCriterion>>(json, Json) ?? [];
        return new FormCriteriaList(list);
    }
}
