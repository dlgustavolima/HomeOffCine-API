using HomeOffCine.Business.Models;

namespace HomeOffCine.Business.Interfaces.Repository;

public interface IMovieRepository : IRepository<Movie>
{
    Task<PagedResult<Movie>> GetMoviesPagination(int ps, int page, string q, string g);

    Task<bool> VerifyIfMovieExistsById(Guid id);
}