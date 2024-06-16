using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MemoryGame.Commands;

namespace MemoryGame.Models
{
    public class Image
    {
        public byte[] ImageValue { get; set; }
        public object ImagePicture { get; set; }
        public ICommand DeleteImage => new DeleteImageCommand(this);
        public Image(object imagePicture)
        {
            ImagePicture = imagePicture;
        }
        public Image(byte[] imageValue, object imagePicture) : this(imagePicture)
        {
            ImageValue = imageValue;
        }
    }
}
