using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MemoryGame.Data;
using MemoryGame.Models;
using MemoryGame.ViewModels;

namespace MemoryGame.Commands
{
    public class OpenFileDialogCommand : CommandBase
    {
        private readonly SettingsViewModel _viewModel;

        public OpenFileDialogCommand(SettingsViewModel viewModel)
        {
            _viewModel = viewModel;
        }
        public override void Execute(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Afbeeldingsbestanden|*.jpg;*.jpeg;*.png;*.gif;*.bmp|Alle bestanden|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedImagePath = openFileDialog.FileName;
                DataAccess.InsertImage(selectedImagePath);
                _viewModel.InvokeReload();
            }
        }
    }
}
