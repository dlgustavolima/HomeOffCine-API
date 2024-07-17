using HomeOffCine.Business.Interfaces;
using HomeOffCine.Business.Interfaces.Repository;
using HomeOffCine.Business.Interfaces.Service;
using HomeOffCine.Business.Models;

namespace HomeOffCine.Business.Services;

public class RatingService : BaseService, IRatingService
{
    private readonly IRatingRepository _ratingRepository;
    private readonly IMovieRepository _movieRepository;

    public RatingService(IRatingRepository ratingRepository,
                         IMovieRepository movieRepository,
                         INotificator notificator) : base(notificator)
    {
        _ratingRepository = ratingRepository;
        _movieRepository = movieRepository;
    }

    public async Task<Rating> GetRatingById(Guid id)
    {
        return await _ratingRepository.GetByIdAsync(id);
    }

    public async Task<Rating> GetRatingByIdNoTracking(Guid id)
    {
        return await _ratingRepository.GetRatingByIdNoTracking(id);
    }

    public async Task<List<Rating>> GetRatingsByUserId(Guid userId)
    {
        return await _ratingRepository.GetRatingsByUserId(userId);
    }

    public async Task<List<Rating>> GetRatingsByMovieId(Guid movieId)
    {
        return await _ratingRepository.GetRatingsByMovieId(movieId);
    }

    public async Task<string> AddRating(Rating rating)
    {
        var validate = rating.Validate();
        if (!validate.IsValid)
        {
            Notificar(validate);
            return string.Empty;
        }

        var movie = await _movieRepository.VerifyIfMovieExistsById(rating.MovieId);
        if (movie == false) 
        {
            Notificar($"Não encontramos o filme com esse Id: {rating.MovieId}");
            return string.Empty;
        }

        _ratingRepository.Add(rating);
        await _ratingRepository.SaveChanges();
        return rating.Id.ToString();
    }

    public async Task UpdateRating(Rating rating)
    {
        var validate = rating.Validate();
        if (!validate.IsValid)
        {
            Notificar(validate);
            return;
        }

        var movie = await _movieRepository.VerifyIfMovieExistsById(rating.MovieId);
        if (movie == false)
        {
            Notificar($"Não encontramos o filme com esse Id: {rating.MovieId}");
            return;
        }

        _ratingRepository.Update(rating);
        await _ratingRepository.SaveChanges();
    }

    public async Task DeleteRating(Guid id)
    {
        var rating = await _ratingRepository.GetByIdAsync(id);
        _ratingRepository.Remove(rating);
        await _ratingRepository.SaveChanges();
    }

}
