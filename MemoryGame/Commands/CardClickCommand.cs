using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using MemoryGame.Models;
using MemoryGame.ViewModels;

namespace MemoryGame.Commands
{
    public class CardClickCommand : CommandBase
    {
        private readonly MemoryCard _memoryCard;
        private static List<MemoryCard> chosenCards = new List<MemoryCard>();
        public static event Action CheckIfWon;

        public CardClickCommand(MemoryCard memoryCard)
        {
            _memoryCard = memoryCard;
        }
        public override void Execute(object parameter)
        {
            if (chosenCards.Count <= 1)
            {
                _memoryCard.CardImageFront = _memoryCard.CardImage.ImagePicture;
                chosenCards.Add(_memoryCard);
                chosenCards[0].CardIsEnabled = false;
                MemoryBoardViewModel.OnChangeDescription("");
            }

            if (chosenCards.Count == 2)
            {
                if (chosenCards[0].CardPairID == chosenCards[1].CardPairID)
                {
                    chosenCards[0].CardIsEnabled = false;
                    chosenCards[1].CardIsEnabled = false;
                    MemoryBoardViewModel.OnChangeCardStatus(chosenCards[0].CardPairID);
                    ClearChosenCards();
                    MemoryBoardViewModel.OnChangeDescription("The pair is right!");
                    //MessageBox.Show("The pair is right!");
                }
                else
                {
                    MessageBox.Show("The pair is incorrect... try again!");
                    chosenCards[0].CardImageFront = MemoryCard.BlueCard;
                    chosenCards[1].CardImageFront = MemoryCard.BlueCard;
                    chosenCards[0].CardIsEnabled = true;
                    ClearChosenCards();
                }
                MemoryBoardViewModel.OnPlayerGuesses();
            }
            
            CheckIfWon?.Invoke();
        }

        public static void ClearChosenCards()
        {
            chosenCards.Clear();
        }

        public static void NullCheckIfWon()
        {
            CheckIfWon = null;
        }
    }
}
