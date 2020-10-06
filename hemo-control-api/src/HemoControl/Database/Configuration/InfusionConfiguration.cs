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
                .HasColumnType("decimal(19,5)");

            builder.OwnsOne(i => i.Factor, factorBuilder =>
            {
                factorBuilder.Property(f => f.Unity)
                    .HasColumnName("FactorUnity")
                    .IsRequired();

                factorBuilder.Property(i => i.Brand)
                    .HasMaxLength(50)
                    .HasColumnName("FactorBrand")
                    .IsRequired();

                factorBuilder.Property(i => i.Lot)
                    .HasMaxLength(20)
                    .HasColumnName("FactorLot")
                    .IsRequired();
            });

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