using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using ChampionsLeagueMaster.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ChampionsLeagueMaster.Data
{
    public class ChampionsLeagueMasterContext : DbContext
    {
        public ChampionsLeagueMasterContext(DbContextOptions<ChampionsLeagueMasterContext> options)
            : base(options)
        {
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<SeasonStats> SeasonStats { get; set; }
        public DbSet<Result> Results { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Result>()
                .HasOne(r => r.HomeTeam)
                .WithMany(t => t.HomeMatches)
                .HasForeignKey(r => r.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Result>()
                .HasOne(r => r.AwayTeam)
                .WithMany(t => t.AwayMatches)
                .HasForeignKey(r => r.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Player>()
                .HasOne(p => p.Team)
                .WithMany(t => t.Players)
                .HasForeignKey(p => p.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SeasonStats>()
                .HasOne(s => s.Team)
                .WithMany(t => t.SeasonStats)
                .HasForeignKey(s => s.TeamId);

            // Jeśli chcesz, aby GoalDifference była kolumną obliczaną w bazie:
            modelBuilder.Entity<SeasonStats>()
                .Property(s => s.GoalDifference)
                .HasComputedColumnSql("[GoalsScored] - [GoalsConceded]");
        }

        // Przeciążamy zarówno SaveChanges, jak i SaveChangesAsync
        public override int SaveChanges()
        {
            UpdateSeasonStatsIfNeeded();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateSeasonStatsIfNeeded();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateSeasonStatsIfNeeded()
        {
            var results = ChangeTracker.Entries<Result>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                .Select(e => e.Entity)
                .ToList();

            if (results.Any())
            {
                UpdateSeasonStats(results);
            }
        }

        private void UpdateSeasonStats(List<Result> results)
        {
            foreach (var result in results)
            {
                if (string.IsNullOrEmpty(result.Season))
                    continue;

                // Przetwarzamy statystyki dla obu drużyn
                var teams = new[] { result.HomeTeamId, result.AwayTeamId };
                foreach (var teamId in teams)
                {
                    if (teamId == null)
                        continue;

                    // Szukamy istniejących statystyk dla danej drużyny i sezonu
                    var stats = SeasonStats.FirstOrDefault(s => s.TeamId == teamId && s.Season == result.Season);
                    if (stats == null)
                    {
                        stats = new SeasonStats
                        {
                            TeamId = teamId.Value,
                            Season = result.Season,
                            Wins = 0,
                            Draws = 0,
                            Losses = 0,
                            Points = 0,
                            GoalsScored = 0,
                            GoalsConceded = 0
                        };
                        SeasonStats.Add(stats);
                    }

                    // Aktualizujemy statystyki zależnie od tego, czy drużyna była gospodarzem czy gościem
                    if (teamId == result.HomeTeamId)
                    {
                        stats.GoalsScored += result.HomeTeamGoals ?? 0;
                        stats.GoalsConceded += result.AwayTeamGoals ?? 0;
                        UpdatePointsAndResults(stats, result.HomeTeamGoals, result.AwayTeamGoals);
                    }
                    else
                    {
                        stats.GoalsScored += result.AwayTeamGoals ?? 0;
                        stats.GoalsConceded += result.HomeTeamGoals ?? 0;
                        UpdatePointsAndResults(stats, result.AwayTeamGoals, result.HomeTeamGoals);
                    }
                }
            }
        }

        private void UpdatePointsAndResults(SeasonStats stats, int? teamGoals, int? opponentGoals)
        {
            if (teamGoals > opponentGoals)
            {
                stats.Wins++;
                stats.Points += 3;
            }
            else if (teamGoals == opponentGoals)
            {
                stats.Draws++;
                stats.Points += 1;
            }
            else
            {
                stats.Losses++;
            }
        }
    }
}
