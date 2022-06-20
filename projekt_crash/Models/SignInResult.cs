using CrashApp.Data.Entities;

namespace CrashApp.Models
{
    public class SignInResult
    {
        public bool SignInSuccess { get; set; }

        public Player SignedInPlayer { get; set; }
    }
}