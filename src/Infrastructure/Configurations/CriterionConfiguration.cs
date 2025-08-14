using CascVel.Module.Evaluations.Management.Domain.Entities.Criteria;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Configurations;

internal sealed class CriterionConfiguration : IEntityTypeConfiguration<Criterion>
{
    public void Configure(EntityTypeBuilder<Criterion> builder)
    {
        builder.ToTable("criteria");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

              ConfigureText(builder);
              ConfigureOptions(builder);
              ConfigureAutomation(builder);
    }

       private static void ConfigureText(EntityTypeBuilder<Criterion> builder)
       {
              builder.ComplexProperty(x => x.Text, text =>
              {
                     text.ComplexProperty(t => t.Title)
                            .Property(p => p.Value)
                            .HasColumnName("title")
                            .HasColumnType("text")
                            .IsRequired();

                     text.ComplexProperty(t => t.Description)
                            .Property(p => p.Value)
                            .HasColumnName("description")
                            .HasColumnType("text")
                            .IsRequired();
              });
       }

       private static void ConfigureOptions(EntityTypeBuilder<Criterion> builder)
       {
              // Options owned collection in separate table
              builder.OwnsMany(x => x.Options, options =>
              {
                     options.ToTable("criterion_options");
                     options.WithOwner().HasForeignKey("criterion_id");
                     options.Property<long>("id").ValueGeneratedOnAdd();
                     options.HasKey("id");

                     options.Property(o => o.Score)
                               .HasColumnName("score")
                               .HasColumnType("smallint")
                               .IsRequired();

                     options.Property(o => o.Caption)
                               .HasColumnName("caption")
                               .HasColumnType("text");

                     options.Property(o => o.Annotation)
                               .HasColumnName("annotation")
                               .HasColumnType("text");

                     options.Property(o => o.Threshold)
                               .HasColumnName("threshold");

                           // Index on the owner FK and on Score (avoid string-based composite including value object's member)
                           options.HasIndex("criterion_id").HasDatabaseName("ix_criterion_options_fk");
                           options.HasIndex(o => o.Score).HasDatabaseName("ix_criterion_options_score");
              });
       }

       private static void ConfigureAutomation(EntityTypeBuilder<Criterion> builder)
       {
              builder.OwnsOne(x => x.Automation, auto =>
              {
                    // Map to a separate table to avoid optional-owned-with-nested-dependents table sharing issues
                    auto.ToTable("criterion_automation");

                     auto.OwnsOne(a => a.Source, s =>
                     {
                            s.Property(p => p.ParameterKey)
                             .HasColumnName("auto_source_key")
                             .HasColumnType("text");
                     });

                     auto.OwnsOne(a => a.Rule, r =>
                     {
                            r.OwnsOne(t => t.ThresholdPolicy, tp =>
                            {
                                   tp.Property(p => p.Goal)
                                     .HasColumnName("auto_goal")
                                     .HasConversion<string>()
                                     .HasMaxLength(16);
                            });
                     });
              });
              builder.Navigation(x => x.Automation).IsRequired(false);
       }
}
