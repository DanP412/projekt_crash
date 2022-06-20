using CrashApp.Data.Entities;

namespace CrashApp.Models
{
    public class InitialPlayerResult
    {
        public Player InitialPlayer { get; set; }

        public bool WasInitialPlayerCreated => InitialPlayer != null;
    }
}