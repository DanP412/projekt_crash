using CrashApp.Data.Entities;
using CrashApp.Services;
using System.Windows;

namespace CrashApp.Windows
{
    public partial class NewPlayerWindow : Window
    {
        private readonly PlayerService _playerService = new PlayerService();

        public Player NewPlayer { get; private set; }

        public NewPlayerWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string username = tbxUsername.Text;
            string password = tbxPassword.Text;
            string name = tbxName.Text;
            string surname = tbxSurname.Text;
            string emailAddress = tbxEmailAddress.Text;
            string phoneNumber = tbxPhoneNumber.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Username is required.");
                return;
            }

            if (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Password is required.");
                return;
            }

            if (password.Length < 6)
            {
                MessageBox.Show("Password needs to contain minimum 6 characters.");
                return;
            }

            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Name is required.");
                return;
            }

            if (string.IsNullOrEmpty(surname) || string.IsNullOrWhiteSpace(surname))
            {
                MessageBox.Show("Surname is required.");
                return;
            }

            if (string.IsNullOrEmpty(emailAddress) || string.IsNullOrWhiteSpace(emailAddress))
            {
                MessageBox.Show("Email address is required.");
                return;
            }

            if (string.IsNullOrEmpty(phoneNumber) || string.IsNullOrWhiteSpace(phoneNumber))
            {
                MessageBox.Show("Phone number is required.");
                return;
            }

            bool playerWithSpecifiedUsernameExists = await _playerService.PlayerWithSpecifiedUsernameExists(tbxUsername.Text);

            if (playerWithSpecifiedUsernameExists)
            {
                MessageBox.Show("Username is already in use.");
                return;
            }

            NewPlayer = await _playerService.CreateNewPlayerAsync(username, password, name, surname, emailAddress, phoneNumber);

            MessageBox.Show("User was created successfully!");

            DialogResult = true;
        }
    }
}