using HemoControl.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HemoControl.Database.Configuration
{
    public class InfusionConfiguration : IEntityTypeConfiguration<Infusion>
    {
        public void Configure(EntityTypeBuilder<Infusion> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Date)
                .IsRequired();

            builder.Property(i => i.UserWeigth)
                .HasColumnType("decimal(19,5)")
                .IsRequired();

            builder.Property(i => i.FactorUnity)
                .IsRequired();

            builder.Property(i => i.FactorBrand)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(i => i.FactorLot)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(i => i.IsHemarthrosis)
                .IsRequired();

            builder.Property(i => i.IsBleeding)
                .IsRequired();

            builder.Property(i => i.IsTreatmentContinuation)
                .IsRequired();

            builder.Property(i => i.BleedingLocal)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(i => i.TreatmentLocal)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}