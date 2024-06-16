using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoryGame.Navigation;
using MemoryGame.ViewModels;

namespace MemoryGame.Commands
{
    internal class HighscoreCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;

        public HighscoreCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }
        public override void Execute(object parameter)
        {
            _navigationStore.CurrentViewModel = new HighscoresViewModel(_navigationStore);
        }
    }
}
