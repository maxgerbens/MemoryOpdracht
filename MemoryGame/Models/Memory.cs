using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MemoryGame.Models
{
    public abstract class Memory
    {
        public ObservableCollection<MemoryCard> Cards { get; }
        public Dictionary<MemoryCard, bool> CardStatus { get; set; }
        public int AmountGuessed { get; set; }
        public Stopwatch Timer { get; set; }

        protected Memory(int amount)
        {
            Cards = new ObservableCollection<MemoryCard>();
            CardStatus = new Dictionary<MemoryCard, bool>();
            Timer = new Stopwatch();
            MakeCards(amount);
            ShuffleCards();
        }

        protected abstract void MakeCards(int amount);
        protected abstract void ShuffleCards();
    }
}
