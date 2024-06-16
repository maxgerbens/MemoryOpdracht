using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MemoryGame.Commands;
using MemoryGame.Data;
using MemoryGame.Models;
using MemoryGame.Navigation;

namespace MemoryGame.ViewModels
{
    public class HighscoresViewModel : ViewModelBase
    {
        public ObservableCollection<Player> HighScores { get; } = new ObservableCollection<Player>();
        public ICommand Homepage { get; }
        public HighscoresViewModel(NavigationStore navigationStore)
        {
            HighScores = DataAccess.GetHighscores();
            Homepage = new HomePageCommand(navigationStore);
        }
    }
}
