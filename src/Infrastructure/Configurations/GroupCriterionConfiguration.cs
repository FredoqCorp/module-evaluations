using CascVel.Module.Evaluations.Management.Domain.Entities.EvaluationCriteria.Criteria;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Configurations;

internal sealed class GroupCriterionConfiguration : IEntityTypeConfiguration<GroupCriterion>
{
    public void Configure(EntityTypeBuilder<GroupCriterion> builder)
    {
        builder.ToTable("criteria"); // TPH table

        // Many-to-many self join for children
        builder.HasMany(x => x.Childrens)
               .WithMany()
               .UsingEntity<Dictionary<string, object>>(
                    "criterion_children",
                    right => right.HasOne<BaseCriterion>()
                                  .WithMany()
                                  .HasForeignKey("child_id")
                                  .OnDelete(DeleteBehavior.Cascade),
                    left => left.HasOne<GroupCriterion>()
                                 .WithMany()
                                 .HasForeignKey("parent_id")
                                 .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.ToTable("criterion_children");
                        j.HasKey("parent_id", "child_id");
                    });
    }
}
