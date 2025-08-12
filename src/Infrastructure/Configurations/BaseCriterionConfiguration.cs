using CascVel.Module.Evaluations.Management.Domain.Entities.EvaluationCriteria.Criteria;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Configurations;

internal sealed class BaseCriterionConfiguration : IEntityTypeConfiguration<BaseCriterion>
{
    public void Configure(EntityTypeBuilder<BaseCriterion> builder)
    {
        builder.ToTable("criteria");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Title).IsRequired().HasMaxLength(200).HasColumnName("title");
        builder.Property(x => x.Weight).HasColumnName("weight");
        builder.Property(x => x.Order).HasColumnName("order_index");

        // Shadow FK to EvaluationForm
        builder.Property<long>("EvaluationFormId").HasColumnName("evaluation_form_id");
        builder.HasIndex("EvaluationFormId");

        builder.HasDiscriminator<string>("criterion_type")
               .HasValue<DefaultCriterion>("default")
               .HasValue<AutomaticCriterion>("automatic")
               .HasValue<GroupCriterion>("group");

        builder.HasIndex("criterion_type");
    }
}
