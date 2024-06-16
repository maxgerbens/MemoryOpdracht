using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using MemoryGame.Commands;
using MemoryGame.Data;
using MemoryGame.Models;
using MemoryGame.Navigation;

namespace MemoryGame.ViewModels
{
    public class MemoryBoardViewModel : ViewModelBase
    {
        private readonly string _name;
        public WPFGame WpfGame { get; }

        private int _amountOfGuesses;
        public int AmountOfGuesses
        {
            get => _amountOfGuesses;
            set
            {
                _amountOfGuesses = value;
                OnPropertyChanged(nameof(AmountOfGuesses));
            }
        }

        private TimeSpan _timer;
        public TimeSpan Timer
        {
            get => _timer;
            set
            {
                _timer = value;
                OnPropertyChanged(nameof(Timer));
            }
        }

        private double _score;
        public double Score
        {
            get => _score;
            set
            {
                _score = value;
                OnPropertyChanged(nameof(Score));
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public static event Action OnPlayerGuess;

        public static event Action<int> ChangeCardStatus;
        public static event Action<string> ChangeDescription;
        public ICommand Retry { get; }
        public ICommand ToHomePage { get; }
        public MemoryBoardViewModel(int amount, string name, NavigationStore navigationStore, HomePageViewModel viewModel)
        {
            _name = name;
            WpfGame = new WPFGame(amount);
            WpfGame.Timer.Start();
            OnPlayerGuess = () => { WpfGame.AmountGuessed++; AmountOfGuesses = WpfGame.AmountGuessed; };
            ChangeCardStatus = pairId =>
            {
                WpfGame.CardStatus[WpfGame.CardStatus.FirstOrDefault(id => id.Key.CardPairID == pairId).Key] = true;
            };
            CardClickCommand.CheckIfWon += () => CheckIfWon(amount);
            ChangeDescription += (message) => Description = message;
            Retry = new StartGameCommand(navigationStore, viewModel, nameof(Retry));
            ToHomePage = new HomePageCommand(navigationStore);
        }
        public static void OnPlayerGuesses()
        {
            OnPlayerGuess?.Invoke();
        }

        public static void OnChangeCardStatus(int pairid)
        {
            ChangeCardStatus?.Invoke(pairid);
        }

        public static void OnChangeDescription(string message)
        {
            ChangeDescription?.Invoke(message);
        }

        public static void ResetActions()
        {
            ChangeCardStatus = null;
            OnPlayerGuess = null;
            ChangeDescription = null;
            CardClickCommand.NullCheckIfWon();
        }

        public void CheckIfWon(int amount)
        {
            if (!WpfGame.CardStatus.ContainsValue(false))
            {
                CardClickCommand.ClearChosenCards();
                ChangeCardStatus = null;
                OnPlayerGuess = null;
                ChangeDescription = null;
                CardClickCommand.NullCheckIfWon();

                WpfGame.Timer.Stop();
                Timer = WpfGame.Timer.Elapsed;
                //Formula for checking the highscore.
                Score = Math.Round(Math.Pow(amount, 2) / (Timer.Seconds * AmountOfGuesses) * 1000);
                MessageBox.Show("You have guessed all the cards! Your score is visible at the bottom of the page.");
                if (DataAccess.CheckIfIsHighscore(amount, _name, (int)Score))
                {
                    MessageBox.Show("Your score is a high score, Congratulations!");
                }
                else
                {
                    MessageBox.Show("Unfortunately your score is not a high score... Better luck next time!");
                }
            }
        }
    }
}
