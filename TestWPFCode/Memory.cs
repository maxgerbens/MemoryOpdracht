using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TestWPFCode
{
    public class Memory
    {
        public ObservableCollection<MemoryCard> Cards { get; }
        public Dictionary<ObservableCollection<MemoryCard>, bool> CardsStatus { get; set; } = new Dictionary<ObservableCollection<MemoryCard>, bool>();

        public Memory(int amount)
        {
            Cards = new ObservableCollection<MemoryCard>();
            MakeCards(amount);
        }

        private void MakeCards(int amount)
        {
            int cardid = 1;
            for (int i = 0; i < amount / 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Cards.Add(new MemoryCard(cardid, i + 1));
                    cardid++;
                }
            }
        }
    }
}
