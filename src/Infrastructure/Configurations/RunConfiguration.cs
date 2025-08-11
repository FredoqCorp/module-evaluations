using CascVel.Module.Evaluations.Management.Domain.Entities.Runs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Configurations;

internal sealed class RunConfiguration : IEntityTypeConfiguration<Run>
{
    public void Configure(EntityTypeBuilder<Run> builder)
    {
        builder.ToTable("runs");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.RunFor).IsRequired().HasMaxLength(100).HasColumnName("run_for");
        builder.Property(x => x.ScoreResult).HasColumnName("score_result");
        builder.Property(x => x.LastSavedAt).HasColumnName("last_saved_at");
        builder.Property(x => x.LastSavedBy).HasMaxLength(100).HasColumnName("last_saved_by");
        builder.Property(x => x.FirstSavedAt).HasColumnName("first_saved_at");
        builder.Property(x => x.FirstSavedBy).HasMaxLength(100).HasColumnName("first_saved_by");
        builder.Property(x => x.PublishedAt).HasColumnName("published_at");
        builder.Property(x => x.PublishedBy).HasMaxLength(100).HasColumnName("published_by");
        builder.Property(x => x.CreatedAt).IsRequired().HasColumnName("created_at");
        builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(100).HasColumnName("created_by");
        builder.Property(x => x.EvaluationFormComment).HasMaxLength(1000).HasColumnName("evaluation_form_comment");
        builder.Property(x => x.ViewedAt).HasColumnName("viewed_at");
        builder.Property(x => x.AgreementStatus).HasConversion<string>().HasMaxLength(32).HasColumnName("agreement_status");
        builder.Property(x => x.AgreementAt).HasColumnName("agreement_at");

        // Context dictionary -> jsonb
        builder.Property(x => x.Context).HasColumnType("jsonb").HasColumnName("context");

        // FK to EvaluationForm by id only (no navigation on entity)
        builder.Property(x => x.EvaluationFormId).HasColumnName("evaluation_form_id");

        builder.HasMany(x => x.RunCriterionResults)
               .WithOne()
               .HasForeignKey("RunId")
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.RunFor);
        builder.HasIndex(x => x.EvaluationFormId);
        builder.HasIndex(x => x.CreatedAt);
    }
}
