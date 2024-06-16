using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MemoryGame.Commands;
using MemoryGame.Models;
using MemoryGame.Navigation;

namespace MemoryGame.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private ObservableCollection<int> _amountOfPlayingCards = new ObservableCollection<int>();
        public ObservableCollection<int> AmountOfPlayingCards
        {
            get => _amountOfPlayingCards;
            set
            {
                _amountOfPlayingCards = value;
                OnPropertyChanged(nameof(AmountOfPlayingCards));
            }
        }

        private int _selectedAmountOfPlayingCards;
        public int SelectedAmountOfPlayingCards
        {
            get => _selectedAmountOfPlayingCards;
            set
            {
                _selectedAmountOfPlayingCards = value;
                OnPropertyChanged(nameof(SelectedAmountOfPlayingCards));
            }
        }

        public ICommand StartGame { get; }
        public ICommand ShowHighScores { get; }
        public ICommand Settings { get; }
        public HomePageViewModel(NavigationStore navigationStore)
        {
            AmountOfPlayingCards = SetAmountOfPlayingCards();
            StartGame = new StartGameCommand(navigationStore, this, nameof(StartGame));
            ShowHighScores = new HighscoreCommand(navigationStore);
            Settings = new SettingsCommand(navigationStore);
        }

        private ObservableCollection<int> SetAmountOfPlayingCards()
        {
            ObservableCollection<int> playingsCards = new ObservableCollection<int>();
            int amountOfPossiblePlayingCards = MemoryCard.Images.Length * 2;
            int i = 0;
            while (amountOfPossiblePlayingCards - i >= 2)
            {
                playingsCards.Add(amountOfPossiblePlayingCards - i);
                i += 2;
            }

            if (playingsCards.Count != 0)
                SelectedAmountOfPlayingCards = 0;
            else
                SelectedAmountOfPlayingCards = -1;

            return playingsCards;
        }
    }
}
