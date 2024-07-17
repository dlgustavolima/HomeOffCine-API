using HomeOffCine.Business.Interfaces.Repository;
using HomeOffCine.Business.Models;
using HomeOffCine.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace HomeOffCine.Infra.Repository;

public class MovieRepository : Repository<Movie>, IMovieRepository
{
    public MovieRepository(HomeOffCineDbContext context) : base(context)
    {

    }

    public async Task<PagedResult<Movie>> GetMoviesPagination(int ps, int page, string q, string g)
    {
        var query = _dbSet.AsQueryable();

        if (page > 1)
            query = query.Skip(ps);

        if (ps > 0)
            query = query.Take(ps);

        if (!string.IsNullOrEmpty(q))
        {
            query = query.Where(p => p.Name.Contains(q));
        }

        else if (!string.IsNullOrEmpty(g))
        {
            query = query.Where(p => p.Gender.Equals(g));
        }

        var movies = await query.ToListAsync();
        var total = Db.Movies.Count();

        return new PagedResult<Movie>
        {
            List = movies,
            TotalResults = total,
            PageIndex = page,
            PageSize = ps,
            Query = q
        };
    }

    public override async Task<Movie> GetByIdAsync(Guid id)
    {
        return await Db.Movies
            .Where(p => p.Id == id)
            .Include(p => p.Rating)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> VerifyIfMovieExistsById(Guid id)
    {
        var movie = await Db.Movies
            .FirstOrDefaultAsync<Movie>(p => p.Id == id);

        if (movie == null) return false;
        else return true;
    }
}
