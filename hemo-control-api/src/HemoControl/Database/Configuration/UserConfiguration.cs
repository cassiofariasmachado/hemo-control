using HemoControl.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HemoControl.Database.Configuration
{

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(u => u.LastName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.Username)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.Password)
                .HasMaxLength(60)
                .IsRequired();

            builder.Property(u => u.Email)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(u => u.Weigth)
                .HasColumnType("decimal(19,5)");

            builder.HasMany(u => u.Infusions)
                .WithOne(i => i.User);

            builder.Metadata.FindNavigation("Infusions")
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }

}