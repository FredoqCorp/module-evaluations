using System;
using System.Linq;
using CascVel.Module.Evaluations.Management.Domain.Entities.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Configurations;

internal sealed class EvaluationFormConfiguration : IEntityTypeConfiguration<EvaluationForm>
{
    public void Configure(EntityTypeBuilder<EvaluationForm> builder)
    {
        builder.ToTable("evaluation_forms", tbl =>
        {
            // Period integrity: forbid orphaned "valid_to" without "valid_from" and ensure range order
            tbl.HasCheckConstraint(
                name: "ck_eval_forms_validity_presence",
                sql: "(valid_from IS NULL AND valid_to IS NULL) OR (valid_from IS NOT NULL)"
            );

            tbl.HasCheckConstraint(
                name: "ck_eval_forms_validity_range",
                sql: "valid_to IS NULL OR valid_from <= valid_to"
            );
        });

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
               .HasColumnName("id")
               .ValueGeneratedOnAdd();

        ConfigureMeta(builder);
        ConfigureLifecycle(builder);
        ConfigureDesign(builder);

        // Useful indexes
        builder.HasIndex(e => e.Lifecycle.Status)
               .HasDatabaseName("ix_evaluation_forms_status");

        builder.HasIndex(e => e.Meta.Code.Value)
               .HasDatabaseName("ux_evaluation_forms_code")
               .IsUnique();
    }

    private static void ConfigureMeta(EntityTypeBuilder<EvaluationForm> builder)
    {
    builder.ComplexProperty(x => x.Meta, meta =>
        {
            meta.ComplexProperty(m => m.Name, name =>
            {
                name.Property(n => n.Value)
                    .HasColumnName("name")
                    .HasColumnType("text")
                    .IsRequired();
            });

            meta.Property(m => m.Description)
                .HasColumnName("description")
                .HasColumnType("text");

            // Tags: store as Postgres text[]; map IReadOnlyList<string> with conversion and comparer
            var tagsComparer = new ValueComparer<IReadOnlyList<string>>(
                (a, b) => ReferenceEquals(a, b) || (a != null && b != null && a.SequenceEqual(b, StringComparer.OrdinalIgnoreCase)),
                v => v != null ? v.Aggregate(0, (acc, s) => HashCode.Combine(acc, StringComparer.OrdinalIgnoreCase.GetHashCode(s ?? string.Empty))) : 0,
                v => (IReadOnlyList<string>) (v == null ? Array.Empty<string>() : v.ToArray()))
            ;

            meta.Property(m => m.Tags)
                .HasColumnName("tags")
                .HasColumnType("text[]")
                .HasConversion(v => v.ToArray(), v => (IReadOnlyList<string>)Array.AsReadOnly(v))
                .Metadata.SetValueComparer(tagsComparer);

            // Keep non-null (empty array by default)
            meta.Property(m => m.Tags).HasDefaultValueSql("'{}'::text[]").IsRequired();

            meta.ComplexProperty(m => m.Code, code =>
            {
                code.Property(c => c.Value)
                    .HasColumnName("code")
                    .HasColumnType("text")
                    .IsRequired();
            });
        });
    }

    private static void ConfigureLifecycle(EntityTypeBuilder<EvaluationForm> builder)
    {
        builder.ComplexProperty(x => x.Lifecycle, lc =>
        {
            lc.Property(l => l.Status)
              .HasColumnName("status")
              .HasConversion<string>()
              .HasMaxLength(16)
              .IsRequired();

            lc.ComplexProperty(l => l.Validity, period =>
            {
                period.Property(p => p.Start)
                      .HasColumnName("valid_from");
                period.Property(p => p.End)
                      .HasColumnName("valid_to");
            });

            lc.ComplexProperty(l => l.Audit, audit =>
            {
                audit.ComplexProperty(a => a.Created, s => MapStamp(s, "created", required: true));
                audit.ComplexProperty(a => a.Updated, s => MapStamp(s, "updated", required: false));
                audit.ComplexProperty(a => a.StateChanged, s => MapStamp(s, "state_changed", required: false));
            });
        });
    }

    private static void ConfigureDesign(EntityTypeBuilder<EvaluationForm> builder)
    {
        builder.ComplexProperty(x => x.Design, design =>
        {
            design.Property(d => d.Calculation)
                  .HasColumnName("calculation_kind")
                  .HasConversion<string>()
                  .HasMaxLength(32)
                  .IsRequired();

        });
    }

    private static void MapStamp(ComplexPropertyBuilder<Stamp> stamp, string prefix, bool required)
    {
        var user = stamp.Property(s => s.UserId)
             .HasColumnName($"{prefix}_by")
             .HasMaxLength(100);

        var at = stamp.Property(s => s.At)
             .HasColumnName($"{prefix}_at");

        if (required)
        {
            user.IsRequired();
            at.IsRequired();
        }
    }
}
