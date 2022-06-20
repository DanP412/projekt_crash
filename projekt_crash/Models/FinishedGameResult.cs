using CrashApp.Data.Entities;

namespace CrashApp.Models
{
    public class FinishedGameResult
    {
        public Game Game { get; set; }

        public bool IsNewHighScore { get; set; }

        public int HighScorePlace { get; set; }
    }
}