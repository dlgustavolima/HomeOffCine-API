using HomeOffCine.Business.Models;

namespace HomeOffCine.Business.Interfaces.Service;

public interface IMovieService
{
    Task<Movie> GetMovieById(Guid id);

    Task<PagedResult<Movie>> GetMoviesPagination(int ps, int page, string q, string g);

    Task<string> AddMovie(Movie movie);

    Task UpdateMovie(Movie movie);

    Task DeleteMovie(Guid id);

}
