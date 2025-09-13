using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Modules.Evaluations.Management.Infrastructure.Persistence.Rows;
using CascVel.Modules.Evaluations.Management.Infrastructure.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Configurations.Rows;

/// <summary>
/// Entity Framework configuration for normalized form group rows.
/// Maps criteria as jsonb and configures the adjacency indexes.
/// </summary>
internal sealed class FormGroupRowConfiguration : IEntityTypeConfiguration<FormGroupRow>
{
    /// <summary>
    /// Configures mapping for FormGroupRow.
    /// </summary>
    public void Configure(EntityTypeBuilder<FormGroupRow> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable("form_groups", schema: "evaluations");

        var criteriaConv = new ValueConverter<FormCriteriaList, string>(
            v => FormCriteriaJson.Serialize(v),
            s => FormCriteriaJson.Deserialize(s));

        builder.HasKey(nameof(FormGroupRow.FormId), nameof(FormGroupRow.Id));

        builder.Property(x => x.FormId).HasColumnName("form_id");
        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.ParentId).HasColumnName("parent_id");
        builder.Property(x => x.Title).HasColumnName("title").IsRequired();
        builder.Property(x => x.Order).HasColumnName("order").IsRequired();
        builder.Property(x => x.Criteria)
            .HasColumnName("criteria")
            .HasColumnType("jsonb")
            .HasConversion(criteriaConv)
            .IsRequired();

        builder.HasIndex(x => new { x.FormId, x.ParentId });

        builder.HasOne<EvaluationForm>()
            .WithMany()
            .HasForeignKey(x => x.FormId);
    }

}
