using CascVel.Module.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Module.Evaluations.Management.Domain.Entities.Forms.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Configurations;

internal sealed class FormCriterionConfiguration : IEntityTypeConfiguration<FormCriterion>
{
    public void Configure(EntityTypeBuilder<FormCriterion> builder)
    {
        builder.ToTable("form_criteria", tbl =>
        {
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
        //builder.HasIndex("order_index").HasDatabaseName("ix_form_criteria_order");
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
        builder.Property(x => x.Weight)
                .HasConversion(v => v == null ? (ushort?)null : v.Bps(),
                           v => v == null ? null : new Weight(v.Value / 100m))
                .HasColumnName("weight")
                .HasColumnType("smallint")
                .IsRequired(false);

        builder.ToTable(t => t.HasCheckConstraint(
            "CK_formcrit_weight",
            "weight IS NULL OR weight BETWEEN 0 AND 10000"));
    }
}
