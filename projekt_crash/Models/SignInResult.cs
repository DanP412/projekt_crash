using CrashApp.Data.Entities;

namespace projekt_crash.Models
{
    public class SignInResult
    {
        public bool SignInSuccess { get; set; }

        public Player SignedInPlayer { get; set; }
    }
}
