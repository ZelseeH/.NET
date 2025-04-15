using FluentValidation;
using ChampionsLeagueMaster.ViewModels.Teams;

namespace ChampionsLeagueMaster.Validators
{
    public class TeamCreateEditViewModelValidator : AbstractValidator<TeamCreateEditViewModel>
    {
        public TeamCreateEditViewModelValidator()
        {
            RuleFor(t => t.Name)
                .NotEmpty().WithMessage("Nazwa zespołu jest wymagana")
                .Length(2, 100).WithMessage("Nazwa zespołu musi mieć od 2 do 100 znaków");

            RuleFor(t => t.Country)
                .NotEmpty().WithMessage("Kraj jest wymagany")
                .Length(2, 60).WithMessage("Nazwa kraju musi mieć od 2 do 60 znaków")
                .Matches(@"^[A-Za-zÀ-ÖØ-öø-ÿ\s-]+$").WithMessage("Nazwa kraju nie może zawierać cyfr ani znaków specjalnych");

            RuleFor(t => t.FoundedAt)
                .InclusiveBetween(1800, 2025).WithMessage("Rok założenia musi być między 1800 a 2025");
        }
    }
}
