using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Configurations;

internal sealed class FormGroupConfiguration : IEntityTypeConfiguration<FormGroup>
{
    public void Configure(EntityTypeBuilder<FormGroup> builder)
    {
        builder.ToTable("form_groups",
            t => t.HasCheckConstraint("ck_form_groups_order_non_negative", "order_index >= 0"));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        // Title
        builder.Property(x => x.Title)
            .HasColumnName("title")
            .HasColumnType("text")
            .IsRequired();

        // Order
        builder.ComplexProperty(x => x.Order, order =>
        {
            order.Property(o => o.Value)
                .HasColumnName("order_index")
                .HasColumnType("integer")
                .IsRequired();
        });

        // Weight
        builder.Property(x => x.Weight)
            .HasConversion(v => v == null ? (ushort?)null : v.Bps(),
                v => v == null ? null : new Weight(v.Value))
            .HasColumnName("weight")
            .HasColumnType("smallint")
            .IsRequired(false);

        builder.ToTable(t =>
            t.HasCheckConstraint("CK_form_group_weight", "weight IS NULL OR weight BETWEEN 0 AND 10000"));

        builder.Property(x => x.FormId).HasColumnName("form_id").IsRequired();
        builder.Property(x => x.ParentId).HasColumnName("parent_id");

        builder.Navigation(x => x.Groups).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(x => x.Criteria).UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasOne(x => x.Form)
            .WithMany(f => f.Groups)
            .HasForeignKey(x => x.FormId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Parent)
            .WithMany(x => x.Groups)
            .HasForeignKey(x => x.ParentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Criteria)
            .WithOne()
            .HasForeignKey("group_id")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
