using Biletall.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Biletall.DataAccess.EntityFramework.Configurations.Concrete
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {

            builder.Property(x => x.AspNetUserId)
               .HasColumnType("nvarchar(450)")
               .HasMaxLength(450)
               .IsRequired();

            builder.Property(x => x.Nereden)
               .HasColumnType("nvarchar(50)")
               .HasMaxLength(50)
               .IsRequired();

            builder.Property(x => x.Nereye)
              .HasColumnType("nvarchar(50)")
              .HasMaxLength(50)
              .IsRequired();

            builder.Property(x => x.SeyahatTarihi)
              .HasColumnType("nvarchar(100)")
              .HasMaxLength(50)
              .IsRequired();

            builder.Property(x => x.PnrNo)
              .HasColumnType("nvarchar(50)")
              .HasMaxLength(50)
              .IsRequired();

            builder.Property(x => x.YaklasikSeyehatSuresi)
              .HasColumnType("nvarchar(50)")
              .HasMaxLength(50)
              .IsRequired();

            builder.Property(x => x.Ucret)
              .HasColumnType("nvarchar(50)")
              .HasMaxLength(50)
              .IsRequired();

            builder.Property(x => x.Durum)
              .HasColumnType("bit")
              .HasDefaultValue(0)
              .IsRequired();

            builder.Property(x => x.KoltukNo)
              .HasColumnType("int")
              .IsRequired();

            builder.Property(x => x.EBiletNo)
              .HasColumnType("nvarchar(50)")
              .HasMaxLength(50)
              .IsRequired();

            builder.Property(x => x.ServisIstegi)
             .HasColumnType("bit")
             .HasDefaultValue(0)
             .IsRequired();

            builder.Property(x => x.BiletIslemlerim)
              .HasColumnType("nvarchar(50)")
              .HasMaxLength(50)
              .IsRequired();

            builder.Property(x => x.SeferTipi)
              .HasColumnType("nvarchar(50)")
              .HasMaxLength(50)
              .IsRequired();

            builder.Property(x => x.SeferNo)
              .HasColumnType("nvarchar(50)")
              .HasMaxLength(50)
              .IsRequired();

            builder.Property(x => x.Peron)
              .HasColumnType("int")
              .IsRequired();

            builder.HasOne(x => x.ApplicationUser)
              .WithMany(x => x.Reservations)
              .HasForeignKey(x => x.AspNetUserId)
              .HasConstraintName("FK_Reservations_ApplicationUser")
              .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
