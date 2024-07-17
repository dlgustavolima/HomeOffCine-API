using HomeOffCine.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HomeOffCine.Infra.Mappings;

public class MovieMapping : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasColumnType("varchar(200)");

        builder.Property(p => p.Description)
            .IsRequired()
            .HasColumnType("varchar(5000)");

        builder.Property(p => p.Gender)
            .IsRequired()
            .HasColumnType("varchar(100)");

        builder.Property(p => p.Imdb)
            .IsRequired();

        builder.Property(p => p.ReleaseDate)
            .IsRequired();

        builder.Property(p => p.Image)
            .IsRequired()
            .HasColumnType("varchar(1000)");

        builder.Property(p => p.ImageBanner)
            .HasColumnType("varchar(1000)");

        builder.Property(p => p.UrlTrailer)
            .HasColumnType("varchar(1000)");

        builder.HasMany(p => p.Rating)
            .WithOne(p => p.Movie)
            .HasForeignKey(p => p.MovieId);
    }
}
