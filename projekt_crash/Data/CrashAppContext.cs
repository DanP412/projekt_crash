using CrashApp.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrashApp.Data
{
    public class CrashAppContext : DbContext
    {
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<HighScore> HighScores { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CRASH;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>(entity =>
            {
                entity.ToTable("Players");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Username);
                entity.Property(p => p.Password);
                entity.Property(p => p.Balance);
                entity.Property(p => p.ContactId).HasColumnName("ContactsId");
                entity.HasOne(p => p.Contact).WithMany().HasForeignKey(p => p.ContactId);
            });

            //modelBuilder.Entity<Contact>(entity =>
            //{
            //    entity.ToTable("Contacts");
            //    entity.HasKey(c => c.Id);
            //    entity.Property(c => c.Email);
            //    entity.Property(c => c.PhoneNumber);
            //});

            //modelBuilder.Entity<Game>(entity =>
            //{
            //    entity.ToTable("Games");
            //    entity.HasKey(g => g.Id);
            //    entity.Property(g => g.Multiplier);
            //    entity.Property(g => g.Bet);
            //    entity.Property(g => g.Prize);
            //    entity.Property(g => g.GameWin);
            //    entity.Property(g => g.PlayerId).HasColumnName("PlayersId");
            //    entity.HasOne(p => p.Player).WithMany().HasForeignKey(p => p.PlayerId);
            //});

            //modelBuilder.Entity<HighScore>(entity =>
            //{
            //    entity.ToTable("HighScores");
            //    entity.HasKey(hs => hs.Id);
            //    entity.Property(hs => hs.Date);
            //    entity.Property(hs => hs.GameId).HasColumnName("GamesId");
            //    entity.HasOne(hs => hs.Game).WithMany().HasForeignKey(hs => hs.GameId);
            //});
        }
    }
}