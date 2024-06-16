using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MemoryGame.Navigation;
using MemoryGame.ViewModels;

namespace MemoryGame.Commands
{
    public class StartGameCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly HomePageViewModel _viewModel;
        private readonly string _propName;

        public StartGameCommand(NavigationStore navigationStore, HomePageViewModel viewModel, string propName)
        {
            _navigationStore = navigationStore;
            _viewModel = viewModel;
            _propName = propName;
        }

        public override void Execute(object parameter)
        {
            //TODO: Niet hardcoded
            if (_propName == "Retry")
            {
                MessageBox.Show("Game will be restarted. After clicking OK, the game will begin.");
                MemoryBoardViewModel.ResetActions();
                CardClickCommand.ClearChosenCards();
            }
            if (!string.IsNullOrEmpty(_viewModel.Name))
                _navigationStore.CurrentViewModel =
                    new MemoryBoardViewModel(_viewModel.AmountOfPlayingCards[_viewModel.SelectedAmountOfPlayingCards], _viewModel.Name, _navigationStore, _viewModel);
            else
                MessageBox.Show("A playername is required!");
        }
    }
}
