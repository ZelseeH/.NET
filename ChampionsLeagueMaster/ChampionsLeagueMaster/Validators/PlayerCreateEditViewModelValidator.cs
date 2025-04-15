using ChampionsLeagueMaster.ViewModels.Players;
using FluentValidation;
using System.Text.RegularExpressions;

namespace ChampionsLeagueMaster.Validators
{
    public class PlayerCreateEditViewModelValidator : AbstractValidator<PlayerCreateEditViewModel>
    {
        public PlayerCreateEditViewModelValidator()
        {
            var nameRegex = new Regex(@"^[\p{L}\s'-]+$", RegexOptions.Compiled);

            RuleFor(p => p.FirstName)
                .NotEmpty().WithMessage("Imię jest wymagane")
                .Length(2, 50).WithMessage("Imię musi mieć od 2 do 50 znaków")
                .Matches(nameRegex).WithMessage("Imię zawiera niedozwolone znaki (cyfry/symbole)")
                .Must(BeProperlyCapitalized).WithMessage("Imię musi zaczynać się wielką literą");

            RuleFor(p => p.LastName)
                .NotEmpty().WithMessage("Nazwisko jest wymagane")
                .Length(2, 50).WithMessage("Nazwisko musi mieć od 2 do 50 znaków")
                .Matches(nameRegex).WithMessage("Nazwisko zawiera niedozwolone znaki (cyfry/symbole)")
                .Must(BeProperlyCapitalized).WithMessage("Nazwisko musi zaczynać się wielką literą");

            RuleFor(p => p.Position)
                .NotEmpty().WithMessage("Pozycja jest wymagana")
                .Matches(@"^[\p{L}\s-]+$").WithMessage("Pozycja zawiera niedozwolone znaki")
                .Must(BeProperlyCapitalized).WithMessage("Pozycja musi zaczynać się wielką literą");

            RuleFor(p => p.JerseyNumber)
                .NotNull().WithMessage("Numer koszulki jest wymagany")
                .InclusiveBetween(1, 99).WithMessage("Numer musi być między 1 a 99");

            RuleFor(p => p.BirthDate)
                .NotNull().WithMessage("Data urodzenia jest wymagana")
                .LessThan(DateOnly.FromDateTime(DateTime.Today.AddYears(-14)))
                .WithMessage("Zawodnik musi mieć co najmniej 14 lat");

            RuleFor(p => p.Country)
                .NotEmpty().WithMessage("Kraj pochodzenia jest wymazane wymagany")
                .Matches(@"^[\p{L}\s-]+$").WithMessage("Nazwa kraju zawiera niedozwolone znaki")
                .Must(BeProperlyCapitalized).WithMessage("Nazwa kraju musi zaczynać się wielką literą");
        }

        private bool BeProperlyCapitalized(string value)
        {
            if (string.IsNullOrEmpty(value)) return true;
            return char.IsUpper(value[0]) && value.Substring(1).All(c => char.IsLower(c) || char.IsWhiteSpace(c));
        }
    }
}
