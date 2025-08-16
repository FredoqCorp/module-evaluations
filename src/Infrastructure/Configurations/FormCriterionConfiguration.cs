using CascVel.Module.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Module.Evaluations.Management.Domain.Entities.Criteria;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Configurations;

internal sealed class FormCriterionConfiguration : IEntityTypeConfiguration<FormCriterion>
{
    public void Configure(EntityTypeBuilder<FormCriterion> builder)
    {
        builder.ToTable("form_criteria");
        builder.Property<long>("id").ValueGeneratedOnAdd();
        builder.HasKey("id");

        builder.Property<long?>("form_id").HasColumnName("form_id");
        builder.Property<long?>("group_id").HasColumnName("group_id");

        builder.HasOne<EvaluationForm>()
            .WithMany()
            .HasForeignKey("form_id")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<FormGroup>()
            .WithMany(g => g.Criteria)
            .HasForeignKey("group_id")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property<long>("criterion_id");
        builder.HasOne(c => c.Criterion)
            .WithMany()
            .HasForeignKey("criterion_id")
            .OnDelete(DeleteBehavior.Cascade);
        builder.Property<long>("criterion_id").HasColumnName("criterion_id");

        builder.ComplexProperty(c => c.Order, order =>
        {
            order.Property(o => o.Value).HasColumnName("order_index").IsRequired();
        });

        builder.ComplexProperty(c => c.Weight, weight =>
        {
            weight.Property(w => w.Percent).HasColumnName("weight");
        });

        builder.HasCheckConstraint("ck_form_criteria_owner", "((form_id IS NULL) <> (group_id IS NULL))");

        builder.HasIndex("form_id", "Order_Value")
            .HasDatabaseName("ix_form_criteria_root_order_unique")
            .IsUnique()
            .HasFilter("group_id IS NULL");

        builder.HasIndex("group_id", "Order_Value")
            .HasDatabaseName("ix_form_criteria_group_order_unique")
            .IsUnique()
            .HasFilter("group_id IS NOT NULL");

        builder.HasIndex("form_id", "criterion_id")
            .HasDatabaseName("ix_form_criteria_root_unique")
            .IsUnique()
            .HasFilter("group_id IS NULL");

        builder.HasIndex("group_id", "criterion_id")
            .HasDatabaseName("ix_form_criteria_group_unique")
            .IsUnique()
            .HasFilter("group_id IS NOT NULL");
    }
}

