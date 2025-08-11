using CascVel.Module.Evaluations.Management.Domain.Entities.AutomaticParameters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Configurations;

internal sealed class AutomaticParameterConfiguration : IEntityTypeConfiguration<AutomaticParameter>
{
    public void Configure(EntityTypeBuilder<AutomaticParameter> builder)
    {
        builder.ToTable("automatic_parameters");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Caption).IsRequired().HasMaxLength(200).HasColumnName("caption");
        builder.Property(x => x.ConditionType).HasConversion<string>().HasMaxLength(32).HasColumnName("condition_type");

        // Complex type for ServiceCode
        builder.ComplexProperty(x => x.ServiceCode, svc =>
        {
            svc.Property(p => p.Value).HasColumnName("service_code").HasMaxLength(50);
        });

        builder.HasIndex(x => x.Caption);
        builder.HasIndex(x => x.ConditionType);
    }
}
