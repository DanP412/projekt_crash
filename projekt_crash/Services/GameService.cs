using CrashApp.Data.Entities;
using System;
using System.Security.Cryptography;
using System.Timers;

namespace projekt_crash.Services
{
    public class GameService
    {
        private readonly int _multiplierInterval = 200;

        private int _millisecondsToStopTheGame;
        private int _currentMillisecondsPassed;

        private readonly Timer _multiplierTimer;

        private decimal _multiplier = 0;

        public event Action<decimal> OnMultiplierChange;
        public event Action<Game> OnGameFinish;

        private Player _player;

        private decimal _bet;

        private bool _isGameRunning;

        public GameService(Player player)
        {
            _player = player;
            _multiplierTimer = new Timer(_multiplierInterval);
            _multiplierTimer.Elapsed += _timer_Elapsed;
        }

        public void StartNewGame(decimal bet)
        {
            if (_isGameRunning)
            {
                throw new InvalidOperationException("Gra jest już w toku.");
            }

            _isGameRunning = true;
            _bet = bet;
            _currentMillisecondsPassed = 0;
            _millisecondsToStopTheGame = RandomNumberGenerator.GetInt32(3000, 10_000);
        }

        public void StopCurrentGame()
        {
            if (!_isGameRunning)
            {
                throw new InvalidOperationException("Żadna gra nie jest w toku.");
            }

            _isGameRunning = false;

            var wonGame = CreateGame(true);
            OnGameFinish?.Invoke(wonGame);
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _multiplier += 0.1m;
            OnMultiplierChange?.Invoke(_multiplier);
            _currentMillisecondsPassed += _multiplierInterval;

            if (_currentMillisecondsPassed >= _millisecondsToStopTheGame)
            {
                _multiplierTimer.Stop();
                var lostGame = CreateGame(false);
                OnGameFinish?.Invoke(lostGame);
            }
        }

        private Game CreateGame(bool isWin)
        {
            var game = new Game
            {
                Player = _player,
                Multiplier = _multiplier,
                Bet = _bet,
                Prize = isWin ? _bet * _multiplier : 0,
                GameWin = isWin
            };

            return game;
        }
    }
}
