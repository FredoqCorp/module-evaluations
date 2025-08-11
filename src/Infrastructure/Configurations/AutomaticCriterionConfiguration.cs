using CascVel.Module.Evaluations.Management.Domain.Entities.EvaluationCriteria.Criteria;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Configurations;

internal sealed class AutomaticCriterionConfiguration : IEntityTypeConfiguration<AutomaticCriterion>
{
    public void Configure(EntityTypeBuilder<AutomaticCriterion> builder)
    {
        builder.ToTable("criteria"); // TPH table

        builder.Property(x => x.Options)
               .HasColumnName("options")
               .HasColumnType("jsonb");

        builder.Property<long>("AutomaticParameterId").HasColumnName("automatic_parameter_id");
        builder.HasOne(x => x.AutomaticParameter)
               .WithMany()
               .HasForeignKey("AutomaticParameterId")
               .OnDelete(DeleteBehavior.Restrict);
    }
}
