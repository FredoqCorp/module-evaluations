using CascVel.Module.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Module.Evaluations.Management.Domain.Entities.Forms.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Configurations;

internal sealed class FormGroupConfiguration : IEntityTypeConfiguration<FormGroup>
{
    public void Configure(EntityTypeBuilder<FormGroup> builder)
    {
        builder.ToTable("form_groups", t =>
        {
            t.HasCheckConstraint("ck_form_groups_order_non_negative", "order_index >= 0");
        });

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
               .HasColumnName("id")
               .ValueGeneratedOnAdd();

        builder.Property<long>("form_id").HasColumnName("form_id");

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
                           v => v == null ? null : new Weight(v.Value / 100m))
                .HasColumnName("weight")
                .HasColumnType("smallint")
                .IsRequired(false);

        builder.ToTable(t => t.HasCheckConstraint("CK_form_group_weight", "weight IS NULL OR weight BETWEEN 0 AND 10000"));

        // Self-nesting: parent_id optional
        builder.Property<long?>("parent_id").HasColumnName("parent_id");
        builder.HasOne<FormGroup>()
               .WithMany(x => x.Groups)
               .HasForeignKey("parent_id")
               .OnDelete(DeleteBehavior.Cascade);

        // Criteria inside this group: link via group_id
        builder.HasMany(x => x.Criteria)
               .WithOne()
               .HasForeignKey("group_id")
               .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex("form_id").HasDatabaseName("ix_form_groups_fk_form");
        builder.HasIndex("parent_id").HasDatabaseName("ix_form_groups_parent");
        //builder.HasIndex("order_index").HasDatabaseName("ix_form_groups_order");
    }
}
