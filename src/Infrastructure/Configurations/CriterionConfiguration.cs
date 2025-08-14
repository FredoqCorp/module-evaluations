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

                     options.HasIndex("criterion_id", "score").HasDatabaseName("ix_criterion_options_score");
              });
       }

       private static void ConfigureAutomation(EntityTypeBuilder<Criterion> builder)
       {
              builder.ComplexProperty(x => x.Automation, auto =>
              {
                     auto.ComplexProperty(a => a.Source)
                            .Property(s => s.ParameterKey)
                            .HasColumnName("auto_source_key")
                            .HasColumnType("text");

                     auto.ComplexProperty(a => a.Rule)
                            .ComplexProperty(r => r.ThresholdPolicy)
                            .Property(tp => tp.Goal)
                            .HasColumnName("auto_goal")
                            .HasConversion<string>()
                            .HasMaxLength(16);
              });
       }
}
