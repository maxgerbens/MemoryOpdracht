using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWPFCode
{
    public class MemoryCard
    {
        public static string[] Images => Directory.GetFiles("C:/Users/njwna/source/repos/MemoryOpdrachtConsoleApp/TestWPFCode/Images");
        public int CardID { get; }
        public int CardPairID { get; }

        public MemoryCard(int cardId, int cardPairID)
        {
            CardID = cardId;
            CardPairID = cardPairID;
        }
    }
}
