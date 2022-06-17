namespace CrashApp.Data.Entities
{
    public class Game : EntityBase
    {
        public virtual int PlayerId { get; set; }
        public virtual Player Player { get; set; }

        public decimal Multiplier { get; set; }

        public decimal Bet { get; set; }

        public decimal Prize { get; set; }

        public bool GameWin { get; set; }
    }
}