using CascVel.Module.Evaluations.Management.Domain.Entities.EvaluationCriteria.Criteria;
using CascVel.Module.Evaluations.Management.Domain.Entities.Form;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Configurations;

internal sealed class EvaluationFormConfiguration : IEntityTypeConfiguration<EvaluationForm>
{
    public void Configure(EntityTypeBuilder<EvaluationForm> builder)
    {
        builder.ToTable("evaluation_forms");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Title).IsRequired().HasMaxLength(200).HasColumnName("title");
        builder.Property(x => x.Code).IsRequired().HasMaxLength(100).HasColumnName("code");
        builder.Property(x => x.Description).HasMaxLength(1000).HasColumnName("description");

        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(32).HasColumnName("status");
        builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(100).HasColumnName("created_by");
        builder.Property(x => x.CreatedAt).IsRequired().HasColumnName("created_at");
        builder.Property(x => x.StatusChangedBy).HasMaxLength(100).HasColumnName("status_changed_by");
        builder.Property(x => x.StatusChangedAt).HasColumnName("status_changed_at");
        builder.Property(x => x.ModifiedBy).HasMaxLength(100).HasColumnName("modified_by");
        builder.Property(x => x.ModifiedAt).HasColumnName("modified_at");
        builder.Property(x => x.ValidFrom).HasColumnName("valid_from");
        builder.Property(x => x.ValidUntil).HasColumnName("valid_until");
        builder.Property(x => x.CalculationRule).HasConversion<string>().HasMaxLength(32).HasColumnName("calculation_rule");

        // Store keywords as jsonb for flexibility
        builder.Property(x => x.FormKeywords).HasColumnType("jsonb").HasColumnName("form_keywords");

        // Link criteria to form via shadow FK
        builder.HasMany(x => x.FormCriteria)
               .WithOne()
               .HasForeignKey("EvaluationFormId")
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.Code).IsUnique();
        builder.HasIndex(x => x.Status);
    }
}
