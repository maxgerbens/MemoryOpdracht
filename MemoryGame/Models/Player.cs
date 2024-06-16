using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame.Models
{
    public class Player
    {
        public int Position { get; set; }
        public string PlayerName { get; }
        public int HighScore { get; set; }
        public int AmountOfCards { get; }

        public Player(string playerName, int highScore, int amountOfCards)
        {
            PlayerName = playerName;
            HighScore = highScore;
            AmountOfCards = amountOfCards;
        }
    }
}
