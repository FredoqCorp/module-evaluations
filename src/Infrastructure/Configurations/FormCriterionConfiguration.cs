using CascVel.Module.Evaluations.Management.Domain.Entities.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Configurations;

internal sealed class FormCriterionConfiguration : IEntityTypeConfiguration<FormCriterion>
{
    public void Configure(EntityTypeBuilder<FormCriterion> builder)
    {
        builder.ToTable("form_criteria", tbl =>
        {
            // Ensure domain constraints at DB level for Postgres 16
            tbl.HasCheckConstraint("ck_form_criteria_order_non_negative", "order_index >= 0");
            tbl.HasCheckConstraint("ck_form_criteria_weight_percent_range", "weight_percent IS NULL OR (weight_percent >= 0 AND weight_percent <= 100)");
        });

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
               .HasColumnName("id")
               .ValueGeneratedOnAdd();

        ConfigureCriterionRef(builder);
        ConfigureOrder(builder);
        ConfigureWeight(builder);

        builder.HasIndex("criterion_id").HasDatabaseName("ix_form_criteria_fk_criterion");
        builder.HasIndex("order_index").HasDatabaseName("ix_form_criteria_order");
    }

    private static void ConfigureCriterionRef(EntityTypeBuilder<FormCriterion> builder)
    {
        builder.HasOne(x => x.Criterion)
               .WithMany()
               .HasForeignKey("criterion_id")
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired();

        builder.Property<long>("criterion_id").HasColumnName("criterion_id");
    }

    private static void ConfigureOrder(EntityTypeBuilder<FormCriterion> builder)
    {
        builder.ComplexProperty(x => x.Order, order =>
        {
            order.Property(o => o.Value)
                 .HasColumnName("order_index")
                 .HasColumnType("integer")
                 .IsRequired();
        });
    }

    private static void ConfigureWeight(EntityTypeBuilder<FormCriterion> builder)
    {
        builder.ComplexProperty(x => x.Weight, weight =>
        {
            // Store percent as numeric with 2 decimals; nullable for optional weight
            weight.Property(w => w.Percent)
                 .HasColumnName("weight_percent")
                 .HasColumnType("numeric(5,2)");
        });
        builder.Navigation(x => x.Weight).IsRequired(false);
    }
}
