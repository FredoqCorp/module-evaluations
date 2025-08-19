using CascVel.Module.Evaluations.Management.Domain.Entities.Runs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Configurations;

internal sealed class FormRunConfiguration : IEntityTypeConfiguration<FormRun>
{
    public void Configure(EntityTypeBuilder<FormRun> builder)
    {
        builder.ToTable("form_runs");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();

        ConfigureMeta(builder);
        ConfigureState(builder);
    }

    private static void ConfigureMeta(EntityTypeBuilder<FormRun> builder)
    {
        builder.ComplexProperty(x => x.Meta, meta =>
        {
            meta.ComplexProperty(m => m.Form, form =>
            {
                form.Property(f => f.FormId).HasColumnName("form_id").IsRequired();
                form.Property(f => f.FormCode).HasColumnName("form_code").HasColumnType("text").IsRequired();
            });
            meta.Property(m => m.RunFor).HasColumnName("run_for").HasMaxLength(128).IsRequired();
            meta.Property(m => m.SupervisorComment).HasColumnName("supervisor_comment").HasColumnType("text");
        });
    }

    private static void ConfigureState(EntityTypeBuilder<FormRun> builder)
    {
        builder.OwnsOne(x => x.State, state =>
        {
            ConfigureLifecycle(state);
            ConfigureContext(state);
            ConfigureResult(state);
            ConfigureAgreement(state);
        });
    }

    private static void ConfigureLifecycle(OwnedNavigationBuilder<FormRun, RunState> state)
    {
        state.OwnsOne(s => s.Lifecycle, lc =>
        {
            lc.OwnsOne(l => l.Launched, s =>
            {
                s.Property(x => x.UserId).HasColumnName("launched_by").HasMaxLength(100).IsRequired();
                s.Property(x => x.At).HasColumnName("launched_at").IsRequired();
            });
            lc.OwnsOne(l => l.FirstSaved, s =>
            {
                s.Property(x => x.UserId).HasColumnName("first_saved_by").HasMaxLength(100);
                s.Property(x => x.At).HasColumnName("first_saved_at");
            });
            lc.OwnsOne(l => l.LastSaved, s =>
            {
                s.Property(x => x.UserId).HasColumnName("last_saved_by").HasMaxLength(100);
                s.Property(x => x.At).HasColumnName("last_saved_at");
            });
            lc.OwnsOne(l => l.Published, s =>
            {
                s.Property(x => x.UserId).HasColumnName("published_by").HasMaxLength(100);
                s.Property(x => x.At).HasColumnName("published_at");
            });
        });
    }

    private static void ConfigureContext(OwnedNavigationBuilder<FormRun, RunState> state)
    {
        state.OwnsOne(s => s.Context, ctx =>
        {
            ctx.Property(c => c.Items)
                .HasColumnName("context")
                .HasColumnType("jsonb")
                .IsRequired();
        });
    }

    private static void ConfigureResult(OwnedNavigationBuilder<FormRun, RunState> state)
    {
        state.OwnsOne(s => s.Result, res =>
        {
            res.Property(r => r.CurrentTotal).HasColumnName("current_total");
            res.OwnsMany(r => r.Criteria, crit =>
            {
                crit.ToTable("run_criteria");
                crit.WithOwner().HasForeignKey("run_id");
                // Declare the shadow FK property with explicit type so it can be used in indexes
                crit.Property<long>("run_id");
                crit.Property<long>("id").ValueGeneratedOnAdd();
                crit.HasKey("id");

                crit.Property(c => c.CriterionId).HasColumnName("criterion_id").IsRequired();
                crit.Property(c => c.Skipped).HasColumnName("skipped").IsRequired();

                crit.OwnsOne(c => c.Assessment, a =>
                {
                    a.Property(x => x.SelectedScore).HasColumnName("selected_score");
                    a.Property(x => x.Comment).HasColumnName("comment").HasColumnType("text");
                    a.OwnsOne(x => x.Auto, auto =>
                    {
                        auto.Property(p => p.ParameterKey).HasColumnName("auto_parameter_key").HasColumnType("text");
                        auto.Property(p => p.Value).HasColumnName("auto_value");
                    });
                });

                crit.HasIndex("run_id", nameof(RunCriterionScore.CriterionId))
                    .HasDatabaseName("ix_run_criteria_unique")
                    .IsUnique();
            });
        });
    }

    private static void ConfigureAgreement(OwnedNavigationBuilder<FormRun, RunState> state)
    {
        state.OwnsOne(s => s.Agreement, agree =>
        {
            agree.Property(a => a.ViewedAt).HasColumnName("viewed_at");
            agree.Property(a => a.Status).HasColumnName("agreement_status").HasConversion<string>().HasMaxLength(16);
            agree.Property(a => a.DecidedAt).HasColumnName("decided_at");
        });
    }
}
