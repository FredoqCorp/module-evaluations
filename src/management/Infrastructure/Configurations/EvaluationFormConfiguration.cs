using System;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Configurations;

/// <summary>
/// Configures the EvaluationForm entity for EF Core mapping.
/// Usage: Add this configuration in the DbContext's OnModelCreating method to apply entity settings.
/// </summary>
public sealed class EvaluationFormConfiguration : IEntityTypeConfiguration<EvaluationForm>
{
    /// <summary>
    /// Configures the EF Core mapping for the EvaluationForm entity.
    /// Usage: Called by the DbContext during model creation to apply entity configuration.
    /// </summary>
    /// <param name="builder">The builder used to configure the EvaluationForm entity.</param>
    public void Configure(EntityTypeBuilder<EvaluationForm> builder)
    {
        builder.ToTable("forms");

        

        
    }
}
