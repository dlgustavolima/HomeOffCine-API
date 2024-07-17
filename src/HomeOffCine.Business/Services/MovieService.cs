using HomeOffCine.Business.Interfaces;
using HomeOffCine.Business.Interfaces.Repository;
using HomeOffCine.Business.Interfaces.Service;
using HomeOffCine.Business.Models;

namespace HomeOffCine.Business.Services;

public class MovieService : BaseService, IMovieService
{
    private readonly IMovieRepository _movieRepository;

    public MovieService(
        IMovieRepository movieRepository,
        INotificator _notificator) : base(_notificator)
    {
        _movieRepository = movieRepository;
    }

    public async Task<Movie> GetMovieById(Guid id)
    {
        var movie = await _movieRepository.GetByIdAsync(id);

        if (movie == null) return null;

        movie.CalculateMediaRating();
        return movie;
    }

    public async Task<PagedResult<Movie>> GetMoviesPagination(int ps, int page, string q, string g)
    {
        return await _movieRepository.GetMoviesPagination(ps, page, q, g);
    }

    public async Task<string> AddMovie(Movie movie)
    {
        var validate = movie.Validate();
        if (!validate.IsValid)
        {
            Notificar(validate);
            return string.Empty;
        }

        _movieRepository.Add(movie);
        await _movieRepository.SaveChanges();
        return movie.Id.ToString();
    }

    public async Task UpdateMovie(Movie movie)
    {
        var validate = movie.Validate();
        if (!validate.IsValid)
        {
            Notificar(validate);
            return;
        }

        _movieRepository.Update(movie);
        await _movieRepository.SaveChanges();
    }

    public async Task DeleteMovie(Guid id)
    {
        var movie = await _movieRepository.GetByIdAsync(id);
        _movieRepository.Remove(movie);
        await _movieRepository.SaveChanges();
    }
}
