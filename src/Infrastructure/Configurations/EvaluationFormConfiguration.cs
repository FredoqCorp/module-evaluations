using CascVel.Module.Evaluations.Management.Domain.Entities.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Configurations;

internal sealed class EvaluationFormConfiguration : IEntityTypeConfiguration<EvaluationForm>
{
    public void Configure(EntityTypeBuilder<EvaluationForm> builder)
    {
        builder.ToTable("forms");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();

        ConfigureMeta(builder);
        ConfigureLifecycle(builder);
        ConfigureDesign(builder);

        builder.HasIndex(e => e.Meta.Code.Value)
            .HasDatabaseName("ix_forms_code_unique")
            .IsUnique();
    }

    private static void ConfigureMeta(EntityTypeBuilder<EvaluationForm> builder)
    {
        builder.ComplexProperty(x => x.Meta, meta =>
        {
            meta.ComplexProperty(m => m.Name)
                .Property(p => p.Value)
                .HasColumnName("name")
                .HasColumnType("text")
                .IsRequired();

            meta.Property(m => m.Description)
                .HasColumnName("description")
                .HasColumnType("text");

            meta.Property(m => m.Tags)
                .HasColumnName("tags")
                .HasColumnType("jsonb")
                .IsRequired();

            meta.ComplexProperty(m => m.Code)
                .Property(p => p.Value)
                .HasColumnName("code")
                .HasColumnType("text")
                .IsRequired();
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

            lc.ComplexProperty(l => l.Validity, v =>
            {
                v.Property(p => p.Start).HasColumnName("valid_from");
                v.Property(p => p.End).HasColumnName("valid_to");
            });

            lc.ComplexProperty(l => l.Audit, audit =>
            {
                audit.ComplexProperty(a => a.Created, s =>
                {
                    s.Property(p => p.UserId).HasColumnName("created_by").HasMaxLength(100).IsRequired();
                    s.Property(p => p.At).HasColumnName("created_at").IsRequired();
                });
                audit.ComplexProperty(a => a.Updated, s =>
                {
                    s.Property(p => p.UserId).HasColumnName("updated_by").HasMaxLength(100);
                    s.Property(p => p.At).HasColumnName("updated_at");
                });
                audit.ComplexProperty(a => a.StateChanged, s =>
                {
                    s.Property(p => p.UserId).HasColumnName("state_changed_by").HasMaxLength(100);
                    s.Property(p => p.At).HasColumnName("state_changed_at");
                });
            });
        });
    }

    private static void ConfigureDesign(EntityTypeBuilder<EvaluationForm> builder)
    {
        builder.OwnsOne(x => x.Design, design =>
        {
            design.Property(d => d.Calculation)
                .HasColumnName("calculation")
                .HasConversion<string>()
                .HasMaxLength(32)
                .IsRequired();

            design.HasMany(d => d.Groups)
                .WithOne()
                .HasForeignKey("form_id")
                .OnDelete(DeleteBehavior.Cascade);

            design.HasMany(d => d.Criteria)
                .WithOne()
                .HasForeignKey("form_id")
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}

