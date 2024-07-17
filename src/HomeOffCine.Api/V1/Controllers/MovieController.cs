using Asp.Versioning;
using AutoMapper;
using HomeOffCine.Api.Controllers;
using HomeOffCine.Api.ViewModels.Movie;
using HomeOffCine.Api.ViewModels.Pagination;
using HomeOffCine.Business.Interfaces;
using HomeOffCine.Business.Interfaces.Service;
using HomeOffCine.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static HomeOffCine.Api.Extensions.CustomAuthorization;

namespace HomeOffCine.Api.V1.Controllers;

[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/movies")]
public class MovieController : MainController
{
    private readonly IMovieService _movieService;
    private readonly IMapper _mapper;

    public MovieController(IMovieService movieService,
                           IMapper mapper,
                           INotificator notificador,
                           IUser user) : base(notificador, user)
    {
        _movieService = movieService;
        _mapper = mapper;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<MovieViewModel>> GetById(Guid id)
    {
        var movieViewModel = _mapper.Map<MovieViewModel>(await _movieService.GetMovieById(id));

        if (movieViewModel is null) return NotFound();

        return CustomResponse(movieViewModel);
    }

    [HttpGet]
    public async Task<ActionResult<List<MovieViewModel>>> GetAll(int pageSize, int page, string query, string gender)
    {
        if (pageSize <= 0)
        {
            NotificarErro("Tamanho da pagina não pode ser menor a 0");
            return CustomResponse();
        }

        if (page <= 0)
        {
            NotificarErro("Pagina não pode ser menor a 0");
            return CustomResponse();
        }

        var moviesViewModel = _mapper.Map<PagedViewModel<MovieViewModel>>(await _movieService.GetMoviesPagination(pageSize, page, query, gender));

        if (moviesViewModel is null) return NotFound();

        return CustomResponse(moviesViewModel);
    }

    [ClaimsAuthorize("Filme", "Adm")]
    [HttpPost]
    public async Task<IActionResult> Add([FromForm] AddMovieViewModel movieViewModel)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var imgPrefixo = Guid.NewGuid() + "_";
        var imgBannerPrefixo = Guid.NewGuid() + "_";
        if (!await UploadFile(movieViewModel.ImagemUpload, imgPrefixo))
        {
            return CustomResponse(movieViewModel);
        }

        movieViewModel.Image = imgPrefixo + movieViewModel.ImagemUpload.FileName;

        if (!await UploadFile(movieViewModel.ImageBannerUpload, imgBannerPrefixo))
        {
            return CustomResponse(movieViewModel);
        }

        movieViewModel.ImageBanner = imgBannerPrefixo + movieViewModel.ImageBannerUpload.FileName;

        var movie = new Movie(movieViewModel.Name, movieViewModel.Description, movieViewModel.Gender, movieViewModel.Imdb, movieViewModel.ReleaseDate, movieViewModel.Image, movieViewModel.ImageBanner, movieViewModel.UrlTrailer);
        var movieId = await _movieService.AddMovie(movie);
        movieViewModel.Id = Guid.Parse(movieId);

        return CustomResponse(movieViewModel);
    }

    [ClaimsAuthorize("Filme", "Adm")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Edit(Guid id, [FromForm] EditMovieViewModel editMovieViewModel)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        var movie = await _movieService.GetMovieById(id);
        if (movie is null) return BadRequest();

        if (editMovieViewModel.ImagemUpload != null)
        {
            var imgPrefixo = Guid.NewGuid() + "_";
            if (!await UploadFile(editMovieViewModel.ImagemUpload, imgPrefixo))
            {
                return CustomResponse(editMovieViewModel);
            }
            DeleteFile(movie.Image);
            editMovieViewModel.Image = imgPrefixo + editMovieViewModel.ImagemUpload.FileName;
            editMovieViewModel.Image = editMovieViewModel.Image;
        }

        if (editMovieViewModel.ImageBannerUpload != null)
        {
            var imgBannerPrefixo = Guid.NewGuid() + "_";
            if (!await UploadFile(editMovieViewModel.ImageBannerUpload, imgBannerPrefixo))
            {
                return CustomResponse(editMovieViewModel);
            }
            DeleteFile(movie.ImageBanner);
            editMovieViewModel.Image = imgBannerPrefixo + editMovieViewModel.ImageBannerUpload.FileName;
            editMovieViewModel.Image = editMovieViewModel.Image;
        }

        movie.UpdateMovie(editMovieViewModel.Name, editMovieViewModel.Description, editMovieViewModel.Gender, editMovieViewModel.Imdb, editMovieViewModel.ReleaseDate, editMovieViewModel.Image ?? movie.Image, editMovieViewModel.ImageBanner ?? movie.ImageBanner, editMovieViewModel.UrlTrailer);
        await _movieService.UpdateMovie(movie);
        return CustomResponse(editMovieViewModel);
    }

    [ClaimsAuthorize("Filme", "Adm")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var movie = await _movieService.GetMovieById(id);

        if (movie is null) return NotFound();

        DeleteFile(movie.Image);
        DeleteFile(movie.ImageBanner);
        await _movieService.DeleteMovie(id);

        return CustomResponse(id);
    }

    private async Task<bool> UploadFile(IFormFile arquivo, string imgPrefixo)
    {
        if (arquivo.Length <= 0) return false;

        var path = Directory.GetCurrentDirectory() + "\\Images";

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        path = Path.Combine(path, imgPrefixo + arquivo.FileName);

        if (System.IO.File.Exists(path))
        {
            ModelState.AddModelError(string.Empty, "Já existe um arquivo com este nome!");
            return false;
        }

        using (var stream = new FileStream(path, FileMode.Create))
        {
            await arquivo.CopyToAsync(stream);
        }

        return true;
    }

    private void DeleteFile(string imageName)
    {
        if (string.IsNullOrEmpty(imageName)) return;

        var path = Path.Combine(Directory.GetCurrentDirectory() + "\\Images", imageName);

        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
        }
    }
}
