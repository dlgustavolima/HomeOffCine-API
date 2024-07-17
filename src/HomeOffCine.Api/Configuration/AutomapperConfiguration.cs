using AutoMapper;
using HomeOffCine.Api.ViewModels.Movie;
using HomeOffCine.Api.ViewModels.Pagination;
using HomeOffCine.Api.ViewModels.Rating;
using HomeOffCine.Business.Models;

namespace HomeOffCine.Api.Configuration
{
    public class AutomapperConfiguration : Profile
    {
        public AutomapperConfiguration()
        {
            CreateMap<Movie, MovieViewModel>().ReverseMap();
            CreateMap<Movie, AddMovieViewModel>().ReverseMap();
            CreateMap<Movie, EditMovieViewModel>().ReverseMap();

            CreateMap<Rating, RatingViewModel>().ReverseMap();
            CreateMap<Rating, AddRatingViewModel>().ReverseMap();
            
            CreateMap<PagedResult<Movie>, PagedViewModel<MovieViewModel>>().ReverseMap();
        }
    }
}
