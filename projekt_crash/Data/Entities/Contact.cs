namespace CrashApp.Data.Entities
{
    public class Contact : EntityBase
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}