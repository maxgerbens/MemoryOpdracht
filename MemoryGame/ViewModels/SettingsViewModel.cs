using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using MemoryGame.Commands;
using MemoryGame.Data;
using MemoryGame.Models;
using MemoryGame.Navigation;

namespace MemoryGame.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public ICommand OpenFileDialog { get; }
        public ICommand Homepage { get; }

        private ObservableCollection<Image> _images;
        public ObservableCollection<Image> Images
        {
            get => _images;
            set
            {
                _images = value;
                OnPropertyChanged(nameof(Images));
            }
        } 

        public SettingsViewModel(NavigationStore navigationStore)
        {
            InvokeReload();
            DeleteImageCommand.ImageDeleted += InvokeReload;
            Homepage = new HomePageCommand(navigationStore);
            OpenFileDialog = new OpenFileDialogCommand(this);
        }

        public void InvokeReload()
        {
            Images = new ObservableCollection<Image>(DataAccess.GetImages());
        }
    }
}
