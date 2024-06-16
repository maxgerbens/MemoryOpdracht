using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoryGame.Navigation;
using MemoryGame.ViewModels;

namespace MemoryGame.Commands
{
    public class SettingsCommand : CommandBase
    {
        private readonly NavigationStore _navigation;

        public SettingsCommand(NavigationStore navigation)
        {
            _navigation = navigation;
        }
        public override void Execute(object parameter)
        {
            _navigation.CurrentViewModel = new SettingsViewModel(_navigation);
        }
    }
}
