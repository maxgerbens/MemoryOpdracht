using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MemoryGame.Commands;
using MemoryGame.Data;

namespace MemoryGame.Models
{
    public class MemoryCard : INotifyPropertyChanged
    {
        public static char[] Characters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        public static Image[] Images => DataAccess.GetImages();
        public static object BlueCard => new BitmapImage(new Uri(
            "C:/Users/njwna/source/repos/MemoryOpdrachtConsoleApp/MemoryGame/Images/VoorkantKaart/BlauwKaart.jpg", UriKind.RelativeOrAbsolute));
        public int CardID { get; } 
        public char CardCharacter { get; }
        public int CardPairID { get; }

        private object _cardImageFront;
        public object CardImageFront
        {
            get => _cardImageFront;
            set
            {
                _cardImageFront = value; 
                OnPropertyChanged(nameof(CardImageFront));
            }
        }

        private Image _cardImage;
        public Image CardImage
        {
            get => _cardImage;
            set
            {
                _cardImage = value; 
                OnPropertyChanged(nameof(CardImage));
            }
        }

        private bool _cardIsEnabled = true;
        public bool CardIsEnabled
        {
            get => _cardIsEnabled;
            set
            {
                _cardIsEnabled = value;
                OnPropertyChanged(nameof(CardIsEnabled));
            }
        }
        public ICommand CardClick => new CardClickCommand(this);

        public MemoryCard(int cardId, int cardPairId, char cardCharacter)
        {
            CardID = cardId;
            CardCharacter = cardCharacter;
            CardPairID = cardPairId;
        }
        public MemoryCard(int cardId, int cardPairId, Image image)
        {
            CardID = cardId;
            CardPairID = cardPairId;
            CardImage = image;
            CardImageFront = BlueCard;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
