using HomeOffCine.Business.Interfaces.Repository;
using HomeOffCine.Business.Models;
using HomeOffCine.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace HomeOffCine.Infra.Repository;

public class RatingRepository : Repository<Rating>, IRatingRepository
{
    public RatingRepository(HomeOffCineDbContext context) : base(context)
    {

    }

    public async Task<Rating> GetRatingByIdNoTracking(Guid id)
    {
        return await Db.Ratings.AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public Task<List<Rating>> GetRatingsByMovieId(Guid movieId)
    {
        return Db.Ratings.Where(p => p.MovieId == movieId)
            .ToListAsync();
    }

    public Task<List<Rating>> GetRatingsByUserId(Guid userId)
    {
        return Db.Ratings.Where(p => p.UserId == userId)
            .ToListAsync();
    }

    public async Task<List<Rating>> GetRatingsByMovieIdAndUserId(Guid movieId, Guid userId)
    {
        return await Db.Ratings.Where(p => p.MovieId == movieId && p.UserId == userId)
            .ToListAsync();
    }
}
