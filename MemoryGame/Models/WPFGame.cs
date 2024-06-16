using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MemoryGame.Models
{
    public class WPFGame : Memory
    {
        public WPFGame(int amount) : base(amount) { }
        protected override void MakeCards(int amount)
        {
            int i = 1;
            for (int pairId = 0; pairId < amount/2; pairId++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Cards.Add(new MemoryCard(i, pairId, MemoryCard.Images[pairId]));
                    i++;
                }
            }

            for (int s = 0; s < Cards.Count; s++)
            {
                var card = Cards[s];
                CardStatus.Add(card, false);
                s++;
            }
        }
        protected override void ShuffleCards()
        {
            Random rand = new Random();
            int n = Cards.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                MemoryCard value = Cards[k];
                Cards[k] = Cards[n];
                Cards[n] = value;
            }
        }
    }
}
