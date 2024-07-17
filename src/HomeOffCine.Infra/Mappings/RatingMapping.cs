using HomeOffCine.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeOffCine.Infra.Mappings;

public class RatingMapping : IEntityTypeConfiguration<Rating>
{
    public void Configure(EntityTypeBuilder<Rating> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(p => p.Description)
            .IsRequired()
            .HasColumnType("varchar(1000)");

        builder.Property(p => p.Assessments)
            .IsRequired()
            .HasColumnType("decimal(7,2)");

        builder.Property(p => p.RatingDate)
            .IsRequired();

        builder.HasOne(p => p.Movie)
            .WithMany(p => p.Rating);
    }
}
