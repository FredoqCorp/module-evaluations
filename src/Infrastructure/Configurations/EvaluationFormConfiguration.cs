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
        builder.OwnsOne(x => x.Lifecycle, lc =>
        {
            lc.ToTable("forms");
            lc.Property(l => l.Status)
              .HasColumnName("status")
              .HasConversion<string>()
              .HasMaxLength(16)
              .IsRequired();

            lc.OwnsOne(l => l.Validity, p =>
            {
                p.Property(v => v.Start).HasColumnName("valid_from");
                p.Property(v => v.End).HasColumnName("valid_to");
            });

            lc.OwnsOne(l => l.Audit, a =>
            {
                a.ToTable("forms");
                a.OwnsOne(x => x.Created, s =>
                {
                    s.Property(t => t.UserId).HasColumnName("created_by").HasMaxLength(100).IsRequired();
                    s.Property(t => t.At).HasColumnName("created_at").IsRequired();
                });
                a.OwnsOne(x => x.Updated, s =>
                {
                    s.Property(t => t.UserId).HasColumnName("updated_by").HasMaxLength(100);
                    s.Property(t => t.At).HasColumnName("updated_at");
                });
                a.OwnsOne(x => x.StateChanged, s =>
                {
                    s.Property(t => t.UserId).HasColumnName("state_changed_by").HasMaxLength(100);
                    s.Property(t => t.At).HasColumnName("state_changed_at");
                });
            });
        });
    }

    private static void ConfigureDesign(EntityTypeBuilder<EvaluationForm> builder)
    {
        builder.OwnsOne(x => x.Design, design =>
        {
            design.ToTable("forms"); // share table with owner for scalar property
            design.Property(d => d.Calculation)
                  .HasColumnName("calculation")
                  .HasConversion<string>()
                  .HasMaxLength(32)
                  .IsRequired();

            design.Ignore(d => d.Criteria);
            design.Ignore(d => d.Groups);
        });
    }
}
