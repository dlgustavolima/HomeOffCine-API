using FluentValidation;
using FluentValidation.Results;

namespace HomeOffCine.Business.Models;

public class Rating : Entity
{
    public Guid MovieId { get; private set; }

    public Guid UserId { get; private set; }

    public string Description { get; private set; }

    public long Assessments { get; private set; }

    public DateTime RatingDate { get; private set; }

    public Movie Movie { get; private set; }

    public Rating(string description, long assessments, DateTime ratingDate, Guid movieId, Guid userId)
    {
        Description = description;
        Assessments = assessments;
        RatingDate = ratingDate;
        MovieId = movieId;
        UserId = userId;
    }

    public ValidationResult Validate()
    {
        return new RatingValidator().Validate(this);
    }

    public Rating() { }
}

public class RatingValidator : AbstractValidator<Rating>
{
    public static string DescriptionErrorMessage => "Avaliação sem descrição";

    public static string AssessmentsErrorMessage => "Avaliação sem ranqueamento";

    public static string RatingDateErrorMessage => "Avaliação não pode ser maior que a data atual";

    public static string MovieIdDateErrorMessage => "Avaliação sem Id do usuario";

    public RatingValidator()
    {
        RuleFor(p => p.Description)
            .NotEmpty()
            .WithMessage(DescriptionErrorMessage);

        RuleFor(p => p.Assessments)
            .NotEmpty()
            .WithMessage(AssessmentsErrorMessage);

        RuleFor(p => p.RatingDate)
            .Must(RatingDateNotGreaterThamDateTimeNow)
            .WithMessage(RatingDateErrorMessage);

        RuleFor(p => p.MovieId)
            .NotNull()
            .WithMessage(MovieIdDateErrorMessage);

        RuleFor(p => p.UserId)
            .NotNull()
            .WithMessage(MovieIdDateErrorMessage);
    }

    protected static bool RatingDateNotGreaterThamDateTimeNow(DateTime ratingDate)
    {
        return ratingDate < DateTime.Now;
    }
}