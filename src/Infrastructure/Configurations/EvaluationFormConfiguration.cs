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

        builder.Property(x => x.Calculation)
                  .HasColumnName("calculation_kind")
                  .IsRequired();

        ConfigureMeta(builder);
        ConfigureLifecycle(builder);
        ConfigureRelations(builder);
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

            var tagsComparer = new ValueComparer<IReadOnlyList<string>>(
                (a, b) => ReferenceEquals(a, b) || (a != null && b != null && a.SequenceEqual(b, StringComparer.OrdinalIgnoreCase)),
                v => v != null ? v.Aggregate(0, (acc, s) => HashCode.Combine(acc, StringComparer.OrdinalIgnoreCase.GetHashCode(s ?? string.Empty))) : 0,
                v => v == null ? Array.Empty<string>() : v.ToArray())
            ;

            meta.Property(m => m.Tags)
                .HasColumnName("tags")
                .HasColumnType("text[]")
                .HasConversion(v => v.ToArray(), v => Array.AsReadOnly(v))
                .Metadata.SetValueComparer(tagsComparer);

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
        builder.OwnsOne(x => x.Lifecycle, lc =>
        {
            lc.Property(l => l.Status)
              .HasColumnName("status")
              .IsRequired();

            lc.OwnsOne(l => l.Validity, period =>
            {
                period.Property(p => p.Start)
                      .HasColumnName("valid_from");
                period.Property(p => p.End)
                      .HasColumnName("valid_to");
            });

            lc.OwnsOne(l => l.Audit, audit =>
            {

                audit.OwnsOne(a => a.Created, s =>
                {
                    MapStampOwned(s, "created");
                    s.ToTable("evaluation_forms");
                });
                audit.Navigation(a => a.Created).IsRequired();

                audit.OwnsOne(a => a.Updated, s =>
                {
                    MapStampOwned(s, "updated");
                    s.ToTable("evaluation_forms");
                });
                audit.Navigation(a => a.Updated).IsRequired(false);

                audit.OwnsOne(a => a.StateChanged, s =>
                {
                    MapStampOwned(s, "state_changed");
                    s.ToTable("evaluation_forms");
                });
                audit.Navigation(a => a.StateChanged).IsRequired(false);

                audit.ToTable(t =>
                {
                    t.HasCheckConstraint("CK_evalform_updated_all_or_none",
                        "(updated_by IS NULL) = (updated_at IS NULL)");
                    t.HasCheckConstraint("CK_evalform_statechg_all_or_none",
                        "(state_changed_by IS NULL) = (state_changed_at IS NULL)");
                });
            });
        });
    }

    private static void ConfigureRelations(EntityTypeBuilder<EvaluationForm> builder)
    {
        builder.HasMany(x => x.Groups)
            .WithOne(g => g.Form)
            .HasForeignKey(g => g.FormId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Criteria)
            .WithOne()
            .HasForeignKey("form_id")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Groups).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(x => x.Criteria).UsePropertyAccessMode(PropertyAccessMode.Field);
    }

    private static void MapStampOwned<TParent>(
        OwnedNavigationBuilder<TParent, Stamp> s, string prefix)
        where TParent : class
    {
        s.Property(x => x.UserId)
         .HasColumnName($"{prefix}_by")
         .HasMaxLength(100)
         .IsRequired(true);

        s.Property(x => x.At)
         .HasColumnName($"{prefix}_at")
         .IsRequired(true);
    }
}
