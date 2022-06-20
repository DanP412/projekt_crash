using CrashApp.Constants;
using CrashApp.Data;
using CrashApp.Data.Entities;
using CrashApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrashApp.Services
{
    public class HighScoreService
    {
        public async Task SaveGameAsHighScoreAsync(Game game)
        {
            int highScoreCount = await StaticContext.Context.HighScores.CountAsync();

            bool highScoreNeedsToBeDeleted = highScoreCount >= SettingConstants.HIGH_SCORE_GAME_MAX_AMOUNT;

            if (highScoreNeedsToBeDeleted)
            {
                var allHighScores = await StaticContext.Context.HighScores.ToListAsync();

                decimal worstHighScorePrize = allHighScores.Min(hs => hs.Game.Prize);

                // W przypadku kiedy mamy kilka takich samych high scorów z taką samą wygraną, bierzemy obojętnie którego z nich
                var worstHighScore = allHighScores.First(hs => hs.Game.Prize == worstHighScorePrize);

                // Usunięcie najgorszego high scoru
                StaticContext.Context.HighScores.Remove(worstHighScore);
                await StaticContext.Context.SaveChangesAsync();
            }

            var newHighScore = new HighScore
            {
                Date = DateTime.Now,
                Game = game
            };

            // Dodanie nowego high scoru. Miejsce wynika z warotści prize, więc da się je zawsze wyliczyć
            StaticContext.Context.HighScores.Add(newHighScore);
            await StaticContext.Context.SaveChangesAsync();
        }

        public async Task<int> GetGameHighScorePlaceAsync(Game game)
        {
            var allHighScores = await StaticContext.Context.HighScores.ToListAsync();

            var allGames = allHighScores.Select(hs => hs.Game);

            var betterPrizeGameCount = allGames.Count(g => g.Prize >= game.Prize);

            int gameHighScorePlace = betterPrizeGameCount + 1;

            return gameHighScorePlace;
        }

        public async Task<List<HighScoreGridRowResult>> GetAllHighScoresAsync(Player signedInPlayer)
        {
            var allHighScores = await StaticContext.Context.HighScores.ToListAsync();

            var orderedHighScores = allHighScores.OrderByDescending(hs => hs.Game.Prize);

            var highScoreGridRowResults = orderedHighScores.Select((hs, i) =>
            {
                int number = i + 1;
                bool isHighlighted = hs.Game.Player == signedInPlayer;

                var highScoreGridRowResult = new HighScoreGridRowResult 
                { 
                    Number = number,
                    HighScore = hs,
                    IsHighlighted = isHighlighted
                };

                return highScoreGridRowResult;
            }).ToList();

            return highScoreGridRowResults;
        }
    }
}