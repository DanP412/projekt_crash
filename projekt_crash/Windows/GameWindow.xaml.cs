using CrashApp.Data.Entities;
using CrashApp.Models;
using CrashApp.Services;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;

namespace CrashApp.Windows
{
    public partial class GameWindow : Window
    {
        private readonly GameService _gameService;
        private readonly Player _signedInPlayer;

        public event Action OnSignOut;
        public event Action OnCrossExit;

        // Potrzebne do wyświetlania hajsu wbudowanym formatowaniem: zmiennaHajsu.ToString("C2", _usCultureInfo)
        private readonly CultureInfo _usCultureInfo = new CultureInfo("en-US");

        // Jest to dlaga, która umózliwa programowi rozróżnienie sytuacji, kiedy gracz chce się wylogować,od sytuacji kiedy chce zamknąć okno aplikacji
        private bool _closingBySigningOut;

        public GameWindow(Player signedInPlayer)
        {
            InitializeComponent();

            _signedInPlayer = signedInPlayer;

            _gameService = new GameService(_signedInPlayer);

            _gameService.OnMultiplierChange += _gameService_OnMultiplierChange;
            _gameService.OnGameFinish += _gameService_OnGameFinish;

            lblBalance.Content = $"Your balance: {signedInPlayer.Balance.ToString("C2", _usCultureInfo)}";
            lblWelcome.Content = $"Hi, {_signedInPlayer.Contact.Name} {_signedInPlayer.Contact.Surname}";
        }

        private void btnStartGame_Click(object sender, RoutedEventArgs e)
        {
            bool isInitialBetProperNumber = decimal.TryParse(tbxNewGameBet.Text, out decimal bet);

            if (!isInitialBetProperNumber)
            {
                MessageBox.Show("Incorrect value of bet!");
                return;
            }

            // Blokada opcji interfejsu w momencie rozpoczęcia gry
            EnableInterface(false);

            _signedInPlayer.Balance -= bet;

            lblBalance.Content = $"Your balance: {_signedInPlayer.Balance.ToString("C2", _usCultureInfo)}";

            lblBet.Content = $"Bet: {bet.ToString("C2", _usCultureInfo)}";

            btnStartGame.Visibility = Visibility.Collapsed;
            btnCASHOUT.Visibility = Visibility.Visible;

            _gameService.StartNewGame(bet);
        }

        // Logika wygranej lub przegranej gry
        private async void _gameService_OnGameFinish(FinishedGameResult finishedGameResult)
        {
            if (finishedGameResult.Game.GameWin)
            {
                string winMessage = string.Empty;

                winMessage += finishedGameResult.IsNewHighScore ? $"HIGH SCORE!\n" : string.Empty;

                winMessage += $"You won {finishedGameResult.Game.Prize.ToString("C2", _usCultureInfo)}!\n";

                winMessage += $"High Score Place: {finishedGameResult.HighScorePlace}";

                MessageBox.Show(winMessage);

                _signedInPlayer.Balance += finishedGameResult.Game.Prize;

                lblBalance.Content = $"Your balance: {_signedInPlayer.Balance.ToString("C2", _usCultureInfo)}";
            }
            else
            {
                MessageBox.Show($"Game over!\nYou lost your bet money: {finishedGameResult.Game.Bet}$.\nYou could have won: {finishedGameResult.Game.Prize.ToString("C2", _usCultureInfo)}");
            }

            await _gameService.SaveGameAsync(finishedGameResult.Game);

            btnStartGame.Visibility = Visibility.Visible;
            btnCASHOUT.Visibility = Visibility.Collapsed;

            tbxNewGameBet.Clear();
            lblBet.Content = "Bet:";
            lblPrize.Content = "Current prize:";
            lblMultiplier.Content = "0x";

            // Odblokowanie opcji interfejsu po zakończeniu gry
            EnableInterface(true);
        }

        // Obsługa zdarzenia OnMultiplierChange klasy GameService
        private void _gameService_OnMultiplierChange(decimal newMultiplierValue, decimal updatedPrize)
        {
            lblMultiplier.Content = $"x{newMultiplierValue}";
            lblPrize.Content = $"Current prize: {updatedPrize.ToString("C2", _usCultureInfo)}";
        }

        private void btnFAQ_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Jedna linijka.\nDruga linijka\n");
        }

        private void btnShowHighScoreWindow_Click(object sender, RoutedEventArgs e)
        {
            var highScoreWindow = new HighScoreWindow(_signedInPlayer);

            highScoreWindow.ShowDialog();
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            tbxNewGameBet.Text = "10";
        }

        private void btn100_Click(object sender, RoutedEventArgs e)
        {
            tbxNewGameBet.Text = "100";
        }

        private void btnBetHalf_Click(object sender, RoutedEventArgs e)
        {
            bool isInitialBetProperNumber = decimal.TryParse(tbxNewGameBet.Text, out decimal bet);

            if (!isInitialBetProperNumber)
            {
                MessageBox.Show("Incorrect value of bet!");
                return;
            }

            decimal halfOfBet = bet / 2;

            tbxNewGameBet.Text = halfOfBet.ToString();
        }

        private void btnMax_Click(object sender, RoutedEventArgs e)
        {
            tbxNewGameBet.Text = _signedInPlayer.Balance.ToString();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            tbxNewGameBet.Clear();
        }

        private async void btnCASHOUT_Click(object sender, RoutedEventArgs e)
        {
            await _gameService.StopCurrentGameAsync();
        }

        private void btnSignOut_Click(object sender, RoutedEventArgs e)
        {
            _closingBySigningOut = true;
            // Zmiana flagi => gracz się wylogowywuje
            OnSignOut?.Invoke();
            Close();
        }

        // Zmiana flagi => gracz zamyka aplikacje
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (!_closingBySigningOut)
            {
                OnCrossExit?.Invoke(); 
            }
        }

        // Metoda blokująca interfejs
        private void EnableInterface(bool enabled)
        {
            btnStartGame.IsEnabled = enabled;
            btnFAQ.IsEnabled = enabled;
            btnShowHighScoreWindow.IsEnabled = enabled;
            btnMin.IsEnabled = enabled;
            btn100.IsEnabled = enabled;
            btnBetHalf.IsEnabled = enabled;
            btnMax.IsEnabled = enabled;
            btnClear.IsEnabled = enabled;
            btnSignOut.IsEnabled = enabled;

            tbxNewGameBet.IsEnabled = enabled;

            // Nie blokujemy przycisku Cashout
        }
    }
}