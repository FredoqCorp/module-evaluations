using CascVel.Module.Evaluations.Management.Domain.Entities.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Configurations;

internal sealed class FormGroupConfiguration : IEntityTypeConfiguration<FormGroup>
{
    public void Configure(EntityTypeBuilder<FormGroup> builder)
    {
        builder.ToTable("form_groups");
        builder.HasKey(g => g.Id);
        builder.Property(g => g.Id).HasColumnName("id").ValueGeneratedOnAdd();

        builder.Property(g => g.Title)
            .HasColumnName("title")
            .HasColumnType("text")
            .IsRequired();

        builder.ComplexProperty(g => g.Order, order =>
        {
            order.Property(o => o.Value).HasColumnName("order_index").IsRequired();
        });

        builder.ComplexProperty(g => g.Weight, weight =>
        {
            weight.Property(w => w.Percent).HasColumnName("weight");
        });

        builder.Property<long?>("form_id").HasColumnName("form_id");
        builder.Property<long?>("parent_group_id").HasColumnName("parent_group_id");

        builder.HasOne<EvaluationForm>()
            .WithMany()
            .HasForeignKey("form_id")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<FormGroup>()
            .WithMany(g => g.Groups)
            .HasForeignKey("parent_group_id")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(g => g.Criteria)
            .WithOne()
            .HasForeignKey("group_id")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasCheckConstraint("ck_form_groups_owner", "((form_id IS NULL) <> (parent_group_id IS NULL))");

        builder.HasIndex("form_id", "Order_Value")
            .HasDatabaseName("ix_form_groups_root_order_unique")
            .IsUnique()
            .HasFilter("parent_group_id IS NULL");

        builder.HasIndex("parent_group_id", "Order_Value")
            .HasDatabaseName("ix_form_groups_nested_order_unique")
            .IsUnique()
            .HasFilter("parent_group_id IS NOT NULL");
    }
}

