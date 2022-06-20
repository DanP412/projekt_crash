using CrashApp.Constants;
using CrashApp.Data;
using CrashApp.Data.Entities;
using CrashApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CrashApp.Services
{
    public class PlayerService
    {
        public async Task<InitialPlayerResult> CreateInitialUserIfNeededAsync()
        {
            bool anyPlayerExists = await StaticContext.Context.Players.AnyAsync();

            bool shouldCreateInitialPlayer = !anyPlayerExists;

            var initialPlayerResult = new InitialPlayerResult();

            if (shouldCreateInitialPlayer)
            {
                initialPlayerResult.InitialPlayer = await CreateNewPlayerAsync("a", "a", "A_Imię", "A_Nazwisko", "aaaa@aa.aa", "69 69 69 69 69");
            }

            return initialPlayerResult;
        }

        public async Task<Player> CreateNewPlayerAsync(string username, string password, string name, string surname, string email, string phoneNumber)
        {
            var player = new Player
            {
                Username = username,
                Password = password,
                Balance = SettingConstants.INITIAL_BALANCE,
                Contact = new Contact
                {
                    Name = name,
                    Surname = surname,
                    Email = email,
                    PhoneNumber = phoneNumber
                }
            };

            // Po utworzeniu nowego gracza, trzeba pobrać nowy obiekt z bazy danych przez "entityEntry.Entity"
            // (wtedy gracz ma uzupełnione id i jest znany bazie danych
            
            var entityEntry = StaticContext.Context.Players.Add(player);
            await StaticContext.Context.SaveChangesAsync();

            var newPlayer = entityEntry.Entity;

            return newPlayer;
        }

        public async Task<bool> PlayerWithSpecifiedUsernameExists(string username)
        {
            bool playerWithSpecifiedUsernameExists = await StaticContext.Context.Players.AnyAsync(p => p.Username == username);

            return playerWithSpecifiedUsernameExists;
        }
    }
}