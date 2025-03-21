using ChampionsLeagueMaster.Data;
using ChampionsLeagueMaster.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Dodaj us³ugê DbContext do kontenera DI
builder.Services.AddDbContext<ChampionsLeagueMasterContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ChampionsLeagueMasterContext")));


builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<ISeasonStatsRepository, SeasonStatsRepository>();
builder.Services.AddScoped<IResultRepository, ResultRepository>();



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
