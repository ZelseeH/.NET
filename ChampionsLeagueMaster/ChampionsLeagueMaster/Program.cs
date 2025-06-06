using ChampionsLeagueMaster.Data;
using ChampionsLeagueMaster.Models;
using ChampionsLeagueMaster.Repository;
using ChampionsLeagueMaster.Services;
using ChampionsLeagueMaster.Validators;
using ChampionsLeagueMaster.ViewModels.Players;
using ChampionsLeagueMaster.ViewModels.Results;
using ChampionsLeagueMaster.ViewModels.Teams;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Zmiana z AddDbContext na AddDbContextPool z ServiceLifetime.Transient
builder.Services.AddDbContext<ChampionsLeagueMasterContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ChampionsLeagueMasterContext")),
    ServiceLifetime.Transient);

//repository
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<ISeasonStatsRepository, SeasonStatsRepository>();
builder.Services.AddScoped<IResultRepository, ResultRepository>();
//service
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IResultService, ResultService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<ISeasonStatsService, SeasonStatsService>();
//Validators
builder.Services.AddScoped<IValidator<PlayerCreateEditViewModel>, PlayerCreateEditViewModelValidator>();
builder.Services.AddScoped<IValidator<TeamCreateEditViewModel>, TeamCreateEditViewModelValidator>();
builder.Services.AddScoped<IValidator<ResultCreateEditViewModel>, ResultCreateEditViewModelValidator>();


builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
