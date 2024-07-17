using System.ComponentModel.DataAnnotations;

namespace HomeOffCine.Api.ViewModels.Rating;

public class AddRatingViewModel
{
    public Guid MovieId { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [StringLength(1000, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
    public string Description { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [Range(0, 10)]
    public long Assessments { get; set; }
}
