using ChampionsLeagueMaster.ViewModels.Results;
using FluentValidation;

namespace ChampionsLeagueMaster.Validators
{
    public class ResultCreateEditViewModelValidator : AbstractValidator<ResultCreateEditViewModel>
    {
        public ResultCreateEditViewModelValidator()
        {
            RuleFor(r => r.HomeTeamId)
                .NotNull().WithMessage("Wybierz drużynę gospodarzy");

            RuleFor(r => r.AwayTeamId)
                .NotNull().WithMessage("Wybierz drużynę gości")
                .NotEqual(r => r.HomeTeamId).WithMessage("Drużyna gości nie może być taka sama jak gospodarzy");

            RuleFor(r => r.Season)
                .NotEmpty().WithMessage("Sezon jest wymagany")
                .Matches(@"^\d{4}$|^\d{4}/\d{4}$").WithMessage("Sezon musi mieć format '2024' lub '2023/2024'");

            RuleFor(r => r.Status)
                .NotEmpty().WithMessage("Status meczu jest wymagany")
                .Must(status => new[] { "Zaplanowany", "W trakcie", "Zakończony", "Odwołany" }.Contains(status))
                .WithMessage("Niepoprawny status meczu");

            RuleFor(r => r.MatchDay)
                .NotNull().WithMessage("Data meczu jest wymagana");

            RuleFor(r => r.MatchTime)
                .NotNull().WithMessage("Godzina meczu jest wymagana");

            RuleFor(r => r.Round)
                .NotEmpty().WithMessage("Runda meczu jest wymagana") 
                .Must(round => new[]
                {
                    "1. Kolejka", "2. Kolejka", "3. Kolejka",
                    "4. Kolejka", "5. Kolejka", "6. Kolejka",
                    "7. Kolejka", "8. Kolejka",
                    "1/16 finału", "1/8 finału",
                    "Ćwierćfinał", "Półfinał",
                    "Finał"
                }.Contains(round))
                .WithMessage("Niepoprawna runda meczu");

            When(r => r.Status == "Zaplanowany", () =>
            {
                RuleFor(r => r.HomeTeamGoals)
                    .Null().WithMessage("Nie można podawać wyniku dla meczu zaplanowanego");

                RuleFor(r => r.AwayTeamGoals)
                    .Null().WithMessage("Nie można podawać wyniku dla meczu zaplanowanego");

                RuleFor(r => r.MatchDay)
                    .Must(date => date >= DateOnly.FromDateTime(DateTime.Today))
                    .WithMessage("Zaplanowany mecz nie może mieć daty w przeszłości");
            });

            When(r => r.Status == "Odwołany", () =>
            {
                RuleFor(r => r.HomeTeamGoals)
                    .Null().WithMessage("Nie można podawać wyniku dla meczu odwołanego");

                RuleFor(r => r.AwayTeamGoals)
                    .Null().WithMessage("Nie można podawać wyniku dla meczu odwołanego");
            });

            When(r => r.Status == "W trakcie" || r.Status == "Zakończony", () =>
            {
                RuleFor(r => r.HomeTeamGoals)
                    .NotNull().WithMessage("Bramki gospodarza są wymagane")
                    .InclusiveBetween(0, 50).WithMessage("Bramki muszą być między 0 a 50");

                RuleFor(r => r.AwayTeamGoals)
                    .NotNull().WithMessage("Bramki gościa są wymagane")
                    .InclusiveBetween(0, 50).WithMessage("Bramki muszą być między 0 a 50");

                RuleFor(r => r.MatchDay)
                    .Must(date => date <= DateOnly.FromDateTime(DateTime.Today))
                    .WithMessage("Data zakończonego/trwającego meczu nie może być w przyszłości");
            });
        }
    }
}
