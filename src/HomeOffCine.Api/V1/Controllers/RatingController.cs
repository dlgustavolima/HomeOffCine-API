using Asp.Versioning;
using AutoMapper;
using HomeOffCine.Api.Controllers;
using HomeOffCine.Api.ViewModels.Rating;
using HomeOffCine.Business.Interfaces;
using HomeOffCine.Business.Interfaces.Service;
using HomeOffCine.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeOffCine.Api.V1.Controllers;

[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/rating")]
public class RatingController : MainController
{
    private readonly IRatingService _ratingService;
    private readonly IMapper _mapper;

    public RatingController(IRatingService ratingService,
                            IMapper mapper,
                            INotificator notificador,
                            IUser user) : base(notificador, user)
    {
        _ratingService = ratingService;
        _mapper = mapper;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var ratingViewModel = _mapper.Map<RatingViewModel>(await _ratingService.GetRatingById(id));
        if (ratingViewModel == null) return NotFound();

        return CustomResponse(ratingViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> GetByUserId(Guid userId)
    {
        var ratingsViewModel = _mapper.Map<List<RatingViewModel>>(await _ratingService.GetRatingsByUserId(userId));
        if (ratingsViewModel == null || ratingsViewModel.Count == 0) return NotFound();

        return CustomResponse(ratingsViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddRatingViewModel ratingViewModel)
    {
        if (!ModelState.IsValid) return RedirectToAction("WatchMovie", "Movie", new { id = ratingViewModel.MovieId });

        var rating = new Rating(ratingViewModel.Description, ratingViewModel.Assessments, DateTime.Now, ratingViewModel.MovieId, UserId);

        var ratingId = await _ratingService.AddRating(rating);

        return CustomResponse(Guid.Parse(ratingId));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Edit(Guid id, RatingViewModel ratingViewModel)
    {
        var rating = await _ratingService.GetRatingByIdNoTracking(id);
        if (rating is null) return BadRequest();

        if (UserId != rating.UserId)
        {
            NotificarErro("Não é permitido atualizar o comentario de outro usuario");
            CustomResponse(ratingViewModel);
        }

        var ratingUpdate = new Rating(ratingViewModel.Description, ratingViewModel.Assessments, rating.RatingDate, ratingViewModel.MovieId, UserId);
        ratingUpdate.Id = ratingViewModel.Id;
        await _ratingService.UpdateRating(ratingUpdate);
        return CustomResponse(ratingViewModel);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var rating = await _ratingService.GetRatingById(id);

        if (rating is null) return NotFound();

        await _ratingService.DeleteRating(id);

        return CustomResponse(id);
    }
}
