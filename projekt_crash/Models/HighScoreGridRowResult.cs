using CrashApp.Data.Entities;

namespace CrashApp.Models
{
    public class HighScoreGridRowResult
    {
        public int Number { get; set; }

        public string PlayerDescription => $"{HighScore.Game.Player.Contact.Name} {HighScore.Game.Player.Contact.Surname}";

        public HighScore HighScore { get; set; }

        public bool IsHighlighted { get; set; }
    }
}