using HomeOffCine.Business.Models;

namespace HomeOffCine.Business.Interfaces.Service;

public interface IRatingService
{
    Task<Rating> GetRatingById(Guid id);

    Task<Rating> GetRatingByIdNoTracking(Guid id);

    Task<List<Rating>> GetRatingsByUserId(Guid userId);

    Task<List<Rating>> GetRatingsByMovieId(Guid movieId);

    Task<string> AddRating(Rating rating);

    Task UpdateRating(Rating rating);

    Task DeleteRating(Guid id);
}
