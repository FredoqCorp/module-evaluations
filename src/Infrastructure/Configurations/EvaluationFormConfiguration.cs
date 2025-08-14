using CascVel.Module.Evaluations.Management.Domain.Entities.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Configurations;

internal sealed class EvaluationFormConfiguration : IEntityTypeConfiguration<EvaluationForm>
{
    public void Configure(EntityTypeBuilder<EvaluationForm> builder)
    {
        builder.ToTable("evaluation_forms");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        ConfigureMeta(builder);
        ConfigureLifecycle(builder);
        ConfigureDesign(builder);
        ConfigureIndexes(builder);
    }

    private static void ConfigureMeta(EntityTypeBuilder<EvaluationForm> builder)
    {
        builder.ComplexProperty(x => x.Meta, meta =>
        {
            meta.Property(m => m.Description)
                .HasColumnName("description")
                .HasColumnType("text");

            meta.ComplexProperty(m => m.Name)
                .Property(n => n.Value)
                .HasColumnName("name")
                .HasColumnType("text")
                .IsRequired();

            meta.ComplexProperty(m => m.Code)
                .Property(c => c.Value)
                .HasColumnName("code")
                .HasColumnType("text")
                .IsRequired();

            meta.Property(m => m.Tags)
                .HasColumnName("tags")
                .HasColumnType("jsonb")
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
              .HasMaxLength(32)
              .IsRequired();

            lc.ComplexProperty(l => l.Validity, p =>
            {
                p.Property(v => v.Start).HasColumnName("valid_from");
                p.Property(v => v.End).HasColumnName("valid_until");
            });

            lc.ComplexProperty(l => l.Audit, a =>
            {
                a.ComplexProperty(t => t.Created, s =>
                {
                    s.Property(x => x.UserId).HasColumnName("created_by").HasMaxLength(100).IsRequired();
                    s.Property(x => x.At).HasColumnName("created_at").IsRequired();
                });
                a.ComplexProperty(t => t.Updated, s =>
                {
                    s.Property(x => x.UserId).HasColumnName("updated_by").HasMaxLength(100);
                    s.Property(x => x.At).HasColumnName("updated_at");
                });
                a.ComplexProperty(t => t.StateChanged, s =>
                {
                    s.Property(x => x.UserId).HasColumnName("state_changed_by").HasMaxLength(100);
                    s.Property(x => x.At).HasColumnName("state_changed_at");
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

            design.OwnsMany(d => d.Groups, groups =>
            {
                groups.ToTable("form_groups");
                groups.WithOwner().HasForeignKey("form_id");
                groups.Property<long>("id").ValueGeneratedOnAdd();
                groups.HasKey("id");

                groups.Property(g => g.Title).HasColumnName("title").HasColumnType("text").IsRequired();
                groups.Property(g => g.Order.Value).HasColumnName("order_index").IsRequired();
                groups.Property(g => g.Weight!.Percent).HasColumnName("weight");

                groups.HasIndex("form_id", "order_index").IsUnique();

                groups.OwnsMany(g => g.Criteria, crit =>
                {
                    crit.ToTable("group_criteria");
                    crit.WithOwner().HasForeignKey("group_id");
                    crit.Property<long>("id").ValueGeneratedOnAdd();
                    crit.HasKey("id");

                    crit.Property<long>("criterion_id").HasColumnName("criterion_id");
                    crit.HasOne(c => c.Criterion)
                        .WithMany()
                        .HasForeignKey("criterion_id")
                        .OnDelete(DeleteBehavior.Restrict);
                    crit.Property(c => c.Order.Value).HasColumnName("order_index").IsRequired();
                    crit.Property(c => c.Weight!.Percent).HasColumnName("weight");

                    crit.HasIndex("group_id", "order_index").IsUnique();
                });
            });
        });
    }

    private static void ConfigureIndexes(EntityTypeBuilder<EvaluationForm> builder)
    {
        builder.HasIndex("Meta_Code_Value").HasDatabaseName("ix_evaluation_forms_code").IsUnique();
    }
}
