namespace CrashApp.Data
{
    // Statyczna, bo posiada tylko 1 instancje tego contextu
    public static class StaticContext
    {
        public static CrashAppContext Context { get; } = new CrashAppContext();

        // Konstruktor klasy statycznej wykonuje się TYLKO 1 raz po 1 użyciu klasy statycznej
        static StaticContext()
        {
            // Metoda sprawdzająca, czy baza istnieje. Jeśli nie, wygeneruje ją na podstawie kontekstu (metoda CrashAppContext.OnModelCreating)
            Context.Database.EnsureCreated();
        }
    }
}   