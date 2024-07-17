using FluentValidation;
using FluentValidation.Results;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeOffCine.Business.Models;

public class Movie : Entity
{
    public string Name { get; private set; }

    public string Description { get; private set; }

    public string Gender { get; private set; }

    public long Imdb { get; private set; }

    public DateTime ReleaseDate { get; private set; }

    public string Image { get; private set; }

    public string ImageBanner { get; private set; }

    public string UrlTrailer { get; private set; }

    [NotMapped]
    public decimal MediaRating { get; private set; }

    private readonly List<Rating> _rating;

    public IReadOnlyCollection<Rating> Rating => _rating;

    public Movie(string name, string description, string gender, long imdb, DateTime releaseDate, string image, string imageBanner, string urlTrailer)
    {
        Name = name;
        Description = description;
        Gender = gender;
        Imdb = imdb;
        ReleaseDate = releaseDate;
        Image = image;
        ImageBanner = imageBanner;
        UrlTrailer = urlTrailer;
        _rating = new List<Rating>();
    }

    public Movie() { }

    public void UpdateMovie(string name, string description, string gender, long imdb, DateTime releaseDate, string image, string imageBanner, string urlTrailer)
    {
        Name = name;
        Description = description;
        Gender = gender;
        Imdb = imdb;
        ReleaseDate = releaseDate;
        Image = image;
        ImageBanner = imageBanner;
        UrlTrailer = urlTrailer;
    }

    public void CalculateMediaRating()
    {
        if (Rating == null || Rating.Count <= 0) return;

        MediaRating = Rating.Sum(p => p.Assessments) / Rating.Count;
    }

    public ValidationResult Validate()
    {
        return new MovieValidator().Validate(this);
    }
}

public class MovieValidator : AbstractValidator<Movie>
{
    public static string NameErrorMessage => "Filme sem nome";

    public static string DescriptionErrorMessage => "Filme sem descrição";

    public static string GenderErrorMessage => "Filme sem genero";

    public static string ImdbErrorMessage => "Nota do IMDB não pode ser menor que 0";

    public static string ReleaseDateErrorMessage => "Data de lançamento não pode ser maior que a data atual";

    public static string ImageErrorMessage => "Filme sem imagem";

    public MovieValidator()
    {
        RuleFor(p => p.Name)
            .NotNull().WithMessage(NameErrorMessage)
            .NotEmpty().WithMessage(NameErrorMessage);

        RuleFor(p => p.Description)
            .NotNull().WithMessage(DescriptionErrorMessage)
            .NotEmpty().WithMessage(DescriptionErrorMessage);

        RuleFor(p => p.Gender)
            .NotNull().WithMessage(GenderErrorMessage)
            .NotEmpty().WithMessage(GenderErrorMessage);

        RuleFor(p => p.Imdb)
            .GreaterThan(0)
            .WithMessage(ImdbErrorMessage);

        RuleFor(p => p.ReleaseDate)
            .Must(ReleaseDateNotGreaterThamDateTimeNow)
            .WithMessage(ReleaseDateErrorMessage);

        RuleFor(p => p.Image)
            .NotEmpty()
            .WithMessage(ImageErrorMessage);
    }

    protected static bool ReleaseDateNotGreaterThamDateTimeNow(DateTime releaseDate)
    {
        return releaseDate < DateTime.Now;
    }

}