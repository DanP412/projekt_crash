using CrashApp.Data.Entities;
using CrashApp.Services;
using System.Windows;

namespace CrashApp.Windows
{
    public partial class HighScoreWindow : Window
    {
        private readonly HighScoreService _highScoreService = new HighScoreService();
        private readonly Player _signedInPlayer;

        public HighScoreWindow(Player signedInPlayer)
        {
            InitializeComponent();

            _signedInPlayer = signedInPlayer;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var highScoreGridRowResults = await _highScoreService.GetAllHighScoresAsync(_signedInPlayer);

            dGridHighScores.ItemsSource = highScoreGridRowResults;
        }
    }
}