using HomeOffCine.Business.Models;

namespace HomeOffCine.Business.Interfaces.Repository;

public interface IRatingRepository : IRepository<Rating>
{
    Task<Rating> GetRatingByIdNoTracking(Guid id);
    Task<List<Rating>> GetRatingsByMovieId(Guid movieId);
    Task<List<Rating>> GetRatingsByUserId(Guid userId);
    Task<List<Rating>> GetRatingsByMovieIdAndUserId(Guid movieId, Guid userId);
}
