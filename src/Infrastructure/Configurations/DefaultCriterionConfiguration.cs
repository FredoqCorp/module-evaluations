using CascVel.Module.Evaluations.Management.Domain.Entities.EvaluationCriteria.Criteria;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Configurations;

internal sealed class DefaultCriterionConfiguration : IEntityTypeConfiguration<DefaultCriterion>
{
    public void Configure(EntityTypeBuilder<DefaultCriterion> builder)
    {
        builder.ToTable("criteria"); // TPH table
        builder.Property(x => x.Options)
               .HasColumnName("options")
               .HasColumnType("jsonb");
    }
}
