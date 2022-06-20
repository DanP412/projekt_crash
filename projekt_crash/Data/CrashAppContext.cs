using CrashApp.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrashApp.Data
{
    // Klasa biblioteki ORMu (Object Relational Mapping) o nazwie Entity Framework Core
    // Żeby EF Core działał trzeba stworzyć klasę i dziedziczyć po DbContext
    public class CrashAppContext : DbContext
    {
        // Te "listy" to bezpośrednie źródło każdej z tabel bazodanowych (można używać metod Linq w celu wyciągania danych)
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<HighScore> HighScores { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }

        // Metoda z uwzględnieniem connection stringa
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CRASH;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        // Metoda z informacjami o mapowaniu encji z tabelami
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>(entityTypeBuilder =>
            {
                entityTypeBuilder.ToTable("Players");
                entityTypeBuilder.HasKey(p => p.Id);
                entityTypeBuilder.Property(p => p.Username);
                entityTypeBuilder.Property(p => p.Password);
                entityTypeBuilder.Property(p => p.Balance);
                entityTypeBuilder.Property(p => p.ContactId).HasColumnName("ContactsId");
                entityTypeBuilder.HasOne(p => p.Contact).WithMany().HasForeignKey(p => p.ContactId);
                entityTypeBuilder.Navigation(p => p.Contact).AutoInclude();
            });

            modelBuilder.Entity<Contact>(entityTypeBuilder =>
            {
                entityTypeBuilder.ToTable("Contacts");
                entityTypeBuilder.HasKey(c => c.Id);
                entityTypeBuilder.Property(c => c.Name);
                entityTypeBuilder.Property(c => c.Surname);
                entityTypeBuilder.Property(c => c.Email);
                entityTypeBuilder.Property(c => c.PhoneNumber);
            });

            modelBuilder.Entity<Game>(entityTypeBuilder =>
            {
                entityTypeBuilder.ToTable("Games");
                entityTypeBuilder.HasKey(g => g.Id);
                entityTypeBuilder.Property(g => g.Multiplier);
                entityTypeBuilder.Property(g => g.Bet);
                entityTypeBuilder.Property(g => g.Prize);
                entityTypeBuilder.Property(g => g.GameWin);
                entityTypeBuilder.Property(g => g.PlayerId).HasColumnName("PlayersId");
                entityTypeBuilder.HasOne(p => p.Player).WithMany().HasForeignKey(p => p.PlayerId);
                entityTypeBuilder.Navigation(g => g.Player).AutoInclude();
            });

            modelBuilder.Entity<HighScore>(entityTypeBuilder =>
            {
                entityTypeBuilder.ToTable("HighScores");
                entityTypeBuilder.HasKey(hs => hs.Id);
                entityTypeBuilder.Property(hs => hs.Date);
                entityTypeBuilder.Property(hs => hs.GameId).HasColumnName("GamesId");
                entityTypeBuilder.HasOne(hs => hs.Game).WithMany().HasForeignKey(hs => hs.GameId);
                entityTypeBuilder.Navigation(hs => hs.Game).AutoInclude();
            });
        }
    }
}