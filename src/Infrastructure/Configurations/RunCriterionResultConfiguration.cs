using CascVel.Module.Evaluations.Management.Domain.Entities.Runs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Configurations;

internal sealed class RunCriterionResultConfiguration : IEntityTypeConfiguration<RunCriterionResult>
{
    public void Configure(EntityTypeBuilder<RunCriterionResult> builder)
    {
        builder.ToTable("run_criterion_results");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Comment).HasMaxLength(1000).HasColumnName("comment");
        builder.Property(x => x.IsSkipped).IsRequired().HasColumnName("is_skipped");
        builder.Property(x => x.AutomaticCriterionValue).HasColumnName("automatic_value");
        builder.Property(x => x.Score).HasColumnName("score");
        builder.Property(x => x.CriterionId).HasColumnName("criterion_id");

        builder.Property<long>("RunId").HasColumnName("run_id");

        builder.HasIndex("RunId");
        builder.HasIndex(x => x.CriterionId);
    }
}
