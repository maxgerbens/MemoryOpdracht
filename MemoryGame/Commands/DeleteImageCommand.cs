using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoryGame.Data;
using MemoryGame.Models;

namespace MemoryGame.Commands
{
    public class DeleteImageCommand : CommandBase
    {
        private readonly Image _image;
        public static event Action ImageDeleted;
        public DeleteImageCommand(Image image)
        {
            _image = image;
        }
        public override void Execute(object parameter)
        {
            DataAccess.DeleteImage(_image.ImageValue);
            OnImageDeleted();
        }

        private void OnImageDeleted()
        {
            ImageDeleted?.Invoke();
        }
    }
}
